cd /d %~dp0
python checker/check.py ai_Heros
if %ERRORLEVEL% neq 0 exit

call xlsc.bat ai_Heros AI_HERO

python xlsc_precompile.py
pause