#!/usr/bin/env python
#encoding: utf-8
import os, sys, re, enum
import xlwt, xlrd

WORK_PATH = os.path.dirname(os.path.abspath(__file__))
ROOT_PATH = os.path.abspath(os.path.join(WORK_PATH, '..'))
DATA_PATH = os.path.abspath(os.path.join(ROOT_PATH, 'DataConfig'))
sys.path.append(ROOT_PATH)

PATTERN_XLS_NAME = re.compile(r'^[a-z0-9_]+\.xls[x]?$', re.IGNORECASE)
PATTERN_XLS_SUFFIX = re.compile(r'\.xls[x]?$', re.IGNORECASE)
PATTERN_CHINESE = re.compile(ur'[\u4E00-\u9FFF]+')
PATTERN_NOT_CHINESE = re.compile(ur'[^\u4E00-\u9FFF]+')
PATTERN_FIELD_NAME = re.compile(ur'^[a-z_][0-9a-z_]*$', re.IGNORECASE)

from checker import base_check
from checker.base_check import INDEX_RULE_ROW, INDEX_DATA_ROW, INDEX_NAME_ROW
from checker.xlsdata.utils import os_encode

class DatabaseType(enum.Enum):
    LIBRARY, COLLECT, MERGE, EXCLUDE, MEMORY = ('library', 'collect', 'merge', 'exclude', 'memory')

class HeaderIndex(object):
    NAME, DATA = range(2)

class FieldIndex(object):
    LABEL, CHINESE, JAPANESE = range(3)

    __locale_names = None

    @classmethod
    def chinese_name(self, value):
        if not self.__locale_names:
            import os.path as p
            with open(p.join(p.dirname(p.abspath(__file__)), 'locale_names.txt')) as fp:
                import json
                self.__locale_names = json.load(fp)
        if type(value) == int:
            name = self.name(value)
        elif type(value) == str:
            name = value
        else:
            return None
        if name:
            return self.__locale_names.get(name)
        return None

    @classmethod
    def name(self, index):
        if index == self.LABEL:
            return 'label'
        if index == self.CHINESE:
            return 'zh_Hans_CN'
        if index == self.JAPANESE:
            return 'ja_JP'
        return None
    @classmethod
    def index(self, name):
        name = name.lower().strip()
        dict = vars(self)
        for key in dict.keys():
            if str(key).isupper():
                _name = self.name(dict[key])
                if _name.lower() == name.lower():
                    return dict[key]
        return None
    __indice = None
    @classmethod
    def indice(self):
        if self.__indice == None:
            dict, list = vars(self), []
            for key in dict.keys():
                if str(key).isupper():
                    list.append(dict[key])
            list.sort()
            self.__indice = list
        return self.__indice

class CompareStatus(enum.Enum):
    NONE, IDENTICAL, MISSING, MODIFIED, ADDED = range(5)

def get_default_cell_style():
    font = xlwt.Font()
    font.name = 'Courier New'
    font.height = 12 * 0x14 # 16pt
    style = xlwt.XFStyle()
    style.font = font
    from xlwt import Alignment
    style.alignment = Alignment()
    style.alignment.horz = Alignment.HORZ_CENTER
    style.alignment.vert = Alignment.VERT_CENTER
    return style

