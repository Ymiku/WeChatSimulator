import os
ecode = os.system("python checker/check.py bag_item")
if ecode != 0:
    exit(ecode)
os.system("python xlsc.py bag_item BAG_ITEM_CONF")
os.system("python xlsc.py bag_item BAG_ITEM_TYPE_CONF")
os.system("python xlsc.py bag_item BAG_ITEM_TAG_CONF")
os.system("python xlsc.py bag_item ITEM_RARITY_CONF")

os.system("python xlsc_precompile.py")
