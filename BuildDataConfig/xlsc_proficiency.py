import os
ecode = os.system("python checker/check.py proficiency")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py proficiency PROFICIENCY_CONF")
os.system("python xlsc_precompile.py")
