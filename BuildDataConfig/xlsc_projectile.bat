cd /d %~dp0
python checker/check.py projectile
if %ERRORLEVEL% neq 0 exit

call xlsc.bat projectile PROJECTILE_CONF
call xlsc.bat projectile PROJECTILE_SHOOT_POSITION
python xlsc_precompile.py
pause