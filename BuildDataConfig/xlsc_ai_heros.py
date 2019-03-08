import os
ecode = os.system("python checker/check.py ai_Heros")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py ai_Heros AI_HERO")

os.system("python xlsc_precompile.py")