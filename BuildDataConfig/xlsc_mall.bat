cd /d %~dp0
python checker/check.py mall
if %ERRORLEVEL% neq 0 exit

call xlsc.bat mall MALL_CONF
call xlsc.bat mall MALL_ACHIEVING_CONF
call xlsc.bat mall PROMOTION_POSTERS

python xlsc_precompile.py
pause