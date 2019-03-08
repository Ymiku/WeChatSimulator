cd /d %~dp0
python checker/check.py task
if %ERRORLEVEL% neq 0 exit

call xlsc.bat task TASK_CONF

python xlsc_precompile.py
pause