class TranslationTable(object):
    def __init__(self, name, sheet, parent_book):
        self.name = name
        self.__help = base_check.BaseChecker(None, DATA_PATH)
        self.__sheet, self.__host_book = sheet, parent_book
        self.__data = {}
        if not self.__sheet == None:
            column_map = {}
            for c in range(self.__sheet.ncols):
                cell = self.__sheet.cell(HeaderIndex.NAME, c)
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                field_name = unicode(cell.value).encode('utf-8')
                index = FieldIndex.index(field_name)
                if index == None:
                    print os_encode('[%r]%d行%s列字段(%s)未定义'%(self.name, HeaderIndex.NAME + 1, self.__help.column_to_alphabet(c), field_name))
                    continue
                column_map[index] = c
            def uniform(sheet_row):
                result = []
                for index in FieldIndex.indice():
                    if column_map.has_key(index):
                        map_index = column_map[index]
                        result.append(sheet_row[map_index].value)
                    else:
                        result.append(None)
                return result
            for r in range(HeaderIndex.DATA, self.__sheet.nrows):
                label_cell = self.__sheet.cell(r, FieldIndex.LABEL)
                if label_cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                label = unicode(label_cell.value)
                value = uniform(self.__sheet.row(r))
                if not self.__data.has_key(label):
                    self.__data[label] = value
                else:
                    print os_encode('KEY[%s]重复(%s => %s)'%(label.encode('utf-8'),\
                        self.dump_row_string(self.__data[label]), \
                        self.dump_row_string(value)))
            # print os_encode('table[%s].size = %r'%(self.name, len(self.data.keys())))

    def dump_row_string(self, row):
        return ' - '.join([value.encode('utf-8') for value in row])

    @property
    def is_empty(self):
        return len(self.__data.keys()) == 0

    @property
    def data(self):
        return self.__data

    def strip_text_map(self, field_index = FieldIndex.JAPANESE):
        text_map = {}
        for label in self.__data.iterkeys():
            target_text = self.__data[label][field_index]
            if target_text:
                text_map[label] = target_text
        return text_map

    def strip_translate_map(self, field_index = FieldIndex.JAPANESE):
        pattern = re.compile(r'^([^-]+)-([^-]+)-([^-]+)-([^-]+)$')
        hash_pattern = re.compile(r'^([^-]+)-([^-]+)-([0-9a-f]{32})$', re.IGNORECASE)
        translate_map = {}
        for label in self.__data.iterkeys():
            match = pattern.match(label)
            if match == None:
                match = hash_pattern.match(label)
                if match == None:
                    msg = '[SHEET][%s]字段(label=%s)不符合正则规范<%s>'%(self.name.encode('utf-8'), label.encode('utf-8'), pattern.pattern)
                    print os_encode(msg)
                    continue
                index_field_name = u'HASH'
                sheet_name = match.group(1)
                text_field_name = match.group(2).lower()
                text_key = match.group(3)
            else:
                sheet_name = match.group(1)
                index_field_name = match.group(2)
                text_key = match.group(3)
                text_field_name = match.group(4).lower()
            if len(translate_map.keys()) == 0:
                translate_map['SHEET_NAME'] = sheet_name
            if not translate_map.has_key(text_field_name):
                translate_map[text_field_name] = {}
            text_map = translate_map[text_field_name]
            text_map['INDEX_FIELD'] = index_field_name
            if text_map['INDEX_FIELD'] != index_field_name:
                msg = '[SHEET][%s]配置表索引字段变更(%s => %s)'%(self.name.encode('utf-8'), text_map['INDEX_FIELD'].encode('utf-8'), index_field_name.encode('utf-8'))
                raise Exception(os_encode(msg))
                continue
            if not text_map.has_key(text_key):
                translated_text = self.__data[label][field_index]
                if translated_text:
                    text_map[text_key] = translated_text
            else:
                msg = '[SHEET][%s]字段(label=%s)重复'%(self.name.encode('utf-8'), label.encode('utf-8'))
                print os_encode(msg)
        return translate_map

    def write(self, label, index, value):
        column_indice = FieldIndex.indice()
        if self.__data.has_key(label):
            # print os_encode('M [%s]INDEX[%d]覆盖(%s => %s)'%(label.encode('utf-8'), \
            #     index, self.__data[label][index].encode('utf-8'), value.encode('utf-8')))
            pass
        else:
            # print os_encode('A [%s]INDEX[%s]%s'%(label.encode('utf-8'), index, value.encode('utf-8')))
            item = [None] * len(column_indice)
            item[FieldIndex.LABEL] = label
            self.__data[label] = item
        if index in column_indice:
            self.__data[label][index] = value

    def get(self, label):
        return self.__data.get(label)

    def has(self, label):
        return self.get(label) != None

    def __get_cell_style(self):
        return get_default_cell_style()

    @property
    def character_count(self):
        num = 0
        for label in self.__data.keys():
            value = self.__data[label][FieldIndex.CHINESE]
            if value == None:
                continue
            chinese_characters = PATTERN_NOT_CHINESE.sub('', value)
            num += len(chinese_characters)
        return num

    def flush(self, verbose = True):
        try:
            index = self.__host_book.sheet_index(self.name)
            del self.__host_book._Workbook__worksheet_idx_from_name[self.name.lower()]
            del self.__host_book._Workbook__worksheets[index]
        except Exception:
            pass
        if self.is_empty:
            return
        sheet = self.__host_book.add_sheet(self.name, cell_overwrite_ok=False)
        style = self.__get_cell_style()
        row_index = 0
        row_height = 2 * 256
        for c in FieldIndex.indice():
            sheet.write(row_index, c, FieldIndex.name(c), style)
            sheet.col(c).width = 256 * (80 if c == 0 else 100)
        sheet.row(row_index).height_mismatch = True
        sheet.row(row_index).height = row_height
        row_index += 1
        label_list = self.__data.keys()
        label_list.sort()
        for label in label_list:
            row_value = self.__data[label]
            # print row_value
            for c in FieldIndex.indice():
                cell_value = row_value[c]
                sheet.write(row_index, c, cell_value, style)
            sheet.row(row_index).height_mismatch = True
            sheet.row(row_index).height = row_height
            row_index += 1
        if verbose:
            print os_encode('%25s %6s'%(self.name.encode('utf-8'), '{:,}'.format(self.character_count)))
    
    def merge_for_inspection(self, target_table, field_index):
        if target_table.name != self.name:
            print os_encode('不同表名(self.name=%s <=> target.name=%s)子表无法相互比较'%(self.name.encode('utf-8'), target_table.name.encode('utf-8')))
            return
        for label in target_table.__data.iterkeys():
            if not self.__data.has_key(label):
                self.__data[label] = target_table.__data[label]
            else:
                target_value = target_table.__data[label][field_index]
                self.write(label, field_index, target_value)

    def merge_from_translation(self, target_table, field_index = FieldIndex.JAPANESE, update_indice = [FieldIndex.CHINESE], verbose = True):
        if target_table.name != self.name:
            msg = os_encode('不同表名(self.name=%s <=> target.name=%s)子表无法相互比较'%(self.name.encode('utf-8'), target_table.name.encode('utf-8')))
            raise Exception(msg)
            return
        for label in target_table.__data.iterkeys():
            status = CompareStatus.NONE
            target_value = target_table.__data[label][field_index]
            if not target_value:
                # print 'SKIP type:%r label:%r value:%r'%(type(target_value), label, target_value)
                continue
            curent_value = None
            if not self.__data.has_key(label):
                status = CompareStatus.ADDED
            else:
                curent_value = self.__data[label][field_index]
                status = CompareStatus.MODIFIED if target_value != curent_value else CompareStatus.IDENTICAL
            flag = str(status).split('.')[-1][0]
            msg = '%s [%s][%s]%s'%(flag, label.encode('utf-8'), field_index, target_value.encode('utf-8').replace('\n', '\\n'))
            if curent_value != None:
                msg += ' <= %s'%(curent_value.encode('utf-8').replace('\n', '\\n'))
            if verbose:
                print os_encode(msg)
            related_merge_indice = [field_index]
            if update_indice != None:
                related_merge_indice += update_indice
            for index in related_merge_indice:
                target_value = target_table.__data[label][index]
                if target_value == None:
                    continue # 空白列跳过
                self.write(label, index, target_value)

    def compare_for_translation(self, target_table, output_database, field_index = FieldIndex.JAPANESE):
        if target_table.name != self.name:
            msg = os_encode('不同表名(self.name=%s <=> target.name=%s)子表无法相互比较'%(self.name.encode('utf-8'), target_table.name.encode('utf-8')))
            raise Exception(msg)
            return None
        if output_database.has_table(self.name):
            output_database.del_table(self.name)
        output_table = output_database.get_table(self.name, verbose = True)
        for label in target_table.__data.iterkeys():
            status = CompareStatus.NONE
            index = FieldIndex.CHINESE
            target_value = target_table.__data[label][index]
            curent_value = None
            if not self.__data.has_key(label):
                status = CompareStatus.ADDED
            else:
                curent_value = self.__data[label][index]
                status = CompareStatus.MODIFIED if curent_value != target_value else CompareStatus.IDENTICAL
            if status == CompareStatus.IDENTICAL:
                if not self.__data[label][field_index]:
                    status = CompareStatus.ADDED
                else:
                    continue
            flag = str(status).split('.')[-1][0]
            msg = '%s [%s][%s]%s'%(flag, label.encode('utf-8'), index, target_value.encode('utf-8').replace('\n', '\\n'))
            if curent_value != None:
                msg += ' <= %s'%(curent_value.encode('utf-8').replace('\n', '\\n'))
            print os_encode(msg)
            output_table.write(label, index, target_value)
        return output_table

