#!/usr/bin/env python
#encoding: utf-8

import sys, re, os
from utils import *
from utils import os_encode

class ProtoParser(object):
    def __init__(self, proto_file):
        proto_file = os.path.abspath(proto_file)
        self.work_dir = os.path.dirname(proto_file)
        self.module_name = None
        self.module_dict = {}
        self.proto_name = None
        command = 'protoc --proto_path=%s --python_out=%s %s'%(self.work_dir, self.work_dir, proto_file)
        if is_winsystem():
            command = '%s/%s'%(os.path.dirname(os.path.abspath(__file__)), command)
        if os.system(command) != 0:
            raise Exception(os_encode('PROTO<%s>不存在，无法解析枚举类型'%(proto_file)))
        else:
            self.proto_name = re.sub(r'\.proto$','', os.path.basename(proto_file))
            for name in os.listdir(self.work_dir):
                if re.match(r'^%s[^\.]+\.py$'%(self.proto_name), name):
                    self.module_name = re.sub(r'\.py$', '', name)
            exec('import %s'%(self.module_name))
            module = locals()[self.module_name]
            for key in module.__dict__.iterkeys():
                value = module.__dict__[key]
                if re.search(r'^google\.protobuf', type(value).__module__) != None:
                    self.module_dict[key] = value
        module_file = os.path.join(self.work_dir, self.module_name)
        if is_winsystem():
            os.system("del /F /Q %s.*"%(module_file))
        else:
            os.system("rm -f %s.*"%(module_file))
        
    def get_type(self, type_name):
        if self.module_dict.has_key(type_name):
            return self.module_dict[type_name]
        else:
            return None

class StorageTable(object):
    def __init__(self, name, table):
        self.name = name
        self.location = os.path.join(os.path.dirname(os.path.abspath(__file__)), "data", "%s.data"%(self.name))
        self.table = table
        self.update()
        self.float_format = ".6f"
        if os.path.exists(self.location):
            file = open(self.location, 'rb')
            data = file.read()
            file.close()
            self.table.ParseFromString(data)
        self.map = {}
        for item in table.list:
            self.set(item)

    def new(self):
        return self.table.list.add()

    def get(self, id):
        return self.map[id]

    def set(self, item):
        if not self.map.has_key(item.id):
            self.map[item.id] = item
        else:
            from google.protobuf import text_format
            a = text_format.MessageToString(self.map[item.id], as_utf8 = True, as_one_line = True, float_format = self.float_format)
            b = text_format.MessageToString(item, as_utf8 = True, as_one_line = True, float_format = self.float_format)
            raise Exception(os_encode("表ID(%d)重复|%s|%s"%(item.id, a, b)))
    
    def has(self, id):
        return self.map.has_key(id)

    def dump(self, compact = False):
        # as_utf8=False, as_one_line=False, pointy_brackets=False, use_index_order=False, float_format=None
        from google.protobuf import text_format
        return text_format.MessageToString(self.table, as_one_line = compact, as_utf8 = True, use_index_order = True, float_format = self.float_format)

    def save(self):
        data = self.table.SerializeToString()
        file = open(self.location, 'wb')
        file.write(data)
        file.close()

    def run_inside_bash(self, command):
        return "bash -xc '%s'"%(command)

    def update(self):
        if not is_svn_installed():
            return
        svn_command = "svn info %s"%(self.location)
        if not is_winsystem():
            svn_command = self.run_inside_bash(svn_command)
        if os.system(svn_command) != 0:
            return # 未加入版本管理的文件不更新
        svn_command = "svn update --force %s"%(self.location)
        if not is_winsystem():
            svn_command = self.run_inside_bash(svn_command)
        if os.system(svn_command) != 0:
            raise Exception(os_encode("<%s>更新失败"%(self.location)))

    def commit(self):
        self.save()
        if not is_svn_installed():
            return
        svn_command = "svn add --force %s"%(self.location)
        if not is_winsystem():
            svn_command = self.run_inside_bash(svn_command)
        os.system(svn_command)

        svn_command = "svn commit -m \"工具自动提交提:%s.data\" %s"%(self.name, self.location)
        if not is_winsystem():
            svn_command = self.run_inside_bash(svn_command)
        else:
            svn_command = svn_command.decode(encoding="utf-8").encode(encoding="gbk")
        if os.system(svn_command):
            raise Exception(os_encode("<%s>提交失败"%(self.location)))

class StorageManager(object):
    def __init__(self):
        self.root_dir = os.path.dirname(os.path.abspath(__file__))
        self.parser = ProtoParser(os.path.join(self.root_dir, 'data.proto'))
        self.tables = {}

    def get(self, name):
        name = str(name).strip()
        if self.tables.has_key(name):
            table = self.tables[name]
        else:
            table = StorageTable(name, self.parser.get_type(name)())
            self.tables[table.name] = table
        return table

    def dump(self):
        for name in self.tables.keys():
            table = self.tables[name]
            print "[%s]"%(name)
            print table.dump()

    def save(self):
        for name in self.tables.keys():
            table = self.tables[name]
            table.save()

    def update(self):
        for name in self.tables.keys():
            table = self.tables[name]
            table.update()

    def commit(self):
        for name in self.tables.keys():
            table = self.tables[name]
            table.commit()

# 存储管理器
manager = StorageManager()

    
        
