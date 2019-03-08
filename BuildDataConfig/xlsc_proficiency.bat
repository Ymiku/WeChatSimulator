cd /d %~dp0
python checker/check.py proficiency
if %ERRORLEVEL% neq 0 exit

call xlsc.bat proficiency PROFICIENCY_CONF

python xlsc_precompile.py
pause