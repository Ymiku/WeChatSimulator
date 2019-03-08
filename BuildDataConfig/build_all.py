#!/usr/bin/env python
#encoding: utf-8

from checker.xlsdata.utils import get_sheet_names
import os, re

CONFIG_DIR = os.path.join('.', 'DataConfig')

def main():
    WORK_DIR = os.path.dirname(os.path.abspath(__file__))
    os.chdir(WORK_DIR)
    
    pattern = re.compile(r'^[a-z0-9_]+\.xls[x]?$', re.IGNORECASE)
    for xls_file in os.listdir(CONFIG_DIR):
        if pattern.match(xls_file) == None:
            continue
        xls_file = os.path.join(CONFIG_DIR, xls_file)
        xls_name = re.sub(r'\.xls[x]?$', '', os.path.basename(xls_file))
        ecode = os.system("python checker/check.py %s"%(xls_name))
        if ecode != 0:
            exit(ecode)
        for conf_name in get_sheet_names(xls_file):
            os.system('python xlsc.py %s %s'%(xls_name, conf_name))
    os.system("python xlsc_precompile.py")

if __name__ == '__main__':
    main()