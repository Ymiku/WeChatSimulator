import os
ecode = os.system("python checker/check.py buff")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py buff BUFF")
os.system("python xlsc.py buff BUFF_TAG")
os.system("python xlsc.py buff BUFF_PRIORITY")

os.system("python xlsc_precompile.py")