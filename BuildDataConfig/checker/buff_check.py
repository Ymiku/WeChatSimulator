#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class BuffChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[buff.xls]所有校验
        """
        super(BuffChecker,self).check_all()
        self.check_buff_asset()
        self.check_buff_icon()

    def check_buff_asset(self):
        """
        校验资源文件有效性
        """
        self.log(1,"BUFF|check_buff_asset|技能资源文件(prefab_path)(Resources/ViewEditorPlaymaker)有效校验")
        root_path = os.path.abspath(os.path.join(self.asset_root_path, "ViewEditorPlaymaker"))
        errors = self.check_asset_exist("BUFF", ["prefab_path"], "prefab", custom_root_path = root_path)
        self.__assert__(errors)

    def check_buff_icon(self):
        """
        校验BUFF图标存在有效性
        """
        self.log(1,"BUFF|check_buff_icon|BUFF图标(icon)有效校验")
        errors = self.check_image_exists("BUFF", "icon", "UI/Icon/Buff", True)
        self.__assert__(errors)
