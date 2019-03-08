#!/usr/bin/env python
#encoding: utf-8

from xlsdata.utils import os_encode
import base_check
import check
import sys
import os

base_check.DRY_RUN = True

if __name__ == '__main__':
    script_file = os.path.abspath(__file__)
    xls_root_path = os.path.abspath("%s/%s"%(os.path.dirname(script_file), "../DataConfig"))
    checker = check.ExcelChecker(xls_root_path)
    if len(sys.argv) == 1:
        checker.check_all()
    else:
        for i in range(1, len(sys.argv)):
            checker.check(sys.argv[i])
    
    print '=========[ SUMMERY ]========='
    if len(base_check.DRY_RUN_ERRORS) > 0:
        for error in base_check.DRY_RUN_ERRORS:
            print os_encode('[错误]%s'%(error)) 
        print os_encode('检表遇到错误')
        exit(1)
    else:
        print os_encode('检表无错通过')
        exit(0)
