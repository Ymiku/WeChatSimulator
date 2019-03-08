cd /d %~dp0
python checker/check.py skin
if %ERRORLEVEL% neq 0 exit
call xlsc.bat skin SKIN_CONF

python xlsc_precompile.py
pause