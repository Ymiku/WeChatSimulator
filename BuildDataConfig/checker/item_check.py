#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class ItemChecker(base_check.BaseChecker):
    def check_all(self):
        """
        配置表[item.xls]所有校验
        """
        super(ItemChecker,self).check_all()
        self.check_maps()
        self.check_equipment_ability()
        self.check_item_icon()
        self.check_recommend_equipment()
        self.check_equipment_stat()

    def check_equipment_ability(self):
        """
        校验装备主动技能、被动技能配置有效性
        """
        self.log(1,"ITEM_CONF|check_equipment_ability|装备技能(active_ability/passive_abilities)有效校验")
        self.check_ref_id_exists("ability", "ABILITY", "ITEM_CONF", ["active_ability", "passive_abilities"])

    def check_maps(self):
        """
        校验装备上架地图配置有效性
        """
        self.log(1,"ITEM_CONF|check_maps|装备上架地图(maps)有效校验")
        self.check_ref_id_exists("arena_mode", "ARENA_MODE_CONF", "ITEM_CONF", ["maps"])

    def check_item_icon(self):
        """
        校验装备图标存在有效性
        """
        self.log(1,"ITEM_CONF|check_item_icon|装备图标(icon)有效校验")
        self.check_image_exists("ITEM_CONF", "icon", "UI/Icon/Item", True)

    def check_recommend_equipment(self):
        """
        推荐装备存在检测
        """
        self.log(1,"RECOMMEND_ITEM_CONF|check_recommend_equipment|推荐装备(recommend_item_list*)有效校验")
        self.check_ref_id_exists("item", "ITEM_CONF", "RECOMMEND_ITEM_CONF", ["recommend_item_list1","recommend_item_list2","recommend_item_list3"])
        
        modes = self.get_enabled_modes()
        for arena_mode in modes:
            self.check_recommend_equipment_in_mode(arena_mode)

    def check_recommend_equipment_in_mode(self, arena_mode):
        """
        校验上架英雄推荐装备配置上架有效性
        """
        errors = []
        self.log(1,"RECOMMEND_ITEM_CONF|check_recommend_equipment|在地图(arena_mode=%d)中上架英雄推荐装备(recommend_item_list*)有效校验"%(int(arena_mode)))

        item_map = self.get_enabled_object_map(arena_mode, "item", "ITEM_CONF")

        recommand_item_map = {}
        recommend_sheet = self.book.sheet_by_name("RECOMMEND_ITEM_CONF")
        recommend_field_id_column_index = self.get_column_indice(recommend_sheet, INDEX_NAME_ROW, "id")[0]
        recommend_field_list_column_indice = []
        recommend_field_list_column_indice += self.get_column_indice(recommend_sheet, INDEX_NAME_ROW, "recommend_item_list1")
        recommend_field_list_column_indice += self.get_column_indice(recommend_sheet, INDEX_NAME_ROW, "recommend_item_list2")
        recommend_field_list_column_indice += self.get_column_indice(recommend_sheet, INDEX_NAME_ROW, "recommend_item_list3")
        for r in range(INDEX_DATA_ROW, recommend_sheet.nrows):
            id_cell = recommend_sheet.cell(r, recommend_field_id_column_index)
            id = unicode(int(float(id_cell.value)))
            recommand_item_map[id] = recommend_sheet.row(r)
        
        hero_book = self.get_book("arena_object")
        sheet = hero_book.sheet_by_name("ARENA_OBJECT_CONF")
        field_maps_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "maps")[0]
        field_type_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "arena_type")[0]
        field_name_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "name")[0]
        field_id_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "id")[0]

        for r in range(INDEX_DATA_ROW, sheet.nrows):
            cell_type = sheet.cell(r, field_type_column_index)
            if cell_type.ctype == xlrd.XL_CELL_EMPTY or \
                unicode(cell_type.value).lower() != "hero".decode(encoding="utf-8"):
                continue
            hero_name = unicode(sheet.cell(r, field_name_column_index).value).encode(encoding="utf-8")
            maps_cell = sheet.cell(r, field_maps_column_index)
            if not (arena_mode in self.parse_int_array(unicode(maps_cell.value))):
                continue
            id_cell = sheet.cell(r, field_id_column_index)
            id = unicode(int(float(id_cell.value)))
            if not recommand_item_map.has_key(id):
                msg = "item#RECOMMEND_ITEM_CONF|英雄(%d:%s)|没有推荐装备"%(int(id), hero_name)
                errors.append(msg)
                self.log(2, msg)
            else:
                recommend_item = recommand_item_map[id]
                for c in recommend_field_list_column_indice:
                    recommend_field_name = unicode(recommend_sheet.cell(INDEX_NAME_ROW, c).value).encode(encoding="utf-8")
                    list_cell = recommend_item[c]
                    if list_cell.ctype == xlrd.XL_CELL_EMPTY:
                        msg = "item#RECOMMEND_ITEM_CONF|英雄(%d:%s)推荐装备(%s)未配置"%(int(id),hero_name, recommend_field_name)
                        errors.append(msg)
                        self.log(2, msg)
                    else:
                        item_array = self.parse_int_array(unicode(list_cell.value))
                        enabled_num = 0
                        for item_id in item_array:
                            if item_map.has_key(item_id):
                                enabled_num += 1
                        if enabled_num == 0:
                            msg = "item#RECOMMEND_ITEM_CONF|英雄(%d:%s)在地图(arena_mode=%d)中没有上架的推荐装备(%s)"%(int(id), hero_name, int(arena_mode), recommend_field_name)
                            errors.append(msg)
                            self.log(2, msg)
        self.__assert__(errors)

    def check_equipment_stat(self):
        """
        校验装备属性名有效性
        """
        self.log(1,"ITEM_CONF|check_item_stat|装备属性名有效校验")
        self.check_stat("ITEM_CONF", ["stat_name"])

