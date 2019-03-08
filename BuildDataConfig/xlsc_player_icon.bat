cd /d %~dp0
python checker/check.py player_icon
if %ERRORLEVEL% neq 0 exit

call xlsc.bat player_icon PLAYER_ICON_CONF

python xlsc_precompile.py
pause