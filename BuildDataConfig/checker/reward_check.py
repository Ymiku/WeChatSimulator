#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class RewardChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[reward.xls]所有校验
        """
        super(RewardChecker,self).check_all()
