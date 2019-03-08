#!/usr/bin/env python
#encoding: utf-8

import os, sys, re, platform
import xlrd, xlwt, xlutils

def os_encode(str_data):
    if platform.system().lower() == "windows":
        str_data = str(str_data).decode(encoding='utf-8').encode(encoding='gbk', errors = 'ignore')
    return str_data

def get_default_cell_style():
    font = xlwt.Font()
    font.name = 'Courier New'
    font.height = 12 * 0x14 # 16pt
    style = xlwt.XFStyle()
    style.font = font
    from xlwt import Alignment
    style.alignment = Alignment()
    style.alignment.horz = Alignment.HORZ_LEFT
    style.alignment.vert = Alignment.VERT_CENTER
    return style

def load_book(xls_file_path):
    return xlrd.open_workbook(xls_file_path, encoding_override="cp1252")

def export_csharp_enum(enum_map, enum_name, def_namespace = None):
    def indent_code(depth, code_line):
        return ' ' * 4 * depth + code_line + '\n'
    depth = 0
    text_content = ''
    if def_namespace:
        text_content += indent_code(depth, 'namespace %s'%(def_namespace))
        text_content += indent_code(depth, '{')
        depth += 1
    text_content += indent_code(depth, 'public enum %s'%(enum_name))
    text_content += indent_code(depth, '{')
    depth += 1
    enum_info_list = []
    for label, enum_info in enum_map.items():
        enum_info_list.append(enum_info)
    enum_info_list.sort(lambda a, b: 1 if a[1] > b[1] else -1)
    for enum_info in enum_info_list:
        text_content += indent_code(depth, '%s = %d, /* %s */'%(enum_info[0].encode('utf-8'), enum_info[1], enum_info[2].encode('utf-8')))
    while depth > 0:
        depth -= 1
        text_content += indent_code(depth, '}')
    return text_content

def export_attack_sound_conf(src_xls_path, dst_xls_path):
    book = load_book(src_xls_path)
    sheet = book.sheet_by_index(0)

    row_enum_map, col_enum_map = ({}, {})

    index = 1
    for c in range(2, sheet.ncols):
        cell = sheet.cell(0, c)
        if cell.ctype == xlrd.XL_CELL_EMPTY:
            continue
        label = unicode(cell.value).strip()
        if col_enum_map.get(label):
            raise Exception('护甲值枚举<%s>重复定义'%(label.encode('utf-8')))
            continue
        col_enum_map[label] = (label, index, unicode(sheet.cell(1, c).value).strip())
        index += 1

    index = 1
    for r in range(2, sheet.nrows):
        cell = sheet.cell(r, 0)
        if cell.ctype == xlrd.XL_CELL_EMPTY:
            continue
        label = unicode(cell.value).strip()
        if row_enum_map.get(label):
            raise Exception('武器值枚举<%s>重复定义'%(label.encode('utf-8')))
            continue
        row_enum_map[label] = (label, index, unicode(sheet.cell(r, 1).value).strip())
        index += 1
    
    style = get_default_cell_style()

    dst_book = xlwt.Workbook(encoding='utf-8')
    dst_sheet = dst_book.add_sheet(u'ATTACK_SOUND_CONF', cell_overwrite_ok = False)

    row_index = 0
    def write_row_values(value_list):
        for c in range(len(value_list)):
            dst_sheet.write(row_index, c, value_list[c], style)
        dst_sheet.row(row_index).height_mismatch = True
        dst_sheet.row(row_index).height = 2 * 256
        return row_index + 1
    row_index = write_row_values([u'optional'] * 2)
    row_index = write_row_values([u'uint32', u'string'])
    row_index = write_row_values([u'key', u'sound'])
    row_index = write_row_values([u'c', u'c'])
    row_index = write_row_values([u'音效ID', u'攻击音效'])
    for r in range(2, sheet.nrows):
        for c in range(2, sheet.ncols):
            cell = sheet.cell(r, c)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            sound_path = unicode(cell.value).strip()
            if not sound_path:
                continue
            row_label = unicode(sheet.cell(r, 0).value).strip()
            col_label = unicode(sheet.cell(0, c).value).strip()
            if not (row_label and col_label):
                continue
            id = row_enum_map[row_label][1] * 100 + col_enum_map[col_label][1]
            row_index = write_row_values([id, sound_path])
    dst_sheet.col(0).width = 256 * 14
    dst_sheet.col(1).width = 256 * 80
    dst_book.save(dst_xls_path)

    import os.path as p
    csharp_output_path = p.abspath(p.join(p.dirname(src_xls_path), '../../Assets/Scripts/Sounds'))

    enum_class_info = [(row_enum_map, 'WeaponType'), (col_enum_map, 'ArmorType')]
    for enum_map, enum_name in enum_class_info:
        csharp_enum_text = export_csharp_enum(enum_map, enum_name, 'TheNextMoba.Sounds')
        print os_encode(csharp_enum_text)
        class_path = p.abspath('%s/%s.cs'%(csharp_output_path, enum_name))
        with open(class_path, 'w') as fp:
            fp.write(csharp_enum_text)
            fp.close()
            print os_encode('SAVE => %s'%(class_path))
    
def main():
    import os.path as p
    src_xls_path = p.join(p.dirname(p.abspath(__file__)), '..', 'DataConfig', 'attack_sound.xls')
    dst_xls_path = re.sub(r'(\.xls)$', r'_tmp\g<1>', src_xls_path)
    export_attack_sound_conf(src_xls_path, dst_xls_path)

if __name__ == '__main__':
    main()