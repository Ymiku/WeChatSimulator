cd /d %~dp0
python checker/check.py combo_ability
if %ERRORLEVEL% neq 0 exit

call xlsc.bat combo_ability COMBO_ABILITY
call xlsc.bat combo_ability ABILITY_DISPLAY

python xlsc_precompile.py
pause