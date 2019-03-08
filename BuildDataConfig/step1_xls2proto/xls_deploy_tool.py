#! /usr/bin/env python
#coding=utf-8

##
# @file:   xls_deploy_tool.py
# @author: jameyli <lgy AT live DOT com>
# @brief:  xls 配置导表工具

# 主要功能：
#     1 配置定义生成，根据excel 自动生成配置的PB定义
#     2 配置数据导入，将配置数据生成PB的序列化后的二进制数据或者文本数据
#
# 说明:
#   1 excel 的前四行用于结构定义, 其余则为数据，按第一行区分, 分别解释：
#       required 必有属性
#       optional 可选属性
#           第二行: 属性类型
#           第三行：属性名
#           第四行：注释
#           数据行：属性值
#       repeated 表明下一个属性是repeated,即数组
#           第二行: repeat的最大次数, excel中会重复列出该属性
#           2011-11-29 做了修改 第二行如果是类型定义的话，则表明该列是repeated
#           但是目前只支持整形
#           第三行：无用
#           第四行：注释
#           数据行：实际的重复次数
#       required_struct 必选结构属性
#       optional_struct 可选结构属性
#           第二行：结构元素个数
#           第三行：结构名
#           第四行：在上层结构中的属性名
#           数据行：不用填

#    1  | required/optional | repeated  | required_struct/optional_struct   |
#       | ------------------| ---------:| ---------------------------------:|
#    2  | 属性类型          |           | 结构元素个数                      |
#    3  | 属性名            |           | 结构类型名                        |
#    4  | 注释说明          |           | 在上层结构中的属性名              |
#    5  | 属性值            |           |                                   |

#
#
# 开始设计的很理想，希望配置定义和配置数据保持一致,使用同一个excel
# 不知道能否实现
#
# 功能基本实现，并验证过可以通过CPP解析 ohye
#
# 2011-06-17 修改:
#   表名sheet_name 使用大写
#   结构定义使用大写加下划线
# 2011-06-20 修改bug:
#   excel命名中存在空格
#   repeated_num = 0 时的情况
# 2011-11-24 添加功能
#   默认值
# 2011-11-29 添加功能
# repeated 第二行如果是类型定义的话，则表明该列是repeated
# 但是目前只支持整形

# TODO::
# 1 时间配置人性化
# 2 区分server/client 配置
# 3 repeated 优化
# 4 struct 优化

# 依赖:
# 1 protobuf
# 2 xlrd
##


import xlrd # for read excel
import sys
import os
import re
import tkMessageBox
import enum

DIALOG_MODE = True

# TAP的空格数
TAP_BLANK_NUM = 4

FIELD_RULE_ROW = 0
# 这一行还表示重复的最大个数，或结构体元素数
FIELD_TYPE_ROW = 1
FIELD_NAME_ROW = 2
FIELD_MODE_ROW = 3
FIELD_DESC_ROW = 4
FIELD_DATA_ROW = 5

MODE_TYPE_SERVER = ['s', 'svr', 'server', 'b', 'both']
MODE_TYPE_CLIENT = ['c', 'cli', 'client', 'b', 'both']

RULE_repeated = 'repeated'.decode(encoding='utf-8')
RULE_optional = 'optional'.decode(encoding='utf-8')
RULE_required = 'required'.decode(encoding='utf-8')
RULE_optional_struct = 'optional_struct'.decode(encoding='utf-8')
RULE_required_struct = 'required_struct'.decode(encoding='utf-8')

WORK_PATH=os.path.dirname(os.path.abspath(__file__))
UTIL_PATH=os.path.join(WORK_PATH, '..', 'checker', 'xlsdata')
if not UTIL_PATH in sys.path:
    sys.path.append(UTIL_PATH)
from utils import os_encode, parse_date_seconds, parse_duration_seconds

OUTPUT_FULE_PATH_BASE="dataconfig_"

PROTO_RAW_VALUE = "rawValue"

def alert(title, message):
    import Tkinter as tk
    root = tk.Tk()
    root.withdraw()
    tkMessageBox.showerror(title=title, message=message)

