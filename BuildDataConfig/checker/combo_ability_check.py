#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class ComboAbilityChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[combo_ability.xls]所有校验
        """
        super(ComboAbilityChecker,self).check_all()
        self.check_atomic_ability()
        self.check_combo_ability_icon()

    def check_atomic_ability(self):
        """
        检查组合技能子技能配置有效校验
        """
        self.log(1,"COMBO_ABILITY|check_atomic_ability|子技能(atomic_ability)有效校验")
        self.check_ref_id_exists("ability", "ABILITY", "COMBO_ABILITY", ["atomic_ability"])

    def check_combo_ability_icon(self):
        """
        检查组合技能图标文件有效校验
        """
        self.log(1,"COMBO_ABILITY|check_combo_ability_icon|技能图标(icon_res)有效校验")
        self.check_image_exists("COMBO_ABILITY", "icon_res", "Icons/Ability")
        
