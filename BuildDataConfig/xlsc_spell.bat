cd /d %~dp0
python checker/check.py spell
if %ERRORLEVEL% neq 0 exit

call xlsc.bat spell SPELL_CONF

python xlsc_precompile.py
pause