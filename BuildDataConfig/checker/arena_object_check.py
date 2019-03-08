#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd, time

class ArenaObjectChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[arena_object.xls]所有校验
        """
        super(ArenaObjectChecker,self).check_all()
        self.check_attack_id()
        self.check_combo_ability()
        self.check_maps()
        self.check_passive_ability()
        self.check_skin()
        self.check_talent_ability()
        self.check_equipment()
        self.check_loading_portrait()
        self.check_attack_id_not_used()
        self.check_ability_sample_database()
        # self.check_bits_index()

    def check_maps(self):
        """
        校验英雄上架地图配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_maps|英雄上架地图(maps)有效校验")
        self.check_ref_id_exists("arena_mode", "ARENA_MODE_CONF", "ARENA_OBJECT_CONF", ["maps"])

    def check_skin(self):
        """
        校验英雄皮肤配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_skin|英雄皮肤(defaultSkinID/testSkinID)有效校验")
        self.check_ref_id_exists("skin", "SKIN_CONF", "ARENA_OBJECT_CONF", ["defaultSkinID", "testSkinID"])

    def check_passive_ability(self):
        """
        校验英雄主动技能配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_passive_ability|英雄主动技能(passive_ability)有效校验")
        self.check_ref_id_exists("combo_ability", "COMBO_ABILITY", "ARENA_OBJECT_CONF", ["passive_ability"])
    
    def check_talent_ability(self):
        """
        校验英雄天赋技能配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_talent_ability|英雄主动技能(talent_ability)有效校验")
        self.check_ref_id_exists("combo_ability", "COMBO_ABILITY", "ARENA_OBJECT_CONF", ["talent_ability"])

    def check_combo_ability(self):
        """
        校验英雄键位技能配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_combo_ability|英雄键位技能(combo_ability)有效校验")
        self.check_ref_id_exists("combo_ability", "COMBO_ABILITY", "ARENA_OBJECT_CONF", ["combo_ability"])
    
    def check_attack_id(self):
        """
        校验英雄攻击ID配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_attack_id|英雄攻击ID配置(attack_id、strengthen_attack_id)有效校验")
        self.check_ref_id_exists("attack", "ATTACK_CONF", "ARENA_OBJECT_CONF", ["attack_id", "strengthen_attack_id"])

    def check_equipment(self):
        """
        校验英雄自带装备配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_equipment|英雄自带装备配置(item)有效校验")
        self.check_ref_id_exists("item", "ITEM_CONF", "ARENA_OBJECT_CONF", ["item"])

    def check_loading_portrait(self):
        """
        校验英雄立绘文件有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_loading_portrait|英雄立绘文件(LoadingPortrait)有效校验")
        modes = self.get_enabled_modes()
        for arena_mode in modes:
            self.check_map_hero_portrait(arena_mode)

    def check_map_hero_portrait(self, arena_mode):
        """
        校验地图中上架英雄立绘文件有效性
        """
        errors = []
        self.log(1,"ARENA_OBJECT_CONF|check_map_hero_portrait|在地图(arena_mode=%d)中上架英雄皮肤图标、立绘文件有效校验"%(int(arena_mode)))

        arena_mode = unicode(arena_mode)
        skin_map = self.get_enabled_object_map(arena_mode, "skin", "SKIN_CONF")

        sheet = self.book.sheet_by_name("ARENA_OBJECT_CONF")
        column_indice = []
        column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, "defaultSkinID")
        column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, "testSkinID")

        maps_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "maps")[0]
        type_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "arena_type")[0]
        name_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "name")[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            hero_name = unicode(sheet.cell(r, name_column_index).value).encode(encoding="utf-8")
            maps_cell = sheet.cell(r, maps_column_index)
            if maps_cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            maps_list = self.parse_int_array(unicode(maps_cell.value))
            if (arena_mode in maps_list) == False: # 跳过对未上架英雄检测
                continue
            if unicode(sheet.cell(r, type_column_index).value).encode(encoding="utf-8").lower() != "hero": # 跳过对非英雄检测
                continue
            for c in column_indice:
                filed_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value).encode(encoding="utf-8")
                skin_cell = sheet.cell(r, c)
                skin_list = self.parse_int_array(unicode(skin_cell.value))
                for id in skin_list:
                    if skin_map.has_key(id) == False: # 跳过对未上架皮肤检测
                        continue
                    portrait = "Icons/LoadingPortrait/%d.png"%(int(id))
                    if os.path.exists("%s/%s"%(self.asset_root_path, portrait)) == False:
                        msg = "ARENA_OBJECT_CONF|%d行%s列|英雄(name=%s)皮肤(%s=%s)立绘文件不在"%(r+1, self.column_to_alphabet(c), hero_name, filed_name, portrait)
                        errors.append(msg)
                        self.log(2, msg)
                    icon = "Icons/ArenaObjectIcon/%d.png"%(int(id))
                    if os.path.exists("%s/%s"%(self.asset_root_path, icon)) == False:
                        msg = "ARENA_OBJECT_CONF|%d行%s列|英雄(name=%s)皮肤(%s=%s)图标文件不在"%(r+1, self.column_to_alphabet(c), hero_name, filed_name, icon)
                        errors.append(msg)
                        self.log(2, msg)
        self.__warn__(errors)

    def check_bits_index(self):
        """
        校验英雄比特位(后台使用)配置有效性
        """
        self.log(1,"ARENA_OBJECT_CONF|check_bits_index|英雄比特位配置(bits_index)有效校验")
        errors = []

        database = self.storage.get("champion_bits")

        bits_dict = {}
        sheet = self.book.sheet_by_name("ARENA_OBJECT_CONF")
        bits_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'bits_index')[0]
        type_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'arena_type')[0]
        name_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'name')[0]
        id_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'id')[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            type_cell = sheet.cell(r, type_column_index)
            if unicode(type_cell.value).lower() != "hero":
                continue
            bits_cell = sheet.cell(r, bits_column_index)
            if bits_cell.ctype == xlrd.XL_CELL_EMPTY:
                msg = "%s#ARENA_OBJECT_CONF|%d行%s列|bits_index未配置"%(self.name,r+1, self.column_to_alphabet(bits_column_index))
                errors.append(msg)
                self.log(msg)
            bits = int(float(bits_cell.value))
            qualified = True
            if bits_dict.has_key(bits):
                qualified = False
                msg = "%s#ARENA_OBJECT_CONF|[%d;%d]行%s列|bits_index(%d)重复"%(self.name,bits_dict[bits]+1,r+1, self.column_to_alphabet(bits_column_index), bits)
                errors.append(msg)
                self.log(2, msg)
            if bits < 0 or bits > 1023:
                qualified = False
                msg = "%s#ARENA_OBJECT_CONF|%d行%s列|bits_index(%d)超出范围(0~1023)"%(self.name, r+1, self.column_to_alphabet(bits_column_index), bits)
                errors.append(msg)
                self.log(2, msg)
            champion_id = self.parse_int(sheet.cell_value(r, id_column_index))
            champion_name = unicode(sheet.cell(r, name_column_index).value).encode(encoding='utf-8')
            if qualified == True:
                if database.has(champion_id):
                    item = database.get(champion_id)
                    if item.bits != bits:
                        msg = "%s#ARENA_OBJECT_CONF|%d行%s列|英雄(%d=%s)字段bits_index(%d=>%d))"%(\
                            self.name, r+1, self.column_to_alphabet(bits_column_index), \
                            champion_id, champion_name, item.bits, bits)
                        errors.append(msg)
                        self.log(2, msg)
                else:
                    item = database.new()
                    item.id = champion_id
                    item.name = champion_name.decode(encoding='utf-8')
                    item.bits = bits
                    item.timestamp = time.time()
                    database.set(item)
            bits_dict[bits] = r
        database.commit()
        self.__assert__(errors)
    
    def check_attack_id_not_used(self):
        """
        校验英雄攻击ID配置漏填
        """
        self.log(1, "ARENA_OBJECT_CONF|check_attack_id_not_used|英雄攻击ID配置(attack_id、strengthen_attack_id)配置漏填校验")
        attack_book = self.get_book('attack')
        attack_sheet = attack_book.sheet_by_name('ATTACK_CONF')
        attack_map = {}
        id_column_index = self.get_column_indice(attack_sheet, INDEX_NAME_ROW, u'id')[0]
        for r in range(INDEX_DATA_ROW, attack_sheet.nrows):
            cell = attack_sheet.cell(r, id_column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            id = self.parse_int(cell.value)
            champion_id = unicode(id / 1000)
            if not attack_map.has_key(champion_id):
                attack_map[champion_id] = []
            attack_map[champion_id].append(unicode(id))
        errors = []
        sheet = self.book.sheet_by_name('ARENA_OBJECT_CONF')
        id_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, u'id')[0]
        name_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, u'name')[0]
        mode_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, u'maps')[0]
        attack_column_indice = []
        attack_column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, u'attack_id')
        attack_column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, u'strengthen_attack_id')
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            cell = sheet.cell(r, id_column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            id = self.parse_int_string(cell.value)
            if not attack_map.has_key(id):
                continue
            cell = sheet.cell(r, mode_column_index)
            if len(self.parse_int_array(unicode(cell.value))) == 0: # 只检测上架英雄
                continue
            xls_attack_list = attack_map[id]
            xls_attack_list.sort()
            if len(xls_attack_list) == 0:
                continue
            attack_list = []
            for c in attack_column_indice:
                attack_cell = sheet.cell(r, c)
                if attack_cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                attack_list += self.parse_int_array(unicode(attack_cell.value))
            absent_list = []
            for xls_attack_id in xls_attack_list:
                if not xls_attack_id in attack_list:
                    absent_list.append(int(xls_attack_id))
            if len(absent_list) != 0:
                champion_name = unicode(sheet.cell(r, name_column_index).value).encode('utf-8')
                msg = '%s#ARENA_OBJECT_CONF|%d行英雄(%d:%s)在attack#ATTACK_CONF表中对应普攻配置%r漏填'%(self.name, r+1, int(id), champion_name, absent_list)
                errors.append(msg)
                self.log(2, msg)
        self.__assert__(errors)

    def check_ability_sample_database(self):
        """
        校验英雄攻击ID配置漏填
        """
        self.log(1, "ARENA_OBJECT_CONF|check_ability_sample_database|英雄训练数据(ability_sampledatabase)配置校验")
        errors = self.check_asset_exist('ARENA_OBJECT_CONF', ['ability_sampledatabase'], 'txt')
        self.__assert__(errors)