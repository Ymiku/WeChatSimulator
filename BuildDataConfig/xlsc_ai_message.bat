cd /d %~dp0
python checker/check.py ai_message
if %ERRORLEVEL% neq 0 exit

call xlsc.bat ai_message AI_MESSAGE

python xlsc_precompile.py
pause