cd /d %~dp0
python checker/check.py operating_activity
if %ERRORLEVEL% neq 0 exit

call xlsc.bat operating_activity OPERATING_ACTIVITY_CONF
call xlsc.bat operating_activity OPERATING_POPUP_CONF

python xlsc_precompile.py
pause