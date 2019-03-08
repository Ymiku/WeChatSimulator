import os
ecode = os.system("python checker/check.py ai_message")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py ai_message AI_MESSAGE")

os.system("python xlsc_precompile.py")