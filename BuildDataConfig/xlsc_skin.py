import os
ecode = os.system("python checker/check.py skin")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py skin SKIN_CONF")

os.system("python xlsc_precompile.py")