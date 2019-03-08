cd /d %~dp0
python checker/check.py UI_config
if %ERRORLEVEL% neq 0 exit

call xlsc.bat UI_config UI_CONFIG

python xlsc_precompile.py
pause