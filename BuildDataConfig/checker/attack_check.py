#!/usr/bin/env python
#encoding: utf-8

import base_check
from base_check import *
import xlrd

class AttackChecker(base_check.BaseChecker):
    
    def check_all(self):
        """
        配置表[attack.xls]所有校验
        """
        super(AttackChecker,self).check_all()
        self.check_crit_hit_effect_id()
        self.check_hit_effect_id()

    def check_hit_effect_id(self):
        """
        hit_effect_id只填写1个时，表示所有普攻受击都采用同一个受击美术特效；
        如果填写多个，则需要按照字段“attack_anim_name”中配置的顺序挨个配置，数量和顺序必须严格一致
        """
        self.log(1, "ATTACK_CONF|普攻效果(hit_effect_id)与普攻动画(attack_anim_name)数量匹配校验")
        errors= []

        sheet = self.book.sheet_by_name("ATTACK_CONF")
        effect_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "hit_effect_id")[0]
        animat_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "attack_anim_name")[0]

        for r in range(INDEX_DATA_ROW, sheet.nrows):
            effect_cell_vlaue = unicode(sheet.cell_value(r, effect_index))
            animat_cell_vlaue = unicode(sheet.cell_value(r, animat_index))
            effect_array = self.parse_int_array(effect_cell_vlaue)
            animat_array = self.parse_str_array(animat_cell_vlaue)
            if len(effect_array) > len(animat_array):
                msg = "ATTACK_CONF|%3d行|普攻效果[%d:%s列](%s)与普攻动画[%d:%s列](%s)数量不匹配"%(r+1, \
                    effect_index+1, self.column_to_alphabet(effect_index), self.array_to_string(effect_array), \
                    animat_index+1, self.column_to_alphabet(animat_index), self.array_to_string(animat_array))
                errors.append(msg)
                self.log(2, msg)
        self.__assert__(errors)

    def check_crit_hit_effect_id(self):
        """
        crit_hit_effect_id只填写1个时，表示所有暴击受击都采用同一个受击美术特效；
        如果填写多个，则需要按照字段“crit_anim_name”中配置的顺序挨个配置，数量和顺序必须严格一致
        """
        self.log(1, "ATTACK_CONF|暴击效果(crit_hit_effect_id)与暴击动画(crit_anim_name)数量匹配校验")
        errors= []

        sheet = self.book.sheet_by_name("ATTACK_CONF")
        effect_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "crit_hit_effect_id")[0]
        animat_index = self.get_column_indice(sheet, INDEX_NAME_ROW, "crit_anim_name")[0]

        for r in range(INDEX_DATA_ROW, sheet.nrows):
            effect_cell_vlaue = unicode(sheet.cell_value(r, effect_index))
            animat_cell_vlaue = unicode(sheet.cell_value(r, animat_index))
            effect_array = self.parse_int_array(effect_cell_vlaue)
            animat_array = self.parse_str_array(animat_cell_vlaue)
            if len(effect_array) > len(animat_array):
                msg = "ATTACK_CONF|%3d行|暴击效果[%d:%s列](%s)与暴击动画[%d:%s列](%s)数量不匹配"%(r+1,   \
                    effect_index+1, self.column_to_alphabet(effect_index), self.array_to_string(effect_array), \
                    animat_index+1, self.column_to_alphabet(animat_index), self.array_to_string(animat_array))
                errors.append(msg)
                self.log(2, msg)
        self.__assert__(errors)

