python checker/check.py buff
if %ERRORLEVEL% neq 0 exit

call xlsc.bat buff BUFF
call xlsc.bat buff BUFF_TAG
call xlsc.bat buff BUFF_PRIORITY
python xlsc_precompile.py
pause