#!/usr/bin/env python
#encoding: utf-8

import sys, re, os
from google.protobuf.internal.enum_type_wrapper import EnumTypeWrapper

WORK_PATH=os.path.dirname(os.path.abspath(__file__))
UTIL_PATH=os.path.abspath(os.path.join(WORK_PATH, '..'))
sys.path.append(UTIL_PATH)

from checker.xlsdata.utils import os_encode, is_winsystem

class ProtoParser(object):
    def __init__(self, proto_file):
        proto_file = os.path.abspath(proto_file)
        self.work_dir = os.path.dirname(proto_file)
        self.module_name = None
        self.module_dict = {}
        self.proto_name = None
        command = 'protoc --proto_path=%s --python_out=%s %s'%(self.work_dir, self.work_dir, proto_file)
        if is_winsystem():
            command = '%s/%s'%(self.work_dir, command)
        if os.system(command) != 0:
            raise Exception('PROTO<%s>不存在，无法解析枚举类型'%(proto_file))
        else:
            self.proto_name = re.sub(r'\.proto$','', os.path.basename(proto_file))
            for name in os.listdir(self.work_dir):
                if re.match(r'^%s[^\.]+\.py$'%(self.proto_name), name):
                    self.module_name = re.sub(r'\.py$', '', name)
            exec('import %s'%(self.module_name))
            os.remove('%s/%s.py'%(self.work_dir, self.module_name))
            module = locals()[self.module_name]
            for key in module.__dict__.iterkeys():
                value = module.__dict__[key]
                if type(value) is EnumTypeWrapper:
                    self.module_dict[key] = value

    def get_type(self, type_name):
        if self.module_dict.has_key(type_name):
            return self.module_dict[type_name]
        else:
            return None

pattern = re.compile(r'^enum\.[a-z_][0-9a-z_]*$', re.IGNORECASE)
proto = ProtoParser('%s/xls_enum.proto'%(os.path.dirname(os.path.abspath(__file__))))

def check_type(declared_type):
    type_name = parse_type_name(declared_type)
    if not match(declared_type):
        if type_name == None:
            raise Exception('<%s>不是枚举声明类型'%(declared_type.encode(encoding='utf-8')))
            return False
    else:
        if proto.get_type(type_name) == None:
            raise Exception('Enum<%s>未定义，请在<xls_enum.proto>%s中添加相关枚举定义'%(type_name.encode(encoding='utf-8'), proto.module_dict.keys()))
            return False
    return True

def check_name(declared_type, name):
    name = unicode(name).strip()
    enum_type = get_type(declared_type)
    if not name in enum_type.keys():
        type_name = parse_type_name(declared_type)
        raise Exception('Enum<%s:%s>不存在枚举类型[%s]'%(type_name.encode(encoding='utf-8'), ','.join(enum_type.keys()), name.encode(encoding='utf-8')))
        return False
    return True

def match(declared_type):
    declared_type = unicode(declared_type).strip()
    return pattern.match(declared_type) != None

def get_default(declared_type):
    enum_type = get_type(declared_type)
    return enum_type.keys()[0]

def get_value(declared_type, name):
    # check_name(declared_type, name)
    enum_type = get_type(declared_type)
    return enum_type.Value(str(name).strip())

def get_type(declared_type):
    check_type(declared_type)
    type_name = parse_type_name(declared_type)
    return proto.get_type(type_name)

def parse_type_name(declared_type):
    declared_type = str(declared_type).strip()
    if pattern.match(declared_type) == None:
        return None
    return re.sub(r'^enum\.', '', declared_type)