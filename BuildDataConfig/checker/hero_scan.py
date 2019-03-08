#!/usr/bin/env python
#encoding: utf-8

from xlsdata.utils import os_encode
from base_check import INDEX_DATA_ROW, INDEX_NAME_ROW
import xlrd, xlutils, xlwt
import sys, os, re
import base_check

class HeroAssetScanner(base_check.BaseChecker):
    def __init__(self):
        import os.path as p
        xls_root_path = p.abspath(p.join(p.dirname(p.abspath(__file__)), '..', 'DataConfig'))
        super(HeroAssetScanner, self).__init__('arena_object', xls_root_path)
        self.hero_map = {}
        self.init()

    def run(self):
        sheet = self.sheet = self.book.sheet_by_name('ARENA_OBJECT_CONF')
        id_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'id')[0]
        mode_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'maps')[0]
        type_column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'arena_type')[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            id_cell = sheet.cell(r, id_column_index)
            if id_cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            id = self.parse_int_string(id_cell.value)
            type_cell = sheet.cell(r, type_column_index)
            if unicode(type_cell.value).strip().lower() != u'hero':
                continue
            mode_cell = sheet.cell(r, mode_column_index)
            # if mode_cell.ctype == xlrd.XL_CELL_EMPTY: # 上架过滤
            #     continue
            self.hero_map[id] = sheet.row(r)
        
        self.scan_ability()
        self.scan_attack()

    def create_id_map(self, sheet):
        id_map = {}
        column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, 'id')[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            cell = sheet.cell(r, column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            id = self.parse_int_string(cell.value)
            id_map[id] = (r, sheet.row(r))
        return id_map

    def mark_row_red_color(self, rsheet, wsheet, row_index):
        font = xlwt.Font()
        font.name = 'Courier New'
        font.height = 12 * 0x14
        style = xlwt.XFStyle()
        style.font = font
        pattern = xlwt.Pattern()
        pattern.pattern = xlwt.Pattern.SOLID_PATTERN
        pattern.pattern_fore_colour = xlwt.Style.colour_map['bright_green']
        style.pattern = pattern
        r = row_index
        for c in range(rsheet.ncols):
            wsheet.write(r, c, rsheet.cell(r, c).value, style)

    def mark_workbook(self, book_name, sheet_name, compare_map):
        r_book = self.get_book(book_name)
        r_sheet = r_book.sheet_by_name(sheet_name)
        from xlutils.copy import copy
        w_book = copy(r_book)
        w_sheet = w_book.get_sheet(w_book.sheet_index(sheet_name))
        changed = False
        id_map = self.create_id_map(r_sheet)
        for item_id in id_map.iterkeys():
            if compare_map.has_key(item_id):
                item_data = id_map[item_id]
                row_index = int(item_data[0])
                self.mark_row_red_color(r_sheet, w_sheet, row_index)
                changed = True
        book_path = re.sub(r'x$', '', self.get_book_path(book_name))
        pattern = re.compile(r'(\.xls)$', re.IGNORECASE)
        book_path = pattern.sub(r'_mark\g<1>', book_path)
        print book_path
        if changed:
            w_book.save(book_path)
            print os_encode('save => %s'%(book_path))

    def scan_ability(self):
        sheet = self.sheet
        related_column_indice = []
        for field_name in (u'passive_ability', u'talent_ability', u'combo_ability'):
            related_column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, field_name)
        combo_ability_map = {}
        for hero_id in self.hero_map.iterkeys():
            hero_data = self.hero_map[hero_id]
            for c in related_column_indice:
                cell = hero_data[c]
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                ability_list = self.parse_int_array(unicode(cell.value))
                for ability_id in ability_list:
                    combo_ability_map[ability_id] = hero_data
        
        self.mark_workbook('combo_ability', 'COMBO_ABILITY', combo_ability_map)

        combo_sheet = self.get_book('combo_ability').sheet_by_name('COMBO_ABILITY')
        ability_column_index = self.get_column_indice(combo_sheet, INDEX_NAME_ROW, 'atomic_ability')[0]
        ability_map = {}
        for r in range(INDEX_DATA_ROW, combo_sheet.nrows):
            cell = combo_sheet.cell(r, ability_column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            ability_list = self.parse_int_array(unicode(cell.value))
            for ability_id in ability_list:
                ability_map[ability_id] = combo_sheet.row(r)
        
        self.mark_workbook('ability', 'ABILITY', ability_map)

    def scan_attack(self):
        sheet = self.sheet
        related_column_indice = []
        for field_name in (u'attack_id', u'strengthen_attack_id'):
            related_column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, field_name)
        attack_map = {}
        for hero_id in self.hero_map.iterkeys():
            hero_data = self.hero_map[hero_id]
            for c in related_column_indice:
                cell = hero_data[c]
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                attack_list = self.parse_int_array(unicode(cell.value))
                for attack_id in attack_list:
                    attack_map[attack_id] = hero_data
        self.mark_workbook('attack', 'ATTACK_CONF', attack_map)

def main():
    scanner = HeroAssetScanner()
    scanner.run()

if __name__ == '__main__':
    main()