#!/usr/bin/env python
#encoding: utf-8

from checker.xlsdata.utils import is_svn_installed, os_encode, get_sheet_names
import xlrd, xml, os, sys, re
import subprocess
from lxml.etree import XMLParser
from lxml import etree

CONFIG_DIR = os.path.join('.', 'DataConfig')

def main():
    WORK_DIR = os.path.dirname(os.path.abspath(__file__))
    os.chdir(WORK_DIR)
    if not is_svn_installed():
        print os_encode('未安装svn命令行，该工具无法使用')
        return
    process = subprocess.Popen(['svn', 'status', CONFIG_DIR, '--xml'], stdout=subprocess.PIPE)
    xmlstr = process.communicate()[0]
    xmlstr = re.sub(r'<\?xml[^>]+\?>', '', xmlstr)
    root = etree.fromstring(xmlstr)
    pattern = re.compile(r'modified', re.IGNORECASE)
    export_list = []
    for item in root.xpath('//status/target/entry'):
        if pattern.search(item.xpath('./wc-status/@item')[0]) != None:
            xls_file = item.xpath('./@path')[0]
            xls_name = re.sub(r'\.xls[x]?$', '', os.path.basename(xls_file))
            ecode = os.system("python checker/check.py %s"%(xls_name))
            if ecode != 0:
                exit(ecode)
            for conf_name in get_sheet_names(xls_file):
                os.system('python xlsc.py %s %s'%(xls_name, conf_name))
            export_list.append(xls_file)
    if len(export_list) > 0:
        os.system("python xlsc_precompile.py")
        msg = "%d个配置表:\n%s\n已导出"%(len(export_list), '\n'.join(export_list))
        print os_encode(msg)
    else:
        msg = "未找到已修改的配置表"
        print os_encode(msg)

if __name__ == '__main__':
    main()