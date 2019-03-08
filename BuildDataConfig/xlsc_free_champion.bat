cd /d %~dp0
python checker/check.py free_champion
if %ERRORLEVEL% neq 0 exit

call xlsc.bat free_champion FREE_CHAMPION_CONF
python xlsc_precompile.py
pause