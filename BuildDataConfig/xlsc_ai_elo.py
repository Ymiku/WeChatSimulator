import os
ecode = os.system("python checker/check.py ai_elo")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py ai_elo AI_ELO")

os.system("python xlsc_precompile.py")