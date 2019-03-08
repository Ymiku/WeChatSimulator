cd /d %~dp0
python checker/check.py achievement
if %ERRORLEVEL% neq 0 exit

call xlsc.bat achievement ACHIEVEMENT_SERIES_CONF
call xlsc.bat achievement ACHIEVEMENT_LEVEL_CONF

python xlsc_precompile.py
pause