import os
ecode = os.system("python checker/check.py attack")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py attack ATTACK_CONF")

os.system("python xlsc_precompile.py")