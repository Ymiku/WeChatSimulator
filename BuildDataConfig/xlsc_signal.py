import os
ecode = os.system("python checker/check.py signal")
if ecode != 0:
    exit(ecode)
    
os.system("python xlsc.py signal SIGNAL_CONF")

os.system("python xlsc_precompile.py")