class TranslationDatabase(object):
    def __init__(self, type = DatabaseType.LIBRARY):
        self.type = type
        self.__help = base_check.BaseChecker(xls_name = None, xls_root_path = DATA_PATH)
        self.book, self.__host_book = None, xlwt.Workbook(encoding='utf-8')
        self.__tables = {}
        if self.type != DatabaseType.MEMORY:
            xls_path = os.path.abspath(os.path.join(WORK_PATH, 'database', '%s.xls'%(self.type.value)))
            self.load(xls_path)

    def load(self, database_xls_path):
        self.path = database_xls_path
        if os.path.exists(self.path):
            self.book = self.__help.load_book(self.path)
            for sheet_name in self.book.sheet_names():
                if sheet_name.isupper():
                    self.add_table(sheet_name, self.book.sheet_by_name(sheet_name))
        print 'OPEN => [%s] %s'%(self.type, self.path)
    
    def clear(self):
        self.__tables = {}
    
    @property
    def is_empty(self):
        return len(self.__tables.keys()) == 0

    @property
    def character_count(self):
        num = 0
        for name in self.__tables.keys():
            table = self.__tables[name]
            num += table.character_count
        return num

    def has_table(self, name):
        return self.__tables.has_key(name)

    def del_table(self, name):
        del self.__tables[name]

    def add_table(self, name, sheet):
        table = self.__tables[name] = TranslationTable(name, sheet, self.__host_book)
        return table

    def get_table(self, name, auto_create = True, verbose = False):
        if self.has_table(name):
            return self.__tables[name]
        if not auto_create:
            return None
        table = TranslationTable(name, None, self.__host_book)
        if verbose:
            print os_encode('%s %s#%s'%('CREATE', self.type, name.encode('utf-8')))
        self.__tables[name] = table
        return table
    
    @property
    def tables(self):
        return self.__tables.values()

    def remove_empty_tables(self):
        empty_list = []
        for name in self.__tables.iterkeys():
            table = self.__tables[name]
            if table.is_empty:
                empty_list.append(name)
        for name in empty_list:
            del self.__tables[name]

    def save_as(self, save_file_path):
        self.remove_empty_tables()
        if self.is_empty:
            return
        table_names = self.__tables.keys()
        table_names.sort()
        for name in table_names:
            self.__tables[name].flush(verbose = False)
        self.__host_book.save(save_file_path)

    def save(self, verbose = True):
        self.remove_empty_tables()
        if self.is_empty:
            return
        table_names = self.__tables.keys()
        table_names.sort()
        for name in table_names:
            self.__tables[name].flush(verbose)
        if self.type == DatabaseType.MEMORY:
            return
        self.__host_book.save(self.path)
        print 'SAVE => %s'%(self.path)

