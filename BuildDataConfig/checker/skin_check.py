#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd, time

class SkinChecker(base_check.BaseChecker):
    # def __init__(self, xls_path):
    #     """
    #     xls_path: 待检配置表文件路径
    #     """
    #     super(SkinChecker, self).__init__(xls_path)

    def check_all(self):
        """
        配置表[skin.xls]所有校验
        """
        super(SkinChecker,self).check_all()
        self.check_maps()
        self.check_attack_id()
        self.check_weapon_type()
        self.check_skin_prefab()
        self.check_enabled_skin()
        self.check_skin_icon()
        # self.check_bits_index()

    def check_attack_id(self):
        """
        attack_id为attack表的引用id，skin表中出现的attack_id应该都在attack表中，否则为错误填写会引起报错
        """
        self.log(1, "SKIN_CONF|check_attack_id|attack_id有效校验")
        self.check_ref_id_exists("attack", "ATTACK_CONF", "SKIN_CONF", ["attack_id"])

    def check_weapon_type(self):
        """
        weapon_material_type/armor_material_type引用attack_sound表中Enum页签中的武器值数字，不可填写武器值不存在的数字
        """
        errors = []
        weapon_map = {}
        weapon_book = self.get_book("attack_sound")
        weapon_sheet = weapon_book.sheet_by_name("Enum")
        weapon_columns = self.get_column_indice(weapon_sheet, 0, "武器值".decode(encoding="utf-8"))
        weapon_id_index = weapon_columns[0]
        for r in range(1, weapon_sheet.nrows):
            row_value = weapon_sheet.row(r)
            weapon_id = row_value[weapon_id_index].value
            weapon_map[weapon_id] = row_value[weapon_id_index - 1].value
        self.log(1,"SKIN_CONF|check_weapon_type|武器值(weapon_material_type)有效校验")
        # weapon_material_type
        skin_sheet = self.book.sheet_by_name("SKIN_CONF")
        skin_columns = self.get_column_indice(skin_sheet, INDEX_NAME_ROW, "weapon_material_type")
        for r in range(INDEX_DATA_ROW, skin_sheet.nrows):
            for c in skin_columns:
                cell = skin_sheet.cell(r, c)
                cell_value = cell.value
                if cell.ctype != xlrd.XL_CELL_EMPTY and weapon_map.has_key(cell_value) == False:
                    msg = "ERROR|skin#SKIN_CONF|%d行%d:%s列(weapon_material_type=%s)attack_sound#Enum表单中不存在"%(r+1,c+1, self.column_to_alphabet(c), int(cell_value))
                    errors.append(msg)
                    self.log(2, msg)
        self.log(1,"SKIN_CONF|check_weapon_type|武器值(armor_material_type)有效校验")
        # armor_material_type
        skin_columns = self.get_column_indice(skin_sheet, INDEX_NAME_ROW, "armor_material_type")
        for r in range(INDEX_DATA_ROW, skin_sheet.nrows):
            for c in skin_columns:
                cell = skin_sheet.cell(r, c)
                cell_value = cell.value
                if cell.ctype != xlrd.XL_CELL_EMPTY and weapon_map.has_key(cell_value) == False:
                    msg = "ERROR|skin#SKIN_CONF|%d行%d:%s列(armor_material_type=%s)attack_sound#Enum表单中不存在"%(r+1,c+1, self.column_to_alphabet(c), int(cell_value))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)

    def check_maps(self):
        """
        校验英雄皮肤上架地图配置有效性
        """
        self.log(1,"SKIN_CONF|check_maps|英雄皮肤上架地图(maps)有效校验")
        self.check_ref_id_exists("arena_mode", "ARENA_MODE_CONF", "SKIN_CONF", ["maps"])

    def check_skin_prefab(self):
        """
        校验英雄皮肤prefab文件有效性
        """
        self.log(1,"SKIN_CONF|check_skin_prefab|皮肤prefab资源文件有效校验")
        self.check_asset_exist("SKIN_CONF", ["assetURL", "allyAssetURL", "enemyAssetURL", "allyAssetURL_N", "enemyAssetURL_N"], "prefab")

    def check_skin_icon(self):
        """
        校验英雄皮肤图标文件有效性
        """
        self.log(1,"SKIN_CONF|check_skin_icon|皮肤图标文件(IconURL)有效校验")
        self.check_asset_exist("SKIN_CONF", ["IconURL"], "png")

    def check_enabled_skin(self):
        """
        校验上架英雄皮肤配置上架有效性
        """
        modes = self.get_enabled_modes()
        for arena_mode in modes:
            self.check_enabled_skin_in_mode(arena_mode)

    def check_enabled_skin_in_mode(self, arena_mode):
        """
        校验上架英雄皮肤配置上架有效性
        """
        self.log(1,"SKIN_CONF|check_enabled_skin|地图中(arena_mode=%d)中上架英雄皮肤(defaultSkinID/testSkinID)有效校验"%(int(arena_mode)))
        
        errors = []
        skin_map = self.get_enabled_object_map(arena_mode, "skin", "SKIN_CONF")

        hero_book = self.get_book("arena_object")
        sheet = hero_book.sheet_by_name("ARENA_OBJECT_CONF")
        field_skin_column_indice = []
        field_skin_column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, "defaultSkinID")
        field_skin_column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, "testSkinID")
        field_type_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "arena_type")[0]
        field_name_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "name")[0]
        field_mode_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "maps")[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            cell = sheet.cell(r, field_type_column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY or \
                unicode(cell.value).lower() != "hero".decode(encoding="utf-8"):
                continue
            mode_cell = sheet.cell(r, field_mode_column_index)
            mode_list = self.parse_int_array(unicode(mode_cell.value))
            if mode_cell.ctype == xlrd.XL_CELL_EMPTY or len(mode_list) == 0: # 未上架英雄不检测
                continue
            if not arena_mode in mode_list: # 上架不匹配跳过
                continue
            hero_name = unicode(sheet.cell(r, field_name_column_index).value).encode(encoding="utf-8")
            enabled_num = 0
            for c in field_skin_column_indice:
                skin_cell = sheet.cell(r, c)
                skin_list = self.parse_int_array(unicode(skin_cell.value))
                for skin_id in skin_list:
                    if skin_map.has_key(skin_id):
                        enabled_num += 1
            if enabled_num == 0:
                msg = "ARENA_OBJECT_CONF|%d行|在地图(arena_mode=%d)中英雄(%s)没有上架的皮肤可用"%(r+1,int(arena_mode), hero_name)
                errors.append(msg)
                self.log(2, msg)
        self.__warn__(errors)

    def check_skin_icon(self):
        """
        校验皮肤图标文件有效性
        """
        self.log(1,"SKIN_CONF|check_skin_icon|英雄图标文件(IconURL/smallIconURL)有效校验")
        errors = self.check_asset_exist("SKIN_CONF", ["IconURL", "smallIconURL"], "png")
        self.__assert__(errors)

    def check_bits_index(self):
        """
        校验皮肤比特位(后台使用)配置有效性
        """
        self.log(1,"SKIN_CONF|check_bits_index|皮肤比特位配置(bits_index)有效校验")
        errors = []

        database = self.storage.get("skin_bits")

        bits_dict = {}
        sheet = self.book.sheet_by_name("SKIN_CONF")
        bits_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'bits_index')[0]
        mode_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'maps')[0]
        name_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'title')[0]
        id_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'id')[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            mode_cell = sheet.cell(r, mode_column_index)
            mode_list = self.parse_int_array(unicode(mode_cell.value))
            if len(mode_list) == 0:
                continue
            skin_id = self.parse_int(sheet.cell_value(r, id_column_index))
            if int(skin_id / 100) >= 1000:
                continue
            bits_cell = sheet.cell(r, bits_column_index)
            if bits_cell.ctype == xlrd.XL_CELL_EMPTY:
                msg = "%s#SKIN_CONF|%d行%s列|bits_index未配置"%(self.name,r+1, self.column_to_alphabet(bits_column_index))
                errors.append(msg)
                self.log(msg)
            bits = int(float(bits_cell.value))
            qualified = True
            if bits_dict.has_key(bits):
                qualified = False
                msg = "%s#SKIN_CONF|[%d;%d]行%s列|bits_index(%d)重复"%(self.name,bits_dict[bits]+1,r+1, self.column_to_alphabet(bits_column_index), bits)
                errors.append(msg)
                self.log(2, msg)
            if bits < 0 or bits > 8191:
                qualified = False
                msg = "%s#SKIN_CONF|%d行%s列|bits_index(%d)超出范围(0~8191)"%(self.name, r+1, self.column_to_alphabet(bits_column_index), bits)
                errors.append(msg)
                self.log(2, msg)
            skin_name = unicode(sheet.cell(r, name_column_index).value).encode(encoding='utf-8')
            if qualified == True:
                if database.has(skin_id):
                    item = database.get(skin_id)
                    if item.bits != bits:
                        msg = "%s#SKIN_CONF|%d行%s列|皮肤(%d=%s)字段bits_index(%d=>%d))"%(\
                            self.name, r+1, self.column_to_alphabet(bits_column_index), \
                            skin_id, skin_name, item.bits, bits)
                        errors.append(msg)
                        self.log(2, msg)
                else:
                    item = database.new()
                    item.id = skin_id
                    item.name = skin_name.decode(encoding='utf-8')
                    item.bits = bits
                    item.timestamp = time.time()
                    database.set(item)
            bits_dict[bits] = r
        database.commit()
        self.__assert__(errors)
