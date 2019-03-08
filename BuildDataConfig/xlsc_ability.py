import os
ecode = os.system("python checker/check.py ability")
if ecode != 0:
    exit(ecode)
    
os.system("python xlsc.py ability ABILITY")
os.system("python xlsc.py ability ABILITY_INPUT")
os.system("python xlsc.py ability ABILITY_BREAK_TYPE")

os.system("python xlsc_precompile.py")
