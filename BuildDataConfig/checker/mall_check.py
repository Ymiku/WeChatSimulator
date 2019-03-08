#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
from xlsdata.utils import supported_date_examples
import xlrd

class MallChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[mall.xls]所有校验
        """
        super(MallChecker,self).check_all()
        self.check_date_time()

    def check_date_time(self):
        """
        配置表日期检测
        """
        self.log(1, 'MALL_CONF|check_date_time|商城日期(time_start/time_end)规范校验')
        errors = []
        sheet = self.book.sheet_by_name('MALL_CONF')
        column_indice = []
        column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, 'time_start')
        column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, 'time_end')
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            date_list = []
            for c in column_indice:
                cell = sheet.cell(r, c)
                field_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value).encode(encoding='utf-8')
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    msg = '%s#%s|%d行%s列|check_date_time|日期字段(%s)未配置，请输入%s格式的日期'%(self.name, sheet.name.encode(encoding='utf-8'), \
                                    r+1, self.column_to_alphabet(c), field_name, supported_date_examples())
                    errors.append(msg)
                    self.log(2, msg)
                    continue
                date_string = unicode(cell.value)
                date = None
                try:
                    date = self.parse_date(date_string)
                except BaseException, e:
                    msg = '%s#%s|%d行%s列|check_date_time|日期字段(%s=%s)不规范|%s'%(self.name, sheet.name.encode(encoding='utf-8'), \
                                    r+1, self.column_to_alphabet(c), field_name, date_string.encode(encoding='utf-8'), e)
                    errors.append(msg)
                    self.log(2, msg)
                date_list.append(date)
            if len(date_list) != 2:
                continue
            date_format = '%Y-%m-%dT%H:%M:%S'
            if date_list[0] != None and date_list[1] != None:
                sfield_name = unicode(sheet.cell(INDEX_NAME_ROW, column_indice[0]).value).encode(encoding='utf-8')
                efield_name = unicode(sheet.cell(INDEX_NAME_ROW, column_indice[1]).value).encode(encoding='utf-8')
                if time.mktime(date_list[1]) <= time.mktime(date_list[0]):
                    msg = '%s#%s|%d行%s列|check_date_time|开始时间(%s=%s)>=结束时间(%s=%s)'%(self.name, sheet.name.encode(encoding='utf-8'), \
                                    r+1, self.column_to_alphabet(c), \
                                    sfield_name, time.strftime(date_format, date_list[0]), \
                                    efield_name, time.strftime(date_format, date_list[1]))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)
