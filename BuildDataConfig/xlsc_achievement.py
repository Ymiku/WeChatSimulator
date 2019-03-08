import os
ecode = os.system("python checker/check.py achievement")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py achievement ACHIEVEMENT_SERIES_CONF")
os.system("python xlsc.py achievement ACHIEVEMENT_LEVEL_CONF")

os.system("python xlsc_precompile.py")
