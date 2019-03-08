#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class AbilityChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[ability.xls]所有校验
        """
        super(AbilityChecker,self).check_all()
        self.check_buff_id()
        self.check_projectile_id()
        self.check_ability_asset() 
        self.check_ability_status()

    def check_buff_id(self):
        """
        校验技能buff配置有效性
        """
        self.log(1,"ABILITY|check_buff_id|buff(buff_id)有效校验")
        self.check_ref_id_exists("buff", "BUFF", "ABILITY", ["buff_id"])

    def check_projectile_id(self):
        """
        校验技能抛射物配置有效性
        """
        self.log(1,"ABILITY|check_projectile_id|抛射物(projectile_id)有效校验")
        self.check_ref_id_exists("projectile", "PROJECTILE_CONF", "ABILITY", ["projectile_id"])

    def check_ability_asset(self):
        """
        校验技能prefab文件有效性
        """
        self.log(1,"ABILITY|check_ability_asset|技能资源文件(prefab_path)(Resources/ViewEditorPlaymaker)有效校验")
        root_path = os.path.abspath(os.path.join(self.asset_root_path, "ViewEditorPlaymaker"))
        errors = self.check_asset_exist("ABILITY", ["prefab_path"], "prefab", custom_root_path = root_path)
        self.__assert__(errors)

    def check_ability_status(self):
        """
        校验技能相关状态有效性
        """
        self.log(1,"ABILITY|check_ability_status|技能相关状态有效校验")
        self.check_status("ABILITY", ["break_precast_status","break_casting_status","holdback_cast_ability"])
