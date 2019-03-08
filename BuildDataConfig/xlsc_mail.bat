cd /d %~dp0
python checker/check.py mall
if %ERRORLEVEL% neq 0 exit

call xlsc.bat mail MAIL_CONF

python xlsc_precompile.py
pause