class LogHelp :
    """日志辅助类"""
    _logger = None
    _close_imme = True

    @staticmethod
    def set_close_flag(flag):
        LogHelp._close_imme = flag

    @staticmethod
    def _initlog():
        import logging

        LogHelp._logger = logging.getLogger()
        logfile = 'xls_deploy_tool.log'
        hdlr = logging.FileHandler(logfile)
        formatter = logging.Formatter('%(asctime)s|%(levelname)s|%(lineno)d|%(funcName)s|%(message)s')
        hdlr.setFormatter(formatter)
        LogHelp._logger.addHandler(hdlr)
        LogHelp._logger.setLevel(logging.NOTSET)
        # LogHelp._logger.setLevel(logging.WARNING)

        LogHelp._logger.info("\n\n\n")
        LogHelp._logger.info("logger is inited!")

    @staticmethod
    def get_logger() :
        if LogHelp._logger is None :
            LogHelp._initlog()

        return LogHelp._logger

    @staticmethod
    def close() :
        if LogHelp._close_imme:
            import logging
            if LogHelp._logger is None :
                return
            logging.shutdown()

# log macro
LOG_DEBUG=LogHelp.get_logger().debug
LOG_INFO=LogHelp.get_logger().info
LOG_WARN=LogHelp.get_logger().warn
LOG_ERROR=LogHelp.get_logger().error

class NextParser(object):
    def __init__(self, file_path, sheet_name):
        self._intent_mode = MODE_TYPE_CLIENT # 导表模式
        self._xls_file_path = file_path # xls文件路径
        self._sheet_name = sheet_name # 表单名
        try :
            self._workbook = xlrd.open_workbook(self._xls_file_path)
        except BaseException, e :
            print "open xls file(%s) failed!"%(self._xls_file_path)
            raise

        try :
            self._sheet = self._workbook.sheet_by_name(self._sheet_name)
        except BaseException, e :
            print "open sheet(%s) failed!"%(self._sheet_name)

    def index2abc(self, index):
        """
        把数字索引转换为字母索引，方便excel查找
        index: 表单列索引(从0开始)
        return [str object]
        """
        label = ""
        import string
        num = len(string.uppercase)
        if index >= num:
            label += string.uppercase[int(index / num) - 1]
            label += string.uppercase[index % num]
        else:
            label = string.uppercase[index]
        return label

    def check_field_mode(self, mode):
        # if self._intent_mode == MODE_TYPE_CLIENT:
        #     return True
        mode = mode.strip()
        return mode == '' or (mode.lower() in self._intent_mode)

    def caculate_size(self, field_column, repeat_num = 1):
        """
        计算指定列变量占用配置表列数量(包含变量本身)
        field_column : 变量所在列
        repeat_num   : 数组元素数量
        return (field_size, offset, field_indice, offset_indice)
        """
        field_rule = unicode(self._sheet.cell(FIELD_RULE_ROW, field_column).value).strip()
        field_name = unicode(self._sheet.cell(FIELD_NAME_ROW, field_column).value).strip()
        field_type = unicode(self._sheet.cell(FIELD_TYPE_ROW, field_column).value).strip()
        field_size = 0; offset = 0; field_indice = []; offset_indice = [];
        if field_rule in (RULE_optional, RULE_required):
            field_size += 1 * repeat_num
            for m in range(repeat_num):
                field_indice.append(field_column + m)
        elif field_rule == RULE_repeated:
            type_cell = self._sheet.cell(FIELD_TYPE_ROW, field_column)
            field_indice.append(field_column)
            if type_cell.ctype in (xlrd.XL_CELL_NUMBER, xlrd.XL_CELL_BOOLEAN):
                num = int(type_cell.value) 
                field_size += 1 # 结构体数组声明
                size = self.caculate_size(field_column + field_size, num)
                field_size += size[0]; offset += size[1]; field_indice += size[2]; offset_indice += size[3];
            else:
                field_size += 1 # 分号(;)间隔的数组
        elif field_rule in (RULE_optional_struct, RULE_required_struct):
            field_size += 1 # 结构体声明
            field_indice.append(field_column)
            field_num = int(self._sheet.cell(FIELD_TYPE_ROW, field_column).value) # 结构体字段数量
            child_stats = [];
            print "struct:%s repeat:%d"%(field_name.encode(encoding='utf-8'), repeat_num) # 调试日志，待导表稳定后清除
            for n in range(repeat_num):
                child_size = 0; child_indice = [];
                for i in range(field_num): # 计算单个struct大小
                    size = self.caculate_size(field_column + field_size + offset) # 支持嵌套结构体
                    field_size += size[0]; offset += size[1]; field_indice += size[2]; offset_indice += size[3];
                    child_size += size[0]; child_indice += size[2];
                child_stats.append(child_size)
                child_fields = [] 
                for c in child_indice:
                    child_fields.append(unicode(self._sheet.cell(FIELD_NAME_ROW, c).value))
                print child_fields # 调试日志，待导表稳定后清除
            for i in range(1, len(child_stats)):
                if child_stats[i - 1] != child_stats[i]:
                    raise Exception("结构体%s数组元素长度不一致(%s)"%(field_name.encode(encoding='utf-8'), child_stats))
        else:
            offset += 1
            offset_indice.append(field_column)
            size = self.caculate_size(field_column + 1) # 非定义字段(*)跳过
            field_size += size[0]; offset += size[1]; field_indice += size[2]; offset_indice += size[3];
        if len(field_indice) != field_size:
            raise Exception("结构体%s占用配置列计算错误(%d!=%d)"%(field_name.encode(encoding='utf-8'), field_size, len(field_indice)))
        return (field_size, offset, field_indice, offset_indice)

    def get_size(self, field_name):
        """
        计算指定变量名占用配置表列数量(包含变量本身)
        field_name: 变量名(unicode编码)
        return (int)column变量占用列数量
        """
        column = -1
        field_name = field_name.strip()
        for c in range(0, self._sheet.ncols):
            cell = self._sheet.cell(FIELD_NAME_ROW, c)
            if unicode(cell.value).strip() == field_name:
                column = c
                break
        if column >= 0:
            return self.caculate_size(column)[0]
        else:
            return 0

