#! /usr/bin/env python
#coding=utf-8
import os, re
from step1_xls2proto.split_sheet import SheetSpliter
ecode = os.system("python checker/check.py user_guide")
if ecode != 0:
    exit(ecode)

# 按照stage字段拆表
spliter = SheetSpliter('user_guide')
try:
    sheet_name = 'USER_GUIDE_CONF'
    segment_list = spliter.split(sheet_name, 'stage')
    for segment_path in segment_list:
        xls_name = os.path.basename(re.sub(r'\.[^\.]+$', '', segment_path))
        suffix = re.split(r'_', xls_name)[-1]
        os.system("python xlsc.py %s %s _%s"%(xls_name, sheet_name, suffix))
        os.remove(segment_path)
except BaseException, e:
    exit(1)
os.system("python xlsc.py user_guide USER_GUIDE_LAN_CONF")
os.system("python xlsc.py user_guide USER_GUIDE_STAGE_CONF")
os.system("python xlsc.py user_guide USER_GUIDE_TRIGGER_CONF")
os.system("python xlsc.py user_guide USER_GUIDE_ARENA_CONF")
os.system("python xlsc_precompile.py")
