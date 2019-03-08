import os
ecode = os.system("python checker/check.py rune")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py rune RUNE_CONF")
os.system("python xlsc.py rune RUNE_SLOT_CONF")
os.system("python xlsc.py rune RUNE_PAGE_CONF")
os.system("python xlsc.py rune RUNE_RECOMMEND)
os.system("python xlsc_precompile.py")
