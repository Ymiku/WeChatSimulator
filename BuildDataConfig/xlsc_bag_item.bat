cd /d %~dp0
python checker/check.py bag_item
if %ERRORLEVEL% neq 0 exit

call xlsc.bat bag_item BAG_ITEM_CONF
call xlsc.bat bag_item BAG_ITEM_TYPE_CONF
call xlsc.bat bag_item BAG_ITEM_TAG_CONF
call xlsc.bat bag_item ITEM_RARITY_CONF

python xlsc_precompile.py
pause