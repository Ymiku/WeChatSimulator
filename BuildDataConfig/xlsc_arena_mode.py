import os
ecode = os.system("python checker/check.py arena_mode")
if ecode != 0:
    exit(ecode)
    
os.system("python xlsc.py arena_mode ARENA_MODE_CONF")
os.system("python xlsc.py arena_mode ARENA_MODE_TAG_CONF")
os.system("python xlsc.py arena_mode ARENA_MODE_MAPTYPE_CONF")

os.system("python xlsc_precompile.py")
