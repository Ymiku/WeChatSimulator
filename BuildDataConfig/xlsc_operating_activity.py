import os
ecode = os.system("python checker/check.py operating_activity")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py operating_activity OPERATING_ACTIVITY_CONF")
os.system("python xlsc.py operating_activity OPERATING_POPUP_CONF")

os.system("python xlsc_precompile.py")
