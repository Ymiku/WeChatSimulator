import os
ecode = os.system("python checker/check.py arena_mode")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py announceTips ANNOUNCE_TIPS")

os.system("python xlsc_precompile.py")