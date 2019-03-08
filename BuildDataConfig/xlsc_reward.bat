cd /d %~dp0
python checker/check.py reward
if %ERRORLEVEL% neq 0 exit

call xlsc.bat reward REWARD_CONF

python xlsc_precompile.py
pause