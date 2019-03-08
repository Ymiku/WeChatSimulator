python checker/check.py announceTips
if %ERRORLEVEL% neq 0 exit
call xlsc.bat announceTips ANNOUNCE_TIPS

python xlsc_precompile.py
pause