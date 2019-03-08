import os
ecode = os.system("python checker/check.py arena_object")
if ecode != 0:
    exit(ecode)
    
os.system("python xlsc.py arena_object ARENA_OBJECT_CONF")
os.system("python xlsc.py arena_object BUILDING_LEVEL_CONF")
os.system("python xlsc.py arena_object DAY_NIGHT_CONF")

os.system("python xlsc_precompile.py")
