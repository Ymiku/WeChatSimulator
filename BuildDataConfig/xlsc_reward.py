import os
ecode = os.system("python checker/check.py reward")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py reward REWARD_CONF")

os.system("python xlsc_precompile.py")
