#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class ArenaModeChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[arena_mode.xls]所有校验
        """
        super(ArenaModeChecker,self).check_all()
        self.check_damage_factor()

    def check_damage_factor(self):
        """
        DamageFactor该字段必须不等于0，否则整个伤害计算流程会出现BUG
        """
        self.log(1, "ARENA_MODE_CONF|check_damage_factor|DamageFactor有效检验")
        errors = []
        sheet = self.book.sheet_by_name("ARENA_MODE_CONF")
        column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "DamageFactor")[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            cell = sheet.cell(r, column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                msg = "ARENA_MODE_CONF|%d行%s列DamageFactor字段为空"%(r+1, self.column_to_alphabet(column_index))
                errors.append(msg)
                self.log(2, msg)
                continue
            if cell.ctype == xlrd.XL_CELL_NUMBER:
                if cell.value == 0:
                    msg = "ARENA_MODE_CONF|%d行%s列DamageFactor字段为0"%(r+1, self.column_to_alphabet(column_index))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)
