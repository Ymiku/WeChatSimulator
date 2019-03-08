#!/usr/bin/env python
#encoding: utf-8
import os, sys, re, xlrd, md5
import prefab
from __init__ import DATA_PATH, PATTERN_XLS_NAME, PATTERN_XLS_SUFFIX, PATTERN_CHINESE, PATTERN_FIELD_NAME
from __init__ import DatabaseType, FieldIndex, TranslationDatabase, TranslationTable, TranslationCompareReport
from checker import base_check
from checker.base_check import INDEX_RULE_ROW, INDEX_DATA_ROW, INDEX_NAME_ROW
from checker.xlsdata.utils import os_encode
import csharp

class TranslationCollector(base_check.BaseChecker):
    def __init__(self):
        super(TranslationCollector, self).__init__(xls_name = None, xls_root_path = DATA_PATH)
        self.library = TranslationDatabase(type = DatabaseType.LIBRARY)
        self.scan = TranslationDatabase(type = DatabaseType.MEMORY)
    
    def __get_sheet_id_column_index(self, sheet):
        column_indice = self.get_column_indice(sheet, INDEX_NAME_ROW, 'id')
        id_column_index = 0
        if len(column_indice) == 0:
            value_map = {}
            for r in range(INDEX_DATA_ROW, sheet.nrows):
                value = unicode(sheet.cell(r, id_column_index).value).strip()
                if not value:
                    continue
                if value_map.get(value):
                    print os_encode('SHEET[%s]%s列<%s>重复，无法选择ID字段'%(sheet.name.encode('utf-8'), self.column_to_alphabet(id_column_index), value.encode('utf-8')))
                    return -1
                value_map[value] = sheet.row(r)
        else:
            id_column_index = column_indice[0]
        return id_column_index

    def collect_from_project(self, lang_field_index):
        for xls_file in os.listdir(self.xls_root_path):
            if PATTERN_XLS_NAME.match(xls_file) == None:
                continue
            xls_name = PATTERN_XLS_SUFFIX.sub('', xls_file)
            book = self.get_book(xls_name)
            for sheet_name in book.sheet_names():
                sheet = book.sheet_by_name(sheet_name)
                if not sheet_name.isupper():
                    continue
                table_name = sheet_name
                table = self.scan.get_table(table_name)

                sheet_scan_map = {}
                id_column_index = self.__get_sheet_id_column_index(sheet)

                id_field_name = None
                if id_column_index >= 0:
                    id_field_name = unicode(sheet.cell(INDEX_NAME_ROW, id_column_index).value)
                def hexdigest(text):
                    return md5.md5(text.encode('utf-8')).hexdigest()
                    
                maps_column_indice = self.get_column_indice(sheet, INDEX_NAME_ROW, 'maps')
                for c in range(sheet.ncols):
                    field_rule = unicode(sheet.cell(INDEX_RULE_ROW, c).value).strip()
                    if field_rule in (u'', u'*', u'\uff0a'):
                        continue
                    field_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value).strip()
                    if PATTERN_FIELD_NAME.match(field_name) == None:
                        continue
                    if not sheet_scan_map.has_key(field_name):
                        sheet_scan_map[field_name] = {}
                    col_text_list = sheet_scan_map[field_name][c] = []
                    for r in range(INDEX_DATA_ROW, sheet.nrows):
                        cell = sheet.cell(r, c)
                        if cell.ctype == xlrd.XL_CELL_EMPTY:
                            continue
                        text = unicode(cell.value).strip()
                        if PATTERN_CHINESE.search(text) == None:
                            continue
                        if len(maps_column_indice) == 1:
                            maps_cell = sheet.cell(r, maps_column_indice[0])
                            if maps_cell.ctype == xlrd.XL_CELL_EMPTY or unicode(maps_cell.value).strip() == '':
                                # print os_encode('SHEET[%s]%s:%s %s:%s'%(sheet_name.encode('utf-8'), \
                                #     id_field_name.encode('utf-8'), id_value.encode('utf-8'), \
                                #     field_name.encode('utf-8'), unicode(cell.value).encode('utf-8')))
                                continue
                        if id_field_name:
                            id_cell = sheet.cell(r, id_column_index)
                            if id_cell.ctype == xlrd.XL_CELL_EMPTY:
                                continue
                            id_value = re.sub(r'\.0$', '', unicode(id_cell.value))
                            # [SHEET_NAME]-[ID_FIELD_NAME]-[ID_VALUE]-[FIELD_NAME]
                            label = u'%s-%s-%s-%s'%(sheet_name, id_field_name, id_value, field_name)
                        else:
                            label = u'%s-%s-%s'%(sheet_name, field_name, hexdigest(text))
                        col_text_list.append((label, text, field_name))
                for field_name, column_list in sheet_scan_map.items():
                    if len(column_list) == 1:
                        _, text_list = column_list.items()[0]
                        for label, text, _ in text_list:
                            table.write(label, FieldIndex.CHINESE, text)
                    else:
                        is_modified = False
                        for _, text_list in column_list.items():
                            for _, text, field_name in text_list:
                                label = u'%s-%s-%s'%(sheet_name, field_name, hexdigest(text))
                                table.write(label, FieldIndex.CHINESE, text)
                                is_modified = True
                        if is_modified:
                            print os_encode('SHEET[%s]HASH_KEY(%s)'%(sheet_name.encode('utf-8'), field_name.encode('utf-8')))
            book.release_resources()
        table = self.scan.get_table(prefab.PREFAB_SHEET_NAME)
        text_map = prefab.dump_text_map()
        for label in text_map.iterkeys():
            table.write(label, FieldIndex.CHINESE, text_map[label])
        table = self.scan.get_table(csharp.SCRIPT_SHEET_NAME)
        text_map = csharp.dump_text_map()
        for label in text_map.iterkeys():
            table.write(label, FieldIndex.CHINESE, text_map[label])
        self.__compare_and_collect(field_index = lang_field_index)

    def __compare_and_collect(self, field_index):
        self.scan.remove_empty_tables()
        exclude = TranslationDatabase(type = DatabaseType.EXCLUDE)
        collect = TranslationDatabase(type = DatabaseType.COLLECT)
        collect.clear()
        for raw_table in self.scan.tables:
            lib_table = self.library.get_table(raw_table.name)
            lib_table.compare_for_translation(raw_table, collect, field_index)
        # 剔除冗余资源
        for exclude_table in exclude.tables:
            if not collect.has_table(exclude_table.name):
                continue
            collect_table = collect.get_table(exclude_table.name)
            for label in exclude_table.data.keys():
                if collect_table.data.has_key(label):
                    del collect_table.data[label]
        collect.save()

        report = TranslationCompareReport()
        report.generate(self.library, self.scan, collect, field_index)
        report.save(re.sub(r'(\.xls)$', r'_detail\g<1>', collect.path))

        import os.path as p
        report = TranslationCompareReport()
        report.generate(self.library, self.scan, self.scan, field_index)
        report.save(re.sub(r'(\.xls)$', r'_all\g<1>', collect.path))
        print os_encode('%25s %6s'%('TOTAL', '{:,}'.format(collect.character_count)))

def main():
    collector = TranslationCollector()
    collector.collect_from_project(lang_field_index = FieldIndex.JAPANESE)

if __name__ == '__main__':
    main()
