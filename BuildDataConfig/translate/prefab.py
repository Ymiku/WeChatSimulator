#!/usr/bin/env python
#encoding: utf-8
from __init__ import PATTERN_CHINESE, ROOT_PATH, TranslationDatabase, TranslationTable, DatabaseType
import os, re, sys, md5
import yaml, tempfile, string, json, xlwt
from checker.xlsdata.utils import is_winsystem, os_encode
import tempfile
import os.path as p

PATTERN_PREFAB_NAME = re.compile(r'\.(prefab|unity)$', re.IGNORECASE)
PATTERN_TEXT_START = re.compile(r'^(\s*-?\s*)(m_Text:\s*)(\".+)$')
PATTERN_TEXT_CLOSE = re.compile(r'\"$')
PATTERN_ASCII = re.compile(r'^[%s]+$'%(string.printable), re.IGNORECASE)
PREFAB_SHEET_NAME = 'PREFAB_TEXT'
TEXT_KEY = 'm_Text'

class PrefabTranslator(object):
    def __init__(self, prefab_root_paths):
        self.prefab_root_paths = prefab_root_paths
        self.cyaml_installed = True
        self.collect_records = []
        try:
            import yaml.cyaml
        except ImportError, e:
            self.cyaml_installed = False
            print os_encode('macOS系统通过命令行<brew install libyaml>安装libyaml，可显著提升yaml解析速度')
        # print self.dumper, self.loader
    
    @property
    def loader(self):
        loader = yaml.Loader
        if self.cyaml_installed:
            loader = yaml.CLoader
        return loader
        
    @property
    def dumper(self):
        dumper = yaml.Dumper
        if self.cyaml_installed:
            dumper = yaml.CDumper
        return dumper

    def get_prefab_list(self):
        prefab_list = []
        for root_path in self.prefab_root_paths:
            for base_path, _, file_name_list in os.walk(root_path):
                for file_name in file_name_list:
                    if PATTERN_PREFAB_NAME.search(file_name) == None:
                        continue
                    prefab_list.append(os.path.join(base_path, file_name))
        return prefab_list
    
    def create_text_map(self):
        text_map = {}
        self.collect_records = []
        for prefab_path in self.get_prefab_list():
            with open(prefab_path, 'r') as f:
                closed, line_num = True, 0
                for line in f:
                    line_num += 1
                    if closed:
                        text = line
                        match = PATTERN_TEXT_START.match(line)
                        if match == None:
                            continue
                        text = ''.join(match.groups()[1:])
                    else:
                        text += line
                    closed = PATTERN_TEXT_CLOSE.search(line) != None
                    if closed:
                        text_obj = yaml.load(text, Loader=self.loader)
                        value = text_obj[TEXT_KEY]
                        if PATTERN_CHINESE.search(value) != None:
                            label = md5.md5(value.encode('utf-8')).hexdigest()
                            if not text_map.has_key(label):
                                text_map[label] = value
                                self.collect_records.append([label, value, line_num, prefab_path])
                                # print '%s[%s] %s'%(text, value.encode('utf-8'), label)
        return text_map

    def translate(self, text_map):
        def uniform(unicode_value):
            if PATTERN_ASCII.match(unicode_value):
                return unicode_value.encode('utf-8')
            return unicode_value
        trim = re.compile(r'(^\s+|\n$)')
        for prefab_path in self.get_prefab_list():
            translated_prefab = open(tempfile.mktemp(), mode='w+')
            changed = False
            with open(prefab_path, 'r') as f:
                closed = True
                text_indent, line_num = '', 0
                for line in f:
                    line_num += 1
                    if closed:
                        text = line
                        match = PATTERN_TEXT_START.match(line)
                        if match == None:
                            translated_prefab.write(text)
                            text_indent = ''
                            continue
                        text_indent = match.group(1)
                        text = ''.join(match.groups()[1:])
                    else:
                        text += line
                    closed = PATTERN_TEXT_CLOSE.search(line) != None
                    if closed:
                        text_obj = yaml.load(text, Loader=self.loader)
                        value = text_obj[TEXT_KEY]
                        if PATTERN_CHINESE.search(value) != None:
                            label = md5.md5(value.encode('utf-8')).hexdigest()
                            if text_map.has_key(label):
                                text_obj = {}
                                text_obj[TEXT_KEY] = uniform(text_map[label])
                                prev_text = text
                                text = yaml.dump(text_obj, Dumper=self.dumper, default_flow_style = False)
                                text_changed = value != text_obj[TEXT_KEY]
                                if text_changed:
                                    changed = True
                                    print '%s[%s] => %s[%s]'%(trim.sub('', prev_text), value.encode('utf-8'), trim.sub('', text), text_obj[TEXT_KEY].encode('utf-8'))
                        text = text_indent + re.sub(r'\n$', '', text) + '\n'
                        translated_prefab.write(text)
            if changed:
                translated_prefab.seek(0)
                prefab_file = open(prefab_path, 'w')
                prefab_file.write(translated_prefab.read())
                prefab_file.close()
                print 'TANSLATED => %s'%(prefab_path)
                translated_prefab.close()
                os.remove(translated_prefab.name)
    
    def dump_collect_report(self, incremental = True, update_database = True, exclude_library_items = False):
        if self.collect_records == None or len(self.collect_records) == 0:
            return None
        current_text_map = None
        database_path = p.join(p.dirname(p.abspath(__file__)), 'database/prefab.txt')
        if incremental:
            current_text_map = load_collect_report(file_path = database_path, use_unicode = True)
        database = None
        if update_database: database = open(database_path, 'w+')

        temp = open(tempfile.mktemp('dump_collect_details.txt'), 'w+')
        file_name_list, maxlen = [], 0
        for n in range(len(self.collect_records)):
            item = self.collect_records[n]
            file_name = item[-1].split('/')[-1]
            length = len(file_name)
            if length > maxlen: maxlen = length
            file_name_list.append(file_name)
        if exclude_library_items:
            library = TranslationDatabase(type = DatabaseType.LIBRARY)
            prefab_table = library.get_table(PREFAB_SHEET_NAME)
        format = u'%%%ds  %%s  %%5d  "%%s"  %%s\n'%(maxlen)
        for n in range(len(self.collect_records)):
            item = self.collect_records[n]
            line = format%(file_name_list[n], item[0], item[-2], re.sub(ur'[\n\r]',r'\\n',item[1]), item[-1])
            if database: database.write(line.encode('utf-8'))
            if exclude_library_items and prefab_table.has(item[0]): continue
            if current_text_map:
                if current_text_map.get(item[0]): continue
            temp.write(line.encode('utf-8'))
        if database: database.close()
        return temp

