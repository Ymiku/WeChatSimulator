import os
ecode = os.system("python checker/check.py free_champion")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py free_champion FREE_CHAMPION_CONF")

os.system("python xlsc_precompile.py")