class SheetInterpreter(NextParser):
    """通过excel配置生成配置的protobuf定义文件"""
    def __init__(self, xls_file_path, sheet_name):
        super(SheetInterpreter, self).__init__(xls_file_path, sheet_name)

        # 行数和列数
        self._row_count = len(self._sheet.col_values(0))
        self._col_count = len(self._sheet.row_values(0))

        self._row = 0
        self._col = 0

        # 将所有的输出先写到一个list， 最后统一写到文件
        self._output = []
        # 排版缩进空格数
        self._indentation = 0
        # field number 结构嵌套时使用列表
        # 新增一个结构，行增一个元素，结构定义完成后弹出
        self._field_index_list = [1]
        # 当前行是否输出，避免相同结构重复定义
        self._is_layout = True
        # 保存所有结构的名字
        self._struct_name_list = []

        self._pb_file_name = OUTPUT_FULE_PATH_BASE + sheet_name.lower() + ".proto"


    def Interpreter(self) :
        """对外的接口"""
        LOG_INFO("begin Interpreter, row_count = %d, col_count = %d", self._row_count, self._col_count)

        self._LayoutFileHeader()

        self._output.append("package dataconfig;\n")
        self._output.append("import \"ProtoFScalar.proto\";\n")
        self._output.append("import \"xls_enum.proto\";\n")

        self._LayoutStructHead(self._sheet_name)
        self._IncreaseIndentation()

        while self._col < self._col_count :
            self._FieldDefine(0)

        self._DecreaseIndentation()
        self._LayoutStructTail()
        
        for line in self._output:
            print os_encode(line.replace('\n', '')) # 调试日志，打印proto协议，版本稳定后清除
        
        self._LayoutArray()

        self._Write2File()

        LogHelp.close()
        # 将PB转换成py格式
        try :
            command = "protoc --python_out=. " + self._pb_file_name
            os.system(command)
        except BaseException, e :
            print "protoc failed!"
            raise

    def _FieldDefine(self, repeated_num, is_struct = False) :
        LOG_INFO("row=%d, col=%d, repeated_num=%d", self._row, self._col, repeated_num)
        field_rule = unicode(self._sheet.cell_value(FIELD_RULE_ROW, self._col)).strip()
        field_mode = unicode(self._sheet.cell_value(FIELD_MODE_ROW, self._col)).strip()

        if field_rule in (RULE_optional, RULE_required):
            field_type = unicode(self._sheet.cell_value(FIELD_TYPE_ROW, self._col)).strip()
            field_name = unicode(self._sheet.cell_value(FIELD_NAME_ROW, self._col)).strip()
            field_comment = unicode(self._sheet.cell_value(FIELD_DESC_ROW, self._col))
            # print "%s|%s|%s|%s"%(field_rule, field_type, field_name, field_comment)
            if self.check_field_mode(field_mode):
                LOG_INFO("%s|%s|%s|%s", field_rule, field_type, field_name, field_comment)
                comment = field_comment.encode("utf-8")
                self._LayoutComment(comment)
                if repeated_num >= 1:
                    field_rule = RULE_repeated # 非结构体分列数组
                self._LayoutOneField(field_rule, field_type, field_name)
            actual_repeated_num = 1 if (repeated_num == 0) else repeated_num
            self._col += actual_repeated_num

        elif field_rule == RULE_repeated:
            # 2011-11-29 修改
            # 若repeated第二行是类型定义，则表示当前字段是repeated，并且数据在单列用分号相隔
            type_cell = self._sheet.cell(FIELD_TYPE_ROW, self._col)
            second_row = unicode(type_cell.value).strip()
            if self.check_field_mode(field_mode):
                LOG_DEBUG("repeated|%s", second_row);
                if type_cell.ctype in (xlrd.XL_CELL_NUMBER, xlrd.XL_CELL_BOOLEAN):
                    # 这里后面一般会是一个结构体
                    repeated_num = int(type_cell.value)
                    LOG_INFO("%s|%d", field_rule, repeated_num)
                    self._col += 1
                    self._FieldDefine(repeated_num)
                elif type_cell.ctype == xlrd.XL_CELL_TEXT :
                    # 一般是简单的单字段，数值用分号相隔
                    field_type = second_row
                    field_name = unicode(self._sheet.cell_value(FIELD_NAME_ROW, self._col)).strip()
                    field_comment = unicode(self._sheet.cell_value(FIELD_DESC_ROW, self._col))
                    LOG_INFO("%s|%s|%s|%s", field_rule, field_type, field_name, field_comment)
                    comment = field_comment.encode(encoding="utf-8")
                    self._LayoutComment(comment)
                    if enum.match(field_type): # 枚举类型转换
                        field_type = enum.parse_type_name(field_type)
                    self._LayoutOneField(field_rule, field_type, field_name)
                    self._col += 1
                else:
                    self._col += 1 # 未定义类型直接跳过
            else:
                self._col += 1
                if is_struct == True: # 如果是结构体定义，则必须找到一个有效定义，否则结构体定义可能不完整
                    self._FieldDefine(0, is_struct)

        elif field_rule in (RULE_optional_struct, RULE_required_struct):
            field_num = int(self._sheet.cell_value(FIELD_TYPE_ROW, self._col))
            
            field_name = unicode(self._sheet.cell_value(FIELD_NAME_ROW, self._col)).strip()
            struct_name = "InternalType_" + field_name;
            
            field_comment = unicode(self._sheet.cell_value(FIELD_DESC_ROW, self._col))
            comment = field_comment.encode("utf-8")

            struct_occupation = self.caculate_size(self._col - 1)[0]
            stop_position = self._col + (struct_occupation - 1)

            LOG_INFO("%s|%d|%s|%s", field_rule, field_num, struct_name, field_name)
            if self.check_field_mode(field_mode):
                if (self._IsStructDefined(struct_name)) :
                    self._is_layout = False
                else :
                    self._struct_name_list.append(struct_name)
                    self._is_layout = True
                self._col += 1
                self._StructDefine(struct_name, field_num, comment)
                self._is_layout = True
                if repeated_num >= 1:
                    field_rule = RULE_repeated
                elif field_rule == RULE_required_struct:
                    field_rule = RULE_required
                else:
                    field_rule = RULE_optional
                self._LayoutOneField(field_rule, struct_name, field_name)
            self._col = stop_position
        else:
            self._col += 1 # 直接跳过未定字段配置列
            return

    def _IsStructDefined(self, struct_name) :
        for name in self._struct_name_list :
            if name == struct_name :
                return True
        return False

    def _StructDefine(self, struct_name, field_num, comment) :
        """嵌套结构定义"""

        # self._col += 1
        self._LayoutComment(comment)
        self._LayoutStructHead(struct_name)
        self._IncreaseIndentation()
        self._field_index_list.append(1)

        while field_num > 0 :
            self._FieldDefine(0, is_struct = True)
            field_num -= 1

        self._field_index_list.pop()
        self._DecreaseIndentation()
        self._LayoutStructTail()

    def _LayoutFileHeader(self) :
        """生成PB文件的描述信息"""
        self._output.append("/**\n")
        self._output.append("* @file:   " + self._pb_file_name + "\n")
        self._output.append("* @author: jameyli <jameyli AT tencent DOT com>\n")
        self._output.append("* @brief:  这个文件是通过工具自动生成的，建议不要手动修改\n")
        self._output.append("*/\n")
        self._output.append("\n")


    def _LayoutStructHead(self, struct_name) :
        """生成结构头"""
        if not self._is_layout :
            return
        self._output.append("\n")
        self._output.append(" "*self._indentation + "message " + struct_name + "{\n")

    def _LayoutStructTail(self) :
        """生成结构尾"""
        if not self._is_layout :
            return
        self._output.append(" "*self._indentation + "}\n")
        self._output.append("\n")

    def _LayoutComment(self, comment) :
        # 改用C风格的注释，防止会有分行
        if not self._is_layout :
            return
        if comment.count("\n") > 1 :
            if comment[-1] != '\n':
                comment = comment + "\n"
                comment = comment.replace("\n", "\n" + " " * (self._indentation + TAP_BLANK_NUM),
                        comment.count("\n")-1 )
                self._output.append(" "*self._indentation + "/** " + comment + " "*self._indentation + "*/\n")
        else :
            self._output.append(" "*self._indentation + "/** " + comment + " */\n")

    def _LayoutOneField(self, field_rule, field_type, field_name) :
        """输出一行定义"""
        if not self._is_layout :
            return
        
        if field_name.find('=') > 0 :
            name_and_value = field_name.split('=')
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                    + " " + unicode(name_and_value[0]).strip() + " = " + self._GetAndAddFieldIndex()\
                    + " [default = " + unicode(name_and_value[1]).strip() + "]" + ";\n")
            return

        if (field_rule != "required" and field_rule != "optional") :
            if field_type == "double" or field_type == "float" :
                field_type = "ProtoFScalar"
                self._output.append(" "*self._indentation + field_rule + " " + field_type \
                        + " /*ProtoFScalar*/ " + field_name + " = " + self._GetAndAddFieldIndex()\
                        + ";\n")
            else:
                self._output.append(" "*self._indentation + field_rule + " " + field_type \
                        + " " + field_name + " = " + self._GetAndAddFieldIndex() + ";\n")
            return

        if field_type == "int32" or field_type == "int64" \
                or field_type == "uint32" or field_type == "uint64"\
                or field_type == "sint32" or field_type == "sint64"\
                or field_type == "fixed32" or field_type == "fixed64"\
                or field_type == "sfixed32" or field_type == "sfixed64" :
                    self._output.append(" "*self._indentation + field_rule + " " + field_type \
                            + " " + field_name + " = " + self._GetAndAddFieldIndex()\
                            + " [default = 0]" + ";\n")
        elif field_type == "bool":
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                            + " " + field_name + " = " + self._GetAndAddFieldIndex()\
                            + " [default = false]" + ";\n")
        elif field_type == "double" or field_type == "float" :
            field_type = "ProtoFScalar"
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                    + " /*ProtoFScalar*/ " + field_name + " = " + self._GetAndAddFieldIndex()\
                    + ";\n")
        elif field_type == "string" or field_type == "bytes" :
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                    + " " + field_name + " = " + self._GetAndAddFieldIndex()\
                    + " [default = \"\"]" + ";\n")
        elif field_type == "DateTime" :
            field_type = "uint64"
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                    + " /*DateTime*/ " + field_name + " = " + self._GetAndAddFieldIndex()\
                    + " [default = 0]" + ";\n")
        elif field_type == "TimeDuration" :
            field_type = "uint64"
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                    + " /*TimeDuration*/ " + field_name + " = " + self._GetAndAddFieldIndex()\
                    + " [default = 0]" + ";\n")
        elif enum.match(field_type): # 枚举类型处理
            enum.check_type(field_type)
            type_name = enum.parse_type_name(field_type)
            enum_default = enum.get_default(field_type)
            self._output.append(" "*self._indentation + field_rule + " " + type_name \
                    + " " + field_name + " = " + self._GetAndAddFieldIndex()\
                    + " [default = %s]"%(enum_default) + ";\n")
        else :
            self._output.append(" "*self._indentation + field_rule + " " + field_type \
                    + " " + field_name + " = " + self._GetAndAddFieldIndex() + ";\n")
        return

    def _IncreaseIndentation(self) :
        """增加缩进"""
        self._indentation += TAP_BLANK_NUM

    def _DecreaseIndentation(self) :
        """减少缩进"""
        self._indentation -= TAP_BLANK_NUM

    def _GetAndAddFieldIndex(self) :
        """获得字段的序号, 并将序号增加"""
        index = unicode(self._field_index_list[- 1])
        self._field_index_list[-1] += 1
        return index

    def _LayoutArray(self) :
        """输出数组定义"""
        self._output.append("message " + self._sheet_name + "_ARRAY {\n")
        self._output.append("    repeated " + self._sheet_name + " items = 1;\n}\n")

    def _Write2File(self) :
        """输出到文件"""
        pb_file = open(self._pb_file_name, "w+")
        pb_file.writelines(self._output)
        pb_file.close()


