import os
ecode = os.system("python checker/check.py combo_ability")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py combo_ability COMBO_ABILITY")

os.system("python xlsc_precompile.py")