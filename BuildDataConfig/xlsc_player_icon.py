import os
ecode = os.system("python checker/check.py player_icon")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py player_icon PLAYER_ICON_CONF")

os.system("python xlsc_precompile.py")
