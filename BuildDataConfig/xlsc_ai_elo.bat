cd /d %~dp0
python checker/check.py ai_elo
if %ERRORLEVEL% neq 0 exit

call xlsc.bat ai_elo AI_ELO

python xlsc_precompile.py
pause