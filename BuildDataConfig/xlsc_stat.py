import os
ecode = os.system("python checker/check.py stat")
if ecode != 0:
    exit(ecode)
    
os.system("python xlsc.py stat STAT_CONF")
os.system("python xlsc.py stat BASE_STATS")
os.system("python xlsc.py stat RESOURCE_CONF")
os.system("python xlsc.py stat STATS_ADDITION_CONF")

os.system("python xlsc_precompile.py")