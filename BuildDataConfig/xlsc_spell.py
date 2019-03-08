import os
ecode = os.system("python checker/check.py spell")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py spell SPELL_CONF")

os.system("python xlsc_precompile.py")