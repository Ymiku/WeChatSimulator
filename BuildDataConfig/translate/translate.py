#!/usr/bin/env python
#encoding: utf-8
import re, os, sys, xlwt, xlrd, xlutils, md5
from __init__ import TranslationDatabase, TranslationTable, DatabaseType, FieldIndex
from __init__ import DATA_PATH, PATTERN_XLS_NAME, PATTERN_XLS_SUFFIX, PATTERN_FIELD_NAME
from checker import base_check
from xlutils.copy import copy
import prefab, csharp

class ProjectTranslator(base_check.BaseChecker):
    def __init__(self):
        super(ProjectTranslator, self).__init__(None, xls_root_path = DATA_PATH)
        self.library = TranslationDatabase(type = DatabaseType.LIBRARY)

    def __translate_to_prefab(self, field_index):
        table = self.library.get_table(prefab.PREFAB_SHEET_NAME)
        text_map = table.strip_text_map(field_index)
        prefab.translate(text_map)

    def __translate_to_script(self, field_index):
        table = self.library.get_table(csharp.SCRIPT_SHEET_NAME)
        text_map = table.strip_text_map(field_index)
        csharp.translate(text_map)

    def __translate_image(self, field_index):
        import image
        image_translator = image.ImageTranslator()
        image_translator.translate(field_index)

    def translate_to_project(self, lang_field_index):
        for xls_file in os.listdir(DATA_PATH):
            if PATTERN_XLS_NAME.match(xls_file) == None:
                continue
            xls_name = PATTERN_XLS_SUFFIX.sub('', xls_file)
            src_book = self.get_book(xls_name)
            dst_book = copy(src_book)
            book_changed = False
            for sheet_name in src_book.sheet_names():
                src_sheet = src_book.sheet_by_name(sheet_name)
                dst_sheet_index = dst_book.sheet_index(sheet_name)
                dst_sheet = dst_book.get_sheet(dst_sheet_index)
                if not self.library.has_table(sheet_name):
                    continue
                table = self.library.get_table(sheet_name)
                translate_map = table.strip_translate_map(lang_field_index)
                for c in range(src_sheet.ncols):
                    field_name = unicode(src_sheet.cell(base_check.INDEX_NAME_ROW, c).value).strip()
                    if PATTERN_FIELD_NAME.match(field_name) == None:
                        continue
                    if not translate_map.has_key(field_name.lower()):
                        continue
                    text_map = translate_map[field_name.lower()]
                    index_field_name = text_map['INDEX_FIELD']
                    if index_field_name == u'HASH':
                        for r in range(base_check.INDEX_DATA_ROW, src_sheet.nrows):
                            src_text = unicode(src_sheet.cell(r, c).value).strip()
                            key_hash = md5.md5(src_text.encode('utf-8')).hexdigest()
                            if text_map.has_key(key_hash):
                                dst_sheet.write(r, c, text_map[key_hash])
                                book_changed = True
                    else:
                        id_column_index = self.get_column_indice(src_sheet, base_check.INDEX_NAME_ROW, index_field_name)[0]
                        for r in range(base_check.INDEX_DATA_ROW, src_sheet.nrows):
                            id_value = re.sub(r'\.0$', '', unicode(src_sheet.cell(r, id_column_index).value))
                            if text_map.has_key(id_value):
                                dst_sheet.write(r, c, text_map[id_value])
                                book_changed = True
            if book_changed:
                dst_path = self.get_book_path(xls_name)
                dst_book.save(dst_path)
                print 'TRANSLATED => %s'%(dst_path)
        self.__translate_to_prefab(lang_field_index)
        self.__translate_to_script(lang_field_index)
        self.__translate_image(lang_field_index)

def main():
    proj_translator = ProjectTranslator()
    proj_translator.translate_to_project(lang_field_index = FieldIndex.JAPANESE)

if __name__ == '__main__':
    main()