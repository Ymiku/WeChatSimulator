#!/usr/bin/env python
#encoding:utf-8
import os, re, sys, xlrd, xlwt, xlutils, md5
from __init__ import DATA_PATH, PATTERN_XLS_SUFFIX
from __init__ import FieldIndex, HeaderIndex, TranslationDatabase, TranslationTable, DatabaseType

def load_book(url):
    return xlrd.open_workbook(url)

def dump_hash_map(url, sheet_name, column):
    import os.path as p
    url = p.abspath(url)
    sheet = load_book(url).sheet_by_name(sheet_name)
    field_name = unicode(sheet.cell(2, column).value).encode('utf-8')
    result = {}
    for r in range(5, sheet.nrows):
        id = re.sub(r'\.0$','', unicode(sheet.cell(r, 0).value))
        cell = sheet.cell(r, column)
        text = unicode(cell.value).strip().encode('utf-8')
        hash = md5.md5(text[0:50]).hexdigest()
        result[hash] = [id, text]
    return result

def create_id_map():
    import os.path as p
    src_map = dump_hash_map(p.join(DATA_PATH, 'combo_ability.xls'), 'COMBO_ABILITY',5)
    dst_map = dump_hash_map(p.join(DATA_PATH, 'arena_tips.xlsx'), 'ARENA_TIPS', 1)
    dict = {}
    for hash, dst_data in dst_map.items():
        src_data = src_map.get(hash)
        dict[src_data[0]] = dst_data[0]
    return dict

def main():
    import os.path as p
    library = TranslationDatabase(type = DatabaseType.LIBRARY)
    table = library.get_table('COMBO_ABILITY')
    translate_map = table.strip_translate_map()

    tips = TranslationDatabase(type = DatabaseType.MEMORY)
    tips_table = tips.get_table('ARENA_TIPS')

    id_map = create_id_map()
    print id_map
    
    field_name = 'description'
    for id, value in translate_map.get(field_name).items():
        if unicode(id).isupper():
            continue
        if not id_map.get(id):
            continue
        label = u'COMBO_ABILITY-id-%s-%s'%(id, field_name)
        tips_label = u'ARENA_TIPS-id-%s-%s'%(id_map[id], field_name)
        tips_table.write(tips_label, FieldIndex.CHINESE, table.get(label)[FieldIndex.CHINESE])
        tips_table.write(tips_label, FieldIndex.JAPANESE, table.get(label)[FieldIndex.JAPANESE])
    tips.save_as('commits/m_arena_tips.xls')

if __name__ == '__main__':
    main()