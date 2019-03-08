cd /d %~dp0
python checker/check.py stat
if %ERRORLEVEL% neq 0 exit

call xlsc.bat stat STAT_CONF
call xlsc.bat stat BASE_STATS
call xlsc.bat stat RESOURCE_CONF
call xlsc.bat stat STATS_ADDITION_CONF

python xlsc_precompile.py
pause