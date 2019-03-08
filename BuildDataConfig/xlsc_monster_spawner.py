import os
ecode = os.system("python checker/check.py monster_spawner")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py monster_spawner MONSTER_SPAWNER_CONF")

os.system("python xlsc_precompile.py")
