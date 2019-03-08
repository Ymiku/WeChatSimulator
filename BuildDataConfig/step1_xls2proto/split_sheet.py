#!/usr/bin/env python
#encoding: utf-8

import os, sys, re
import xlrd, xlwt

WORK_PATH = os.path.dirname(os.path.abspath(__file__))
ROOT_PATH = os.path.abspath(os.path.join(WORK_PATH, '..'))
sys.path.append(os.path.join(ROOT_PATH, 'checker'))

import base_check
from xlsdata.utils import os_encode, is_winsystem

class SheetSpliter(base_check.BaseChecker):
    def __init__(self, xls_name):
        super(SheetSpliter, self).__init__(xls_name, os.path.join(ROOT_PATH, 'DataConfig'))
        self.init()
    
    def split(self, sheet_name, field_name):
        segment_list = []
        src_sheet = self.book.sheet_by_name(sheet_name)
        column_indice = self.get_column_indice(src_sheet, base_check.INDEX_NAME_ROW, field_name)
        if len(column_indice) > 1:
            for i in range(len(column_indice)):
                column_indice[i] = self.column_to_alphabet(column_indice[i]) + '列'
            msg = '表单(%s#%s)包含多个(%s)字段%s，无法分表'%(self.name, sheet_name.encode(encoding='utf-8'), field_name.encode(encoding='utf-8'), column_indice);
            raise Exception(os_encode(msg))
            return
        if len(column_indice) == 0:
            msg = '表单(%s#%s)不包含(%s)字段，无法分表'%(self.name, sheet_name.encode(encoding='utf-8'), field_name.encode(encoding='utf-8'))
            raise Exception(os_encode(msg))
            return
        group_map = {}
        column_index = column_indice[0]
        for r in range(base_check.INDEX_DATA_ROW, src_sheet.nrows):
            cell = src_sheet.cell(r, column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            key = unicode(cell.value)
            if not group_map.has_key(key):
                group_map[key] = []
            group_map[key].append(r)
        for key in group_map.iterkeys():
            dst_book = xlwt.Workbook(encoding='utf-8')
            dst_sheet = dst_book.add_sheet(sheet_name, cell_overwrite_ok=False)
            # 写入配置表表头
            row_index = 0
            for r in range(base_check.INDEX_DATA_ROW):
                for c in range(src_sheet.ncols):
                    src_cell = src_sheet.cell(r, c)
                    dst_sheet.write(row_index, c, src_cell.value)
                row_index += 1
            # 写入配置表分表数据
            for r in group_map[key]:
                for c in range(src_sheet.ncols):
                    src_cell = src_sheet.cell(r, c)
                    dst_sheet.write(row_index, c, src_cell.value)
                row_index += 1
            key = re.sub(r'\.0$', '', key)
            extension = re.compile(r'(\.xls[x]?)$', re.IGNORECASE)
            segment_path = extension.sub(r'_%s\1'%(key.encode(encoding='utf-8')), self.path)
            segment_path = re.sub(r'x$', '', segment_path)
            segment_list.append(segment_path)
            print 'SPLIT => %s'%(segment_path)
            dst_book.save(segment_path)
        return segment_list

if __name__ == '__main__':
    book_name = sys.argv[1]
    spliter = SheetSpliter(book_name)
    sheet_name = sys.argv[2].decode('utf-8')
    field_name = sys.argv[3].decode('utf-8')
    spliter.split(sheet_name, field_name)