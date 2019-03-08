#!/usr/bin/env python
#encoding: utf-8
import os, sys, re, xlrd
from __init__ import TranslationDatabase, TranslationTable, DatabaseType, TranslationMergeReport
from __init__ import HeaderIndex, FieldIndex, PATTERN_XLS_SUFFIX, WORK_PATH
from checker.xlsdata.utils import os_encode
import utils, html

class TranslationMerger(object):
    def __init__(self):
        self.library = TranslationDatabase(type = DatabaseType.LIBRARY)
        self.merge = TranslationDatabase(type = DatabaseType.MERGE)

    def merge_to_library(self, lang_field_index):
        self.merge.clear()
        commits_dir = os.path.join(WORK_PATH, 'commits')
        for file_name in os.listdir(commits_dir):
            if not PATTERN_XLS_SUFFIX.search(file_name) or re.search(r'^~', file_name):
                continue
            translation = TranslationDatabase(type = DatabaseType.MEMORY)
            translation.load(os.path.join(commits_dir, file_name))
            for src_table in translation.tables:
                mrg_table = self.merge.get_table(src_table.name)
                mrg_table.merge_from_translation(src_table, field_index = lang_field_index, update_indice = [FieldIndex.CHINESE], verbose = False)
        self.merge.save(verbose = False)

        report = TranslationMergeReport()
        report.generate(self.merge, self.library, lang_field_index)
        report.save(re.sub(r'(\.xls)$', r'_detail\g<1>', self.merge.path))
        
        for src_table in self.merge.tables:
            dst_table = self.library.get_table(src_table.name)
            dst_table.merge_from_translation(src_table, field_index = lang_field_index, update_indice = [FieldIndex.CHINESE])
        self.library.save(verbose = False)
    
def main():
    merger = TranslationMerger()
    merger.merge_to_library(lang_field_index = FieldIndex.JAPANESE)
    html.validate()

if __name__ == '__main__':
    main()