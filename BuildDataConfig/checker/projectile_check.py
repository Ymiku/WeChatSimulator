#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class ProjectileChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[projectile.xls]所有校验
        """
        super(ProjectileChecker,self).check_all()
        self.check_projectile_asset()

    def check_projectile_asset(self):
        """
        抛射物PLAYMAKER资源有效检测
        """
        self.log(1, "PROJECTILE_CONF|check_projectile_asset|抛射物资源(play_maker_path)(Resources/ViewEditorPlaymaker)有效检测")
        root_path = os.path.abspath(os.path.join(self.asset_root_path, "ViewEditorPlaymaker"))
        errors = self.check_asset_exist("PROJECTILE_CONF", ["play_maker_path"], "prefab", custom_root_path = root_path)
        self.__assert__(errors)
