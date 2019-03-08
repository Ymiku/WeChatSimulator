#!/usr/bin/env python
#encoding: utf-8

import xlrd
import re, os, sys, string, time
import Tkinter as tk
import tkMessageBox
import const_parse
import xlsdata.storage
from xlsdata.utils import is_winsystem, os_encode, supported_date_examples
from xlsdata.utils import parse_date, parse_local_date, parse_date_seconds

WORK_PATH=os.path.dirname(os.path.abspath(__file__))
ENUM_PATH=os.path.abspath(os.path.join(WORK_PATH, '..'))
sys.path.append(ENUM_PATH)
import step1_xls2proto.enum as xls_enum

INDEX_RULE_ROW = 0
INDEX_TYPE_ROW = 1
INDEX_NAME_ROW = 2
INDEX_MODE_ROW = 3
INDEX_DESC_ROW = 4
INDEX_DATA_ROW = 5

PRINT_INDENT = " " * 4

DRY_RUN_ERRORS = []
DRY_RUN = False

class BaseChecker(object):
    def __init__(self, xls_name, xls_root_path):
        """
        xls_name: 待检配置表文件名(不含后缀.xls)
        xls_root_path: 配置表根目录
        """
        self.file_map = {}
        self.name_map = {}
        self.storage = xlsdata.storage.manager

        self.status_types = const_parse.get_status_types()
        self.stat_types = const_parse.get_stat_types()

        self.set_root_path(xls_root_path)
        self.asset_root_path = os.path.abspath("%s/../../Assets/Resources"%(self.xls_root_path))
        self.art_asset_root_path = os.path.abspath("%s/../ArtResource"%(self.asset_root_path))
        self.name = xls_name

    def init(self):
        self.book = self.get_book(self.name)
        self.path = self.name_map[self.name]

    def dispose(self):
        for name in self.file_map.iterkeys():
            book = self.file_map[name]
            book.release_resources()
        self.file_map = None

    def __get_name__(self, file_path):
        """
        file_path: 文件路径
        return 文件名(不含后缀)
        """
        name = os.path.basename(file_path)
        return re.sub(r'\.[^.]+$','', name)

    def log(self, depth, msg):
        print os_encode("%s%s"%(PRINT_INDENT * depth, msg))

    def __assert__(self, errors):
        """
        弹出错误提示框
        errors: 错误信息行列表
        """
        global DRY_RUN_ERRORS
        global DRY_RUN
        DRY_RUN_ERRORS += errors
        # print '==> len:%d +%d'%(len(DRY_RUN_ERRORS), len(errors))
        if len(errors) > 0 and DRY_RUN == False:
            self.__warn__(errors)
            exit(1)

    def __warn__(self, errors):
        """
        弹出错误提示框
        errors: 错误信息行列表
        """
        global DRY_RUN
        if len(errors) > 0 and DRY_RUN == False:
            root = tk.Tk()
            root.withdraw()
            tkMessageBox.showerror('表单配置不符合规范', '\n'.join(errors))

    def set_root_path(self, xls_root_path):
        """
        设置配置表存储根目录
        xls_root_path: 配置表存储根目录
        """
        if os.path.exists(xls_root_path):
            self.xls_root_path = xls_root_path
        else:
            raise Exception("配置表(*.xls)根目录(%s)不存在"%(xls_root_path))

    def load_book(self, related_xls_path):
        """
        读取与待检配置表相关的xls配置表
        related_xls_path: 与待检配置表相关的xls配置表文件路径
        return [xlrd.book object]
        """
        try:
            book = xlrd.open_workbook(related_xls_path, encoding_override="cp1252")
            xls_name = self.__get_name__(related_xls_path)
            self.name_map[xls_name] = related_xls_path
            self.file_map[xls_name] = book
        except BaseException, e :
            print "读取xls文件(%s)失败!\n%s"%(related_xls_path, e)
            raise e

        return book

    def get_books(self, *xls_names):
        """
        批量加载配置表
        *xls_names: 与待检配置表相关的xls配置表文件路径列表
        return [[xlrd.book object]]
        """
        books = []
        for xls_name in xls_names:
            books.append(self.get_book(xls_name))
        return books
    
    def get_book(self, xls_name):
        """
        获取已加载配置表内存对象
        xls_name: 配置表名字，不包含路径和后缀名(.xls)
        return [xlrd.book object]
        """
        if self.name_map.has_key(xls_name):
            return self.file_map[xls_name]
        else:
            xls_file = "%s/%s.xls"%(self.xls_root_path, xls_name)
            if os.path.exists(xls_file):
                return self.load_book(xls_file)
            else:
                xls_file = "%s/%s.xlsx"%(self.xls_root_path, xls_name)
                if os.path.exists(xls_file):
                    return self.load_book(xls_file)
            return None

    def get_book_path(self, xls_name):
        return self.name_map[xls_name]

    def reload_book(self, xls_name):
        """
        重新载入配置表
        xls_name: 配置表名字，不包含路径和后缀名(.xls)
        return [xlrd.book object]
        """
        if self.name_map.has_key(xls_name):
            return self.load_book(self.name_map[xls_name])
        else:
            raise Exception("配置名(%s)不存在加载记录"%(xls_name))
            return None

    def get_column_indice(self, sheet, row_index, value):
        """
        获取配置表包含指定变量名的列索引
        sheet: 配置表(*.xls)子表[xlrd.sheet object]
        row_index: 搜索行索引
        value: 搜索值
        return [int]
        """
        col_indice = []
        for col_index in range(0, sheet.ncols):
            cell_value = sheet.cell_value(row_index, col_index)
            if cell_value == value:
                col_indice.append(col_index)
        return col_indice

    def column_to_alphabet(self, index):
        """
        把数字索引转换为字母索引，方便excel查找
        index: 表单列索引(从0开始)
        return [str object]
        """
        label = ""
        num = len(string.uppercase)
        if index >= num:
            label += string.uppercase[int(index / num) - 1]
            label += string.uppercase[index % num]
        else:
            label = string.uppercase[index]
        return label
    
    def parse_date(self, date_string):
        date = parse_local_date(date_string)
        if date == None:
            raise Exception('日期(%s)不规范，导表工具支持的日期格式%s'%(date_string.encode(encoding='utf-8'), supported_date_examples()))
            return None
        return date
    
    def parse_int(self, value):
        return int(float(unicode(value)))

    def parse_int_string(self, value):
        return re.sub(ur'\.0$', '', unicode(value))

    def parse_str_array(self, value):
        """
        把数组格式的字符串转换成数组
        value: 分号(;)间隔的字符串
        return [str]
        """
        result = []
        value = re.sub(ur'\n', '', value)
        array = re.split(ur';|；', value)
        for item in array:
            item = item.strip()
            if item != "":
                result.append(item)
        return result

    def parse_int_array(self, value):
        """
        把数组格式的字符串转换成整形数组
        value: 分号(;)间隔的字符串
        return [str]
        """
        array = self.parse_str_array(value)
        for i in range(len(array)):
            array[i] = unicode(int(float(array[i])))
        # print len(array), ','.join(array), value
        return array

    def dump_row_string(self, row, encoding="utf-8", sperator=" - "):
        """
        把表单行转换成指定编码字符串
        row: 通过sheet.row(row_index)方法获取的表单行数据
        encoding: 输出字符串编码，默认utf-8
        sperator: 数组元素连接符
        return string
        """
        cvt_array = []
        for i in range(len(row)):
            cell = row[i]
            cell_value = cell.value
            if cell.ctype == xlrd.XL_CELL_NUMBER:
                cell_value = re.sub(r'\.0$', "", unicode(cell_value))
            cvt_array.append("%s:%s"%(self.column_to_alphabet(i), unicode(cell_value).encode(encoding=encoding)))
        return (sperator.join(cvt_array)).replace("\n", "\\n")

    def array_to_string(self, array, encoding="utf-8", sperator=","):
        """
        unicode编码的数组转换成指定编码的字符串
        array: unicode编码的数组
        encoding: 输出字符串编码，默认utf-8
        sperator: 数组元素连接符
        return string
        """
        cvt_array = []
        for i in range(len(array)):
            cvt_array.append(array[i].encode(encoding=encoding))
        return sperator.join(cvt_array)

    def check_all(self):
        self.check_id()
        self.check_enum()

    def check_id(self):
        """
        对xls文件所有子表单id列进行重复性校验
        """
        errors = []
        for i in range(self.book.nsheets):
            sheet = self.book.sheet_by_index(i)
            columns = self.get_column_indice(sheet, INDEX_NAME_ROW, "id")
            if len(columns) != 1:
                continue
            self.log(1, "%s|check_id|ID重复校验"%(sheet.name.encode(encoding="utf-8")))
            column_index = columns[0]
            id_map = {}
            for r in range(INDEX_DATA_ROW, sheet.nrows):
                cell = sheet.cell(r, column_index)
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                id = re.sub(r'\.0$', '', unicode(cell.value).strip())
                if id_map.has_key(id) == False:
                    id_map[id] = []
                id_map[id].append(r)

            for id in id_map.iterkeys():
                row_list = id_map[id]
                if len(row_list) == 1 or id == "":
                    continue
                for r in row_list:
                    cell = sheet.cell(r, column_index)
                    msg = "%s#%s|%d行%s列|ID[%s]重复|%s"%(self.name, sheet.name.encode(encoding="utf-8"),r+1, \
                            self.column_to_alphabet(column_index),id.encode('utf-8'), self.dump_row_string(sheet.row(r)))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)

    def check_ref_id_exists(self, ref_xls_name, ref_sheet_name, sheet_name, field_names):
        """
        通用方法, 校验ID在引用表单中的有效性
        ref_xls_name: 引用配置表名
        ref_sheet_name: 引用配置表子表名
        sheet_name: 当前配置需要校验子表名
        field_names: 需要校验字段列表
        """
        if not (type(field_names) is list):
            raise Exception("字段[field_names]必须为数组，当前为:%s"%(type(field_names)))

        errors = []
        ref_map = {}
        ref_book = self.get_book(ref_xls_name)
        ref_sheet = ref_book.sheet_by_name(ref_sheet_name)
        ref_id_column_index = self.get_column_indice(ref_sheet, INDEX_NAME_ROW, "id")[0]
        for r in range(INDEX_DATA_ROW, ref_sheet.nrows):
            cell = ref_sheet.cell(r, ref_id_column_index)
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            id = unicode(int(float(cell.value)))
            ref_map[id] = ref_sheet.row(r)

        sheet = self.book.sheet_by_name(sheet_name)
        column_indice = []
        for name in field_names:
            column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, name)
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            for c in column_indice:
                field_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value).encode(encoding="utf-8")
                cell = sheet.cell(r, c)
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                array = self.parse_int_array(unicode(cell.value))
                subset = []
                for id in array:
                    if ref_map.has_key(id) == False:
                        subset.append(str(id))
                if len(subset) > 0:
                    msg = "%s#%s|%d行%s列|%s#%s表单不存在配置(%s=%s)"%(self.name, sheet_name ,r+1,self.column_to_alphabet(c), \
                            ref_xls_name, ref_sheet_name,field_name,','.join(subset))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)

    def check_asset_exist(self, sheet_name, field_names, extension, under_assets_root = False, custom_root_path = None):
        """
        通用方法, 校验配置表资源是否存在
        sheet_name : 配置表子表单名字
        field_names: 资源配置表字段名，数组格式
        extension: 资源后缀名
        under_assets_root: True表示字段资源路径是相对于Assets根目录的，否则是相对于Resources目录
        return errors
        """
        if not (type(field_names) is list):
            raise Exception("字段[field_names]必须为数组，当前为:%s"%(type(field_names)))
        
        if custom_root_path == None:
            root_path = self.asset_root_path
            if under_assets_root:
                root_path = os.path.dirname(root_path)
        else:
            root_path = custom_root_path

        errors = []
        sheet = self.book.sheet_by_name(sheet_name)
        for field_name in field_names:
            column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, field_name)[0]
            for r in range(INDEX_DATA_ROW, sheet.nrows):
                cell = sheet.cell(r, column_index)
                cell_value = cell.value
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                if cell.ctype == xlrd.XL_CELL_NUMBER:
                    cell_value = re.sub(r'\.0$', "", unicode(cell_value))
                else:
                    cell_value = unicode(cell_value)
                asset_location = "%s/%s.%s"%(root_path, cell_value.encode(encoding="utf-8"), extension)
                if os.path.exists(asset_location) == False:
                    msg = "%s#%s|%d行%s列|资源文件(%s=%s)不存在"%(self.name, sheet_name,r+1, self.column_to_alphabet(column_index),\
                            field_name, asset_location)
                    errors.append(msg)
                    self.log(2, msg)
        return errors

    def check_image_exists(self, sheet_name, field_name, image_root, is_art_res = False):
        """
        通用方法, 校验配置表资源是否存在
        sheet_name: 配置表子表单名字
        field_name: 资源配置表字段名，数组格式
        image_root: 图标文件所在目录，Resources/ArtResource下的相对目录
        is_art_res: True表示文件在ArtResource，反之在Resources目录
        return errors
        """
        root_path = self.asset_root_path
        if is_art_res:
            root_path = self.art_asset_root_path

        errors = []
        sheet = self.book.sheet_by_name(sheet_name)
        column_index = self.get_column_indice(sheet, INDEX_NAME_ROW, field_name)[0]
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            cell = sheet.cell(r, column_index)
            cell_value = cell.value
            if cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            if cell.ctype == xlrd.XL_CELL_NUMBER:
                cell_value = re.sub(r'\.0$', "", unicode(cell_value))
            else:
                cell_value = unicode(cell_value)
            icons = self.parse_str_array(cell_value)
            for id in icons:
                icon_file = "%s/%s.png"%(image_root, id.encode(encoding="utf-8"))
                if os.path.exists("%s/%s"%(root_path, icon_file)) == False:
                    msg = "%s#%s|%d行%s列|资源文件(%s=%s)不存在"%(self.name, sheet_name,r+1, self.column_to_alphabet(column_index),\
                            field_name, icon_file)
                    errors.append(msg)
                    self.log(2, msg)
        return errors

    def get_enabled_modes(self):
        """
        获取激活的地图模式列表
        return [unicode(id)]
        """
        mode_book = self.get_book("arena_mode")
        mode_sheet = mode_book.sheet_by_name("ARENA_MODE_CONF")
        mode_capacity_column_index = self.get_column_indice(mode_sheet, INDEX_NAME_ROW, "capacity")[0]
        mode_enable_column_index = self.get_column_indice(mode_sheet, INDEX_NAME_ROW, "isEnable")[0]
        mode_id_column_index = self.get_column_indice(mode_sheet, INDEX_NAME_ROW, "id")[0]
        modes = []
        for r in range(INDEX_DATA_ROW, mode_sheet.nrows):
            enable_cell = mode_sheet.cell(r, mode_enable_column_index)
            if enable_cell.ctype == xlrd.XL_CELL_EMPTY or enable_cell.value != 1.0:
                continue
            capacity_cell = mode_sheet.cell(r, mode_capacity_column_index)
            if capacity_cell.ctype == xlrd.XL_CELL_EMPTY or float(capacity_cell.value) != 10.0: 
                continue # 只选取激活的5v5模式
            id_cell = mode_sheet.cell(r, mode_id_column_index)
            if id_cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            modes.append(unicode(int(float(id_cell.value))))
        return modes

    def get_enabled_object_map(self, arena_mode, xls_name, sheet_name):
        """
        获取指定地图模式中上架的物品映射表
        arena_mode: 地图模式unicode(id)
        xls_name  : 配置表名字，不含后缀名(.xls)
        sheet_name: 配置表子表名字
        return {unicode(id):row}
        """
        item_map = {} # 构建地图上架物品索引表
        item_book = self.get_book(xls_name)
        item_sheet = item_book.sheet_by_name(sheet_name)
        field_maps_column_index = self.get_column_indice(item_sheet, INDEX_NAME_ROW, "maps")[0]
        field_id_column_index = self.get_column_indice(item_sheet, INDEX_NAME_ROW, "id")[0]
        for r in range(INDEX_DATA_ROW, item_sheet.nrows):
            maps_cell = item_sheet.cell(r, field_maps_column_index)
            if maps_cell.ctype == xlrd.XL_CELL_EMPTY:
                continue
            if (arena_mode in self.parse_int_array(unicode(maps_cell.value))) == False: # 跳过未上架皮肤
                continue
            id_cell = item_sheet.cell(r, field_id_column_index)
            id = unicode(int(float(id_cell.value)))
            item_map[id] = item_sheet.row(r)
        return item_map

    def check_stat(self, sheet_name, field_names):
        """
        配置表中stat属性合法校验
        sheet_name  : 配置表子表名字
        field_names : 字段列表，数组格式
        """
        if not (type(field_names) is list):
            raise Exception("字段[field_names]必须为数组，当前为:%s"%(type(field_names)))
        errors = [] 
        sheet = self.book.sheet_by_name(sheet_name)

        column_indice = []
        for field_name in field_names:
            column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, field_name)
        
        for r in range(INDEX_DATA_ROW, sheet.nrows):
            for c in column_indice:
                cell = sheet.cell(r, c)
                field_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value).encode(encoding='utf-8')
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                cell_value = unicode(cell.value).strip()
                if cell.ctype == xlrd.XL_CELL_NUMBER:
                    cell_value = re.sub(r'\.0$','', cell_value)
                items = self.parse_str_array(cell_value)
                subset = []
                for item in items:
                    if not (item in self.stat_types):
                        subset.append(item.encode(encoding='utf-8'))
                if len(subset) > 0:
                    msg = "%s#%s|%d行%s列|字段(%s=%s)在StatType.cs定义中不存在"%(self.name, sheet_name.encode(encoding='utf-8'),r+1,self.column_to_alphabet(c),field_name, ','.join(subset))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)

    def check_status(self, sheet_name, field_names):
        """
        配置表中status属性合法校验
        sheet_name  : 配置表子表名字
        field_names : 字段列表，数组格式
        """
        if not (type(field_names) is list):
            raise Exception("字段[field_names]必须为数组，当前为:%s"%(type(field_names)))
        errors = [] 
        sheet = self.book.sheet_by_name(sheet_name)

        column_indice = []
        for field_name in field_names:
            column_indice += self.get_column_indice(sheet, INDEX_NAME_ROW, field_name)

        for r in range(INDEX_DATA_ROW, sheet.nrows):
            for c in column_indice:
                cell = sheet.cell(r, c)
                field_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value).encode(encoding='utf-8')
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                cell_value = unicode(cell.value).strip()
                if cell_value.find('all') == 0:
                    continue
                if cell.ctype == xlrd.XL_CELL_NUMBER:
                    cell_value = re.sub(r'\.0$','', cell_value)
                items = self.parse_str_array(cell_value)
                subset = []
                for item in items:
                    if not (item in self.status_types):
                        subset.append(item.encode(encoding='utf-8'))
                if len(subset) > 0:
                    msg = "%s#%s|%d行%s列|字段(%s=%s)在StatusType.cs定义中不存在"%(self.name, sheet_name.encode(encoding='utf-8'),r+1,self.column_to_alphabet(c),field_name, ','.join(subset))
                    errors.append(msg)
                    self.log(2, msg)
        self.__assert__(errors)

    def check_enum(self):
        """
        表单中枚举enum配置合法校验
        """
        errors = [] 
        for sheet_name in self.book.sheet_names():
            sheet = self.book.sheet_by_name(sheet_name)
            if not unicode(sheet_name).isupper():
                continue
            chapter_done = False
            for c in range(sheet.ncols):
                field_cell = sheet.cell(INDEX_TYPE_ROW, c)
                if field_cell.ctype != xlrd.XL_CELL_TEXT:
                    continue
                if not xls_enum.match(field_cell.value):
                    continue
                if not chapter_done:
                    chapter_done = True
                    self.log(1, "%s#%s|check_enum|表单枚举字段校验"%(self.name, sheet_name.encode(encoding='utf-8')))
                field_rule = unicode(sheet.cell(INDEX_RULE_ROW, c).value)
                field_name = unicode(sheet.cell(INDEX_NAME_ROW, c).value)
                declared_type = field_cell.value
                type_name = xls_enum.parse_type_name(declared_type)
                try:
                    xls_enum.check_type(declared_type)
                except BaseException, e:
                    msg = '%s#%s|%d行%s列|字段(%s)类型(%s)未定义|%s'%(self.name, sheet_name.encode(encoding='utf-8'), INDEX_TYPE_ROW+1, self.column_to_alphabet(c),\
                                                            field_name.encode(encoding='utf-8'), type_name.encode(encoding='utf-8'), e)
                    errors.append(msg)
                    self.log(2, msg)
                    continue
                for r in range(INDEX_DATA_ROW, sheet.nrows):
                    input_cell = sheet.cell(r, c)
                    if input_cell.ctype == xlrd.XL_CELL_EMPTY:
                        continue
                    enum_name_list = []
                    if field_rule == 'repeated':
                        enum_name_list += self.parse_str_array(unicode(input_cell.value))
                    else:
                        enum_name_list.append(unicode(input_cell.value))
                    for enum_name in enum_name_list:
                        try:
                            xls_enum.check_name(declared_type, enum_name)
                        except BaseException, e:
                            msg = '%s#%s|%d行%s列|字段(%s=%s)在枚举中(%s)不存在|%s'%(self.name, sheet_name.encode(encoding='utf-8'), r+1, self.column_to_alphabet(c),\
                                                                field_name.encode(encoding='utf-8'), enum_name.encode(encoding='utf-8'), type_name.encode(encoding='utf-8'), e)
                            errors.append(msg)
                            self.log(2, msg)
        self.__assert__(errors)
