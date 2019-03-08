cd /d %~dp0
python checker/check.py goods
if %ERRORLEVEL% neq 0 exit

call xlsc.bat goods GOODS_CONF

python xlsc_precompile.py
pause