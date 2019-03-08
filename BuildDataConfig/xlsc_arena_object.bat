cd /d %~dp0
python checker/check.py arena_object
if %ERRORLEVEL% neq 0 exit

call xlsc.bat arena_object ARENA_OBJECT_CONF
call xlsc.bat arena_object BUILDING_LEVEL_CONF
call xlsc.bat arena_object BLOODPOT_SPAWNER
call xlsc.bat arena_object DAY_NIGHT_CONF
python xlsc_precompile.py
pause