class DataParser(NextParser):
    """解析excel的数据"""
    def __init__(self, xls_file_path, sheet_name):
        super(DataParser, self).__init__(xls_file_path, sheet_name)

        self._positions = {}
        self._row_count = len(self._sheet.col_values(0))
        self._col_count = len(self._sheet.row_values(0))

        self._row = 0
        self._col = 0

        try:
            self._module_name = OUTPUT_FULE_PATH_BASE + self._sheet_name.lower() + "_pb2"
            sys.path.append(os.getcwd())
            exec('from '+self._module_name + ' import *');
            self._module = sys.modules[self._module_name]
        except BaseException, e :
            print "load module(%s) failed"%(self._module_name)
            raise

    def Parse(self) :
        """对外的接口:解析数据"""
        LOG_INFO("begin parse, row_count = %d, col_count = %d row = %d", self._row_count, self._col_count, self._row)

        item_array = getattr(self._module, self._sheet_name+'_ARRAY')()

        # 先找到定义ID的列
        id_col = 0
        for id_col in range(0, self._col_count) :
            info_id = unicode(self._sheet.cell_value(self._row, id_col)).strip()
            if info_id == "" :
                continue
            else :
                break

        for self._row in range(FIELD_DATA_ROW, self._row_count) :
            # 如果 id 是 空 直接跳过改行
            info_id = unicode(self._sheet.cell_value(self._row, id_col)).strip()
            if info_id == "" :
                LOG_WARN("%d is None", self._row)
                continue
            item = item_array.items.add()
            self._ParseLine(item)

        LOG_INFO("parse result:\n%s", item_array)

        self._WriteReadableData2File(unicode(item_array))

        data = item_array.SerializeToString()
        self._WriteData2File(data)


        #comment this line for test .by kevin at 2013年1月12日 17:23:35
        LogHelp.close()

    def _ParseLine(self, item) :
        LOG_INFO("%d", self._row)

        self._col = 0
        while self._col < self._col_count :
            self._ParseField(0, 0, item)

    def _ParseField(self, max_repeated_num, repeated_num, item, is_struct = False) :
        field_rule = unicode(self._sheet.cell_value(FIELD_RULE_ROW, self._col)).strip()
        field_type = unicode(self._sheet.cell_value(FIELD_TYPE_ROW, self._col)).strip()
        field_mode = unicode(self._sheet.cell_value(FIELD_MODE_ROW, self._col)).strip()

        if field_rule in (RULE_optional, RULE_required):
            field_name = unicode(self._sheet.cell_value(2, self._col)).strip()
            if self.check_field_mode(field_mode) == False:
                self._col += max(1, repeated_num)
                return # 停止读取数据
            if field_name.find('=') > 0 :
                name_and_value = field_name.split('=')
                field_name = unicode(name_and_value[0]).strip()

            LOG_INFO("%d|%d", self._row, self._col)
            LOG_INFO("%s|%s|%s", field_rule, field_type, field_name)

            if max_repeated_num == 0 :
                field_value = self._GetFieldValue(field_type, self._row, self._col)
                # 有value才设值
                if field_value != None and hasattr(item, field_name):
                    if field_type == "float" :
                        item.__getattribute__(field_name).__setattr__(PROTO_RAW_VALUE, field_value)
                    else :
                        item.__setattr__(field_name, field_value)
                self._col += 1
            else:
                if repeated_num == 0 :
                    if field_rule == "required" :
                        print "required but repeated_num = 0"
                        raise
                else :
                    for col in range(self._col, self._col + repeated_num):
                        field_value = self._GetFieldValue(field_type, self._row, col)
                        # 有value才设值
                        if field_value != None :
                            item.__getattribute__(field_name).append(field_value)
                self._col += max_repeated_num

        elif field_rule == RULE_repeated :
            # 2011-11-29 修改
            # 若repeated第二行是类型定义，则表示当前字段是repeated，并且数据在单列用分好相隔
            cell = self._sheet.cell(FIELD_TYPE_ROW, self._col)
            if self.check_field_mode(field_mode) == False:
                self._col += 1
                return # 停止读取数据
            second_row = unicode(cell.value)
            LOG_DEBUG("repeated|%s", second_row);
            if cell.ctype in (xlrd.XL_CELL_NUMBER, xlrd.XL_CELL_BOOLEAN) :
                # 这里后面一般会是一个结构体
                max_repeated_num = int(cell.value)
                read = self._sheet.cell_value(self._row, self._col)
                repeated_num = 0 if read == "" else int(self._sheet.cell_value(self._row, self._col))

                LOG_INFO("%s|%d|%d", field_rule, max_repeated_num, repeated_num)

                if max_repeated_num == 0 :
                    print "max repeated num shouldn't be 0"
                    raise

                if repeated_num > max_repeated_num :
                    repeated_num = max_repeated_num

                self._col += 1
                self._ParseField(max_repeated_num, repeated_num, item)
            else :
                # 一般是简单的单字段，数值用分号相隔
                # 一般也只能是数字类型
                field_type = second_row
                field_name = unicode(self._sheet.cell_value(FIELD_NAME_ROW, self._col)).strip()
                field_value_str = unicode(self._sheet.cell_value(self._row, self._col)).strip()

                #2013-01-24 jamey
                #增加长度判断
                if len(field_value_str) > 0:
                    field_value_str = re.sub(ur'\n', '', field_value_str)
                    field_value_list = re.split(ur';|；', field_value_str)

                    for field_value in field_value_list :
                        field_value = field_value.strip()
                        if field_value == "" :
                            continue
                        if field_type == "bytes" or field_type == "string" :
                            item.__getattribute__(field_name).append(field_value)
                        elif field_type == "bool":
                            item.__getattribute__(field_name).append(self.parse_bool(field_value))
                        elif enum.match(field_type):
                            enum_value = enum.get_value(field_type, field_value)
                            item.__getattribute__(field_name).append(enum_value)
                        else:
                            if(field_type == "float"):
                                temp_item = item.__getattribute__(field_name).add()
                                temp_item.__setattr__(PROTO_RAW_VALUE, self._FromFloat(float(field_value)))
                            else:
                                item.__getattribute__(field_name).append(int(float(field_value)))
                self._col += 1

        elif field_rule in (RULE_optional_struct, RULE_required_struct):
            field_num = int(self._sheet.cell_value(FIELD_TYPE_ROW, self._col))
            field_name = unicode(self._sheet.cell_value(FIELD_NAME_ROW, self._col)).strip()

            position = self._col - 1
            if not self._positions.has_key(position):
                struct_occupation = self.caculate_size(self._col - 1)[0]
                self._positions[position] = position + struct_occupation
            stop_position = self._positions[position]

            if self.check_field_mode(field_mode) == False:
                self._col = stop_position
                return # 停止读取数据
            
            struct_name = "InternalType_" + field_name;
            LOG_INFO("%s|%d|%s|%s", field_rule, field_num, struct_name, field_name)

            self._col += 1

            # 至少循环一次
            if max_repeated_num == 0 :
                struct_item = item.__getattribute__(field_name)
                self._ParseStruct(field_num, struct_item)

            else :
                if repeated_num == 0 :
                    if field_rule == "required_struct" :
                        print "required but repeated_num = 0"
                        raise
                    # 先读取再删除掉
                    struct_item = item.__getattribute__(field_name).add()
                    self._ParseStruct(field_num, struct_item)
                    item.__getattribute__(field_name).__delitem__(-1)
                else :
                    for num in range(0, repeated_num):
                        struct_item = item.__getattribute__(field_name).add()
                        self._ParseStruct(field_num, struct_item)

            self._col = stop_position

        else :
            self._col += 1 # 直接跳过未定配置列
            if is_struct == True: # 如果是结构体数据，则必须读到一个有效定义值，否则可能数据写入不完整
                self._ParseField(max_repeated_num, repeated_num, item, is_struct)
            return

    def _ParseStruct(self, field_num, struct_item) :
        """嵌套结构数据读取"""

        # 跳过结构体定义
        while field_num > 0 :
            self._ParseField(0, 0, struct_item, is_struct = True)
            field_num -= 1

    # float to FScalar
    def _FromFloat(self, oriValue) :
        if(oriValue > 2097151.9 or oriValue < -2097152) :
            return None
            # TODO

        if(oriValue >= 0) :
            return int(min(oriValue * 1024 + 0.5, 2147483647))
        else :
            return int(max(oriValue * 1024 - 0.5, -2147483648))
    
    def parse_bool(self, field_value):
        field_value = unicode(field_value).strip()
        if re.match(ur'^[0-9]*\.?[0-9]*$', field_value) == None:
            return field_value.lower() == 'true'
        else:
            return field_value != '' and int(float(field_value)) != 0

    def _GetFieldValue(self, field_type, row, col) :
        """将pb类型转换为python类型"""

        cell = self._sheet.cell(row, col)
        if cell.ctype == xlrd.XL_CELL_EMPTY:
            return None

        field_value = cell.value
        LOG_INFO("%d|%d|%s", row, col, field_value)

        try:
            if field_type == "int32" or field_type == "int64"\
                    or  field_type == "uint32" or field_type == "uint64"\
                    or field_type == "sint32" or field_type == "sint64"\
                    or field_type == "fixed32" or field_type == "fixed64"\
                    or field_type == "sfixed32" or field_type == "sfixed64" :
                        if len(unicode(field_value).strip()) <=0 :
                            return None
                        else :
                            return int(field_value)
            elif field_type == "bool":
                    return self.parse_bool(field_value)
            elif field_type == "double" or field_type == "float" or field_type == "ProtoFScalar" :
                    if len(unicode(field_value).strip()) <=0 :
                        return None
                    else :
                        return self._FromFloat(float(field_value))
            elif field_type == "string" :
                if cell.ctype in (xlrd.XL_CELL_NUMBER, xlrd.XL_CELL_BOOLEAN):
                    return re.sub(r'\.0$', '', unicode(field_value))
                if len(field_value) <= 0 :
                    return None
                else :
                    return field_value
            elif field_type == "bytes" :
                field_value = unicode(field_value)
                if len(field_value) <= 0 :
                    return None
                else :
                    return field_value
            elif field_type == "DateTime" :
                field_value = unicode(field_value)
                return parse_date_seconds(field_value)
            elif field_type == "TimeDuration" :
                field_value = unicode(field_value)
                return parse_duration_seconds(field_value)
            elif enum.match(field_type): # 枚举类型处理
                field_value = unicode(cell.value).strip()
                return enum.get_value(field_type, field_value)
            else :
                return None
        except BaseException, e :
            # print "====================================================================================="
            error_message = "[%r]单元格[%d行%s列=%r]数据解析失败: %s"%(sheet_name, row + 1, self.index2abc(col), cell.value, e)
            if DIALOG_MODE:
                alert(title='解析失败', message=error_message)
            print error_message
            sys.exit(1)

    def _WriteData2File(self, data) :
        file_name = OUTPUT_FULE_PATH_BASE + self._sheet_name.lower() + ".data"
        file = open(file_name, 'wb+')
        file.write(data)
        file.close()

    def _WriteReadableData2File(self, data) :
        file_name = OUTPUT_FULE_PATH_BASE + self._sheet_name.lower() + ".txt"
        file = open(file_name, 'wb+')
        file.write(data.encode('utf-8'))
        file.close()



