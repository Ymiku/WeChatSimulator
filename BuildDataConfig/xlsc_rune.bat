cd /d %~dp0
python checker/check.py rune
if %ERRORLEVEL% neq 0 exit

call xlsc.bat rune RUNE_CONF
call xlsc.bat rune RUNE_SLOT_CONF
call xlsc.bat rune RUNE_PAGE_CONF
call xlsc.bat rune RUNE_RECOMMEND

python xlsc_precompile.py
pause