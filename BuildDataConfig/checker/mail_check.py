#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class MailChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[mailinfo.xls]所有校验
        """
        super(MailChecker,self).check_all()
