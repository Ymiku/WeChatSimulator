import os
ecode = os.system("python checker/check.py item")
if ecode != 0:
    exit(ecode)
    
os.system("python xlsc.py item ITEM_CONF")
os.system("python xlsc.py item RECOMMEND_ITEM_CONF")

os.system("python xlsc_precompile.py")