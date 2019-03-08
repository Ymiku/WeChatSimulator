import os
ecode = os.system("python checker/check.py mall")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py mall MALL_CONF")
os.system("python xlsc.py mall MALL_ACHIEVING_CONF")
os.system("python xlsc.py mall PROMOTION_POSTERS")

os.system("python xlsc_precompile.py")
