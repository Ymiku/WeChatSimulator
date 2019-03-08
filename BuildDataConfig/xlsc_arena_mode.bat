python checker/check.py arena_mode
if %ERRORLEVEL% neq 0 exit
call xlsc.bat arena_mode ARENA_MODE_CONF
call xlsc.bat arena_mode ARENA_MODE_TAG_CONF
call xlsc.bat arena_mode ARENA_MODE_MAPTYPE_CONF

python xlsc_precompile.py
pause