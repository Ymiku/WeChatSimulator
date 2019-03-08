#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class StatChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[stat.xls]所有校验
        """
        super(StatChecker,self).check_all()