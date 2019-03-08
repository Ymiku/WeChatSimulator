import os
ecode = os.system("python checker/check.py level_up")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py level_up LEVEL_UP_CONF")

os.system("python xlsc_precompile.py")
