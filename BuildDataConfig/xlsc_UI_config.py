import os
ecode = os.system("python checker/check.py UI_config")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py UI_config UI_CONFIG")

os.system("python xlsc_precompile.py")
