cd /d %~dp0
python checker/check.py ability
if %ERRORLEVEL% neq 0 exit

call xlsc.bat ability ABILITY
call xlsc.bat ability ABILITY_INPUT
call xlsc.bat ability ABILITY_BREAK_TYPE

python xlsc_precompile.py
pause