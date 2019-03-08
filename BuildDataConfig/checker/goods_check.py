#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class GoodsChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[goods.xls]所有校验
        """
        super(GoodsChecker,self).check_all()
