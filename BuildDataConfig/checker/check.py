#!/usr/bin/env python
#encoding: utf-8

from xlsdata.utils import os_encode
import xlrd
import sys, os, re
import ability_check
import arena_object_check
import arena_mode_check
import attack_check
import buff_check
import combo_ability_check
import item_check
import projectile_check
import skin_check
import stat_check
import goods_check
import uiconfig_check
import base_check
import bag_check
import reward_check
import mall_check
import mail_check

class ExcelChecker(object):
    
    def __init__(self, xls_root_path):
        self.xls_root_path = xls_root_path
        self.checker_map = {}
        self.pattern = re.compile(r'^[a-z0-9_]+\.xls[x]?$', re.IGNORECASE)

        self.register_checker("ability", ability_check.AbilityChecker)
        self.register_checker("arena_object", arena_object_check.ArenaObjectChecker)
        self.register_checker("arena_mode", arena_mode_check.ArenaModeChecker)
        self.register_checker("attack", attack_check.AttackChecker)
        self.register_checker("buff", buff_check.BuffChecker)
        self.register_checker("combo_ability", combo_ability_check.ComboAbilityChecker)
        self.register_checker("item", item_check.ItemChecker)
        self.register_checker("projectile", projectile_check.ProjectileChecker)
        self.register_checker("skin", skin_check.SkinChecker)
        self.register_checker("stat", stat_check.StatChecker)
        self.register_checker("UI_config", uiconfig_check.UIConfigChecker)
        self.register_checker("goods", goods_check.GoodsChecker)
        self.register_checker("mall", mall_check.MallChecker)
        self.register_checker("bag_item", bag_check.BagChecker)
        self.register_checker("reward", reward_check.RewardChecker)
        self.register_checker("mail", mail_check.MailChecker)
        extension = re.compile(r'\.xls[x]?$', re.IGNORECASE)
        for book_file in os.listdir(self.xls_root_path):
            if self.pattern.match(book_file) == None:
                continue
            book_name = extension.sub('', book_file)
            if not self.has_checker(book_name):
                self.register_checker(book_name, base_check.BaseChecker)
    
    def has_checker(self, name):
        return self.checker_map.has_key(name)
    
    def register_checker(self, name, CheckerClass):
        checker = CheckerClass(name, self.xls_root_path)
        self.checker_map[checker.name] = checker

    def check_duplicate_definitions(self):
        map = {}
        for xls_name in os.listdir(self.xls_root_path):
            if self.pattern.match(xls_name) == None:
                continue
            book = xlrd.open_workbook(self.xls_root_path + '/' + xls_name)
            for sheet_name in book.sheet_names():
                if not unicode(sheet_name).isupper():
                    continue
                if not map.has_key(sheet_name):
                    map[sheet_name] = xls_name
                else:
                    msg = "配置表[%s]表单名[%s]与配置表[%s]子表单重复"%(xls_name.encode('utf-8') ,sheet_name.encode('utf-8'), map[sheet_name].encode('utf-8'))
                    base_check.DRY_RUN_ERRORS.append(msg)
                    print os_encode(msg)  
            book.release_resources()
    
    def check_all(self):
        self.check_duplicate_definitions()
        book_names = self.checker_map.keys()
        def sort_compare(x,y): 
            return 1 if x.lower() > y.lower() else -1
        book_names.sort(cmp=sort_compare)
        for name in book_names:
            checker = self.checker_map[name]
            checker.init()
            print os_encode("[%s][START]配置规范校验"%(name))
            checker.check_all()
            checker.dispose()
            print os_encode("[%s][ END ]"%(name))

    def check(self, name):
        if self.checker_map.has_key(name):
            print os_encode("[%s][START]配置规范校验"%(name))
            checker = self.checker_map[name]
            checker.init()
            checker.check_all()
            checker.dispose()
            print os_encode("[%s][ END ]"%(name))

if __name__ == '__main__':
    script_file = os.path.abspath(__file__)
    xls_root_path = os.path.abspath("%s/%s"%(os.path.dirname(script_file), "../DataConfig"))
    checker = ExcelChecker(xls_root_path)

    if len(sys.argv) == 1:
        checker.check_all()
    else:
        for i in range(1, len(sys.argv)):
            checker.check(sys.argv[i])
