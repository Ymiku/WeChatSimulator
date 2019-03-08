cd /d %~dp0
python checker/check.py signal
if %ERRORLEVEL% neq 0 exit

call xlsc.bat signal SIGNAL_CONF

python xlsc_precompile.py
pause