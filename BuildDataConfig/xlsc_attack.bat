cd /d %~dp0
python checker/check.py attack
if %ERRORLEVEL% neq 0 exit

call xlsc.bat attack ATTACK_CONF

python xlsc_precompile.py
pause