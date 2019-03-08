#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class UIConfigChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[UI_config.xls]所有校验
        """
        super(UIConfigChecker,self).check_all()
        self.check_window_prefab()

    def check_window_prefab(self):
        """
        检查窗口prefab资源存在性
        """
        self.log(1, "UI_CONFIG|check_window_prefab|窗体prefab资源(prefabPath)有效检测")
        errors = self.check_asset_exist("UI_CONFIG", ["prefabPath"], "prefab")
        self.__assert__(errors)