def translate(text_map):
    translator = PrefabTranslator(__get_prefab_paths())
    translator.translate(text_map)

def dump_text_map():
    translator = PrefabTranslator(__get_prefab_paths())
    return translator.create_text_map()

def dump_collect_report(incremental = True, update_database = False):
    translator = PrefabTranslator(__get_prefab_paths())
    translator.create_text_map()
    temp = translator.dump_collect_report(incremental = incremental, update_database = update_database)
    temp.seek(0, os.SEEK_SET)
    content = temp.read()
    os.remove(temp.name)
    return content

def __parse_record(text):
    return text

def load_collect_report(file_path, use_unicode = True):
    text_map = {}
    if not os.path.exists(file_path): return text_map
    pattern = re.compile(r'^(\w+\.(prefab|unity))\s+([a-f0-9]{32})\s+(\d+)\s+"(.+)"\s+([^\n]+)', re.IGNORECASE)
    with open(file_path, 'r') as fp:
        for line in fp.readlines():
            line = re.sub(r'\n$', '', line).strip()
            if not line:
                continue
            match = pattern.search(line)
            if not match:
                raise Exception('不支持的数据行:\n%s'%(line))
                continue
            label = match.group(3)
            value = re.sub(r'\\n', r'\n', match.group(5))
            if use_unicode:
                label = label.decode('utf-8')
                value = value.decode('utf-8')
            if text_map.has_key(label):
                continue
            text_map[label] = value
    return text_map

def load_modification(file_path):
    text_map = load_collect_report(file_path, use_unicode = True)
    print json.dumps(text_map, ensure_ascii=False, sort_keys=True, indent=4)
    translator = PrefabTranslator(__get_prefab_paths())
    translator.translate(text_map)

def dump():
    return json.dumps(dump_text_map(), ensure_ascii=False, sort_keys=True, indent=4)

def __get_prefab_paths():
    path_list = []
    path_list.append('Assets/Resources/Prefabs/UI')
    path_list.append('Assets/Resources/Prefabs/Environment')
    path_list.append('Assets/Scenes')
    root_dir = p.abspath(p.join(p.dirname(p.abspath(__file__)), '..', '..'))
    for n in range(len(path_list)):
        path_list[n] = p.join(root_dir, path_list[n])
    return path_list

def main():
    if len(sys.argv) > 1:
        command = sys.argv[1].lower()
        if command == 'load':
            report_path = p.join(p.dirname(p.abspath(__file__)), 'database/prefab.txt')
            if len(sys.argv) > 2:
                report_path = sys.argv[2]
            load_modification(file_path = p.abspath(report_path))
        elif command == 'dump':
            print dump_collect_report(incremental = False, update_database = False)
        elif command == 'dump_delta':
            print dump_collect_report(incremental = True, update_database = False)
        elif command == 'dump_db':
            print dump_collect_report(incremental = False, update_database = True)
        elif command == 'mail':
            print dump_collect_report(incremental = True, update_database = True)
    else:
        print dump_collect_report()

if __name__ == '__main__':
    main()
