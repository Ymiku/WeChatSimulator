cd /d %~dp0
python checker/check.py localization
if %ERRORLEVEL% neq 0 exit

call xlsc.bat localization LOCALIZATION_CONF

python xlsc_precompile.py
pause