class TranslationMergeReport(object):
    def __init__(self):
        self.__book = None
        self.__sheet_count = 0
        self.__sheet_names = []

    def generate(self, src, dst, field_index = FieldIndex.JAPANESE):
        index_map = {}
        index_map[FieldIndex.LABEL] = 0
        index_map[FieldIndex.CHINESE] = 1 * 2
        index_map[field_index] = 2 * 2

        book = self.__book = xlwt.Workbook(encoding='utf-8')
        style = get_default_cell_style()
        row_height = 2 * 256

        real_field_index = index_map[field_index]
        
        table_list = src.tables
        def table_sort_cmp(a, b):
            return 1 if a.name > b.name else -1
        table_list.sort(cmp=table_sort_cmp)

        for src_table in table_list:
            dst_table = dst.get_table(src_table.name)
            sheet_row_list = []
            label_list = src_table.data.keys()
            label_list.sort()
            for label in label_list:
                row_data, changed = ({}, False)
                for index in index_map.keys():
                    col_index = index_map[index]
                    if index == FieldIndex.LABEL:
                        row_data[col_index] = label
                    else:
                        src_value = src_table.data[label][index]
                        dst_value = None
                        if dst_table.data.has_key(label):
                            dst_value = dst_table.data[label][index]
                        if src_value != dst_value:
                            changed = True
                        row_data[col_index - 1] = dst_value
                        row_data[col_index] = src_value
                if changed and row_data.get(real_field_index):
                    sheet_row_list.append(row_data)
            if len(sheet_row_list) == 0:
                continue
            sheet = book.add_sheet(src_table.name)
            row_index = 0
            for c in index_map.keys():
                field_name = FieldIndex.name(c)
                col_index = index_map[c]
                sheet.write(row_index, col_index, field_name, style)
                if c != 0:
                    sheet.write(row_index, col_index - 1, 'prev_' + field_name, style)
            for c in range(5):
                sheet.col(c).width = 256 * (80 if c == 0 else 100)
            sheet.row(row_index).height_mismatch = True
            sheet.row(row_index).height = row_height
            row_index += 1
            for row_data in sheet_row_list:
                for col_index, value in row_data.items():
                    sheet.write(row_index, col_index, value, style)
                sheet.row(row_index).height_mismatch = True
                sheet.row(row_index).height = row_height
                row_index += 1

    def save(self, file_path):
        if self.__book:
            if len(self.__book._Workbook__worksheets) == 0:
                self.__book.add_sheet('NONE')
            self.__book.save(file_path)

class TranslationCompareReport(object):
    def __init__(self):
        self.__book = None
        
    def generate(self, old, new, diff, field_index = FieldIndex.JAPANESE):
        index_map = {}
        index_map[FieldIndex.LABEL] = 0
        index_map[FieldIndex.CHINESE] = 1 * 2
        index_map[field_index] = 2 * 2
        
        book = self.__book = xlwt.Workbook(encoding='utf-8')
        style = get_default_cell_style()
        row_height = 2 * 256
        
        table_list = diff.tables
        def table_sort_cmp(a, b):
            return 1 if a.name > b.name else -1
        table_list.sort(cmp=table_sort_cmp)

        for d_table in table_list:
            n_table = new.get_table(d_table.name)
            o_table = old.get_table(d_table.name)
            sheet = book.add_sheet(d_table.name)
            row_index = 0
            for c in index_map.keys():
                field_name = FieldIndex.name(c)
                col_index = index_map[c]
                sheet.write(row_index, col_index, field_name, style)
                if c != 0:
                    sheet.write(row_index, col_index - 1, 'prev_' + field_name, style)
            for c in range(5):
                sheet.col(c).width = 256 * (80 if c == 0 else 100)
            sheet.row(row_index).height_mismatch = True
            sheet.row(row_index).height = row_height
            row_index += 1

            label_list = d_table.data.keys()
            label_list.sort()

            for label in label_list:
                for index in index_map.keys():
                    col_index = index_map[index]
                    if index == FieldIndex.LABEL:
                        sheet.write(row_index, col_index, label, style)
                    else:
                        n_value = n_table.data[label][index]
                        o_value = None
                        if o_table.data.has_key(label):
                            o_value = o_table.data[label][index]
                        sheet.write(row_index, col_index - 1, o_value, style)
                        sheet.write(row_index, col_index, n_value, style)
                sheet.row(row_index).height_mismatch = True
                sheet.row(row_index).height = row_height
                row_index += 1

    def save(self, file_path):
        if self.__book:
            if len(self.__book._Workbook__worksheets) == 0:
                self.__book.add_sheet('NONE')
            self.__book.save(file_path)
