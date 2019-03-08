import os
ecode = os.system("python checker/check.py goods")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py goods GOODS_CONF")

os.system("python xlsc_precompile.py")
