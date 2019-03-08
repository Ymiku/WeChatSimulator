import os
ecode = os.system("python checker/check.py task")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py task TASK_CONF")

os.system("python xlsc_precompile.py")