if __name__ == '__main__' :
    """入口"""
    if len(sys.argv) < 3 :
        print "Usage: %s sheet_name(should be upper) xls_file" %(sys.argv[0])
        sys.exit(-1)

    # 导表(服务端/客户端)模式
    intent_mode = MODE_TYPE_CLIENT # 默认客户端模式
    if len(sys.argv) > 3 : 
        mode = sys.argv[3].strip().decode(encoding='utf-8').lower()
        if mode in MODE_TYPE_SERVER:
            intent_mode = MODE_TYPE_SERVER
        if len(sys.argv) > 4:
            DIALOG_MODE = sys.argv[4].lower() == 'true'

    sheet_name =  sys.argv[1]
    if (not sheet_name.isupper()):
        print "sheet_name should be upper"
        sys.exit(-2)

    xls_file_path =  sys.argv[2]

    tool = SheetInterpreter(xls_file_path, sheet_name)
    tool._intent_mode = intent_mode
    # try :
    tool.Interpreter()
    # except BaseException, e :
    #     print "Interpreter Failed!!!"
    #     print e
    #     exit(3)

    print "Interpreter Success!!!"
    
    parser = DataParser(xls_file_path, sheet_name)
    parser._intent_mode = tool._intent_mode
    # try :
    parser.Parse()
    # except BaseException, e :
    #     print "Parse Failed!!!"
    #     print e
    #     sys.exit(-4)

    print "Parse Success!!!"

