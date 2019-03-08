import os
ecode = os.system("python checker/check.py projectile")
if ecode != 0:
    exit(ecode)

os.system("python xlsc.py projectile PROJECTILE_CONF")
os.system("python xlsc.py projectile PROJECTILE_SHOOT_POSITION")
os.system("python xlsc_precompile.py")