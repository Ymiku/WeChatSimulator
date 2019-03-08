cd /d %~dp0
call xlsc.bat arena_tips ARENA_TIPS

python xlsc_precompile.py
pause