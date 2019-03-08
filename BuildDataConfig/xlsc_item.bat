cd /d %~dp0
python checker/check.py item
if %ERRORLEVEL% neq 0 exit

call xlsc.bat item ITEM_CONF
call xlsc.bat item RECOMMEND_ITEM_CONF

python xlsc_precompile.py
pause