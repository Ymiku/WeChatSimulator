#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class BagChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[bag_item.xls]所有校验
        """
        super(BagChecker,self).check_all()
