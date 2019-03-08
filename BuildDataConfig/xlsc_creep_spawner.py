import os
ecode = os.system("python checker/check.py creep_spawner")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py creep_spawner CREEP_SPAWNER_CONF")
os.system("python xlsc.py creep_spawner CREEP_SPAWN_RULE_CONF")

os.system("python xlsc_precompile.py")
