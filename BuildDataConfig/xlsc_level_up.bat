cd /d %~dp0
python checker/check.py level_up
if %ERRORLEVEL% neq 0 exit

call xlsc.bat level_up LEVEL_UP_CONF
python xlsc_precompile.py
pause