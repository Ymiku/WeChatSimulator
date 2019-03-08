cd /d %~dp0
python checker/check.py
if %ERRORLEVEL% neq 0 exit

call xlsc.bat global_config GLOBAL_CONFIG
call xlsc.bat ability ABILITY
call xlsc.bat ability ABILITY_INPUT
call xlsc.bat arena_mode ARENA_MODE_CONF
call xlsc.bat arena_mode ARENA_MODE_TAG_CONF
call xlsc.bat arena_mode ARENA_MODE_MAPTYPE_CONF
call xlsc.bat arena_object ARENA_OBJECT_CONF
call xlsc.bat arena_type ARENA_TYPE_CONF
call xlsc.bat attack ATTACK_CONF
call xlsc.bat buff BUFF
call xlsc.bat buff BUFF_TAG
call xlsc.bat combo_ability COMBO_ABILITY
call xlsc.bat combo_ability ABILITY_DISPLAY
call xlsc.bat projectile PROJECTILE_CONF
call xlsc.bat projectile PROJECTILE_SHOOT_POSITION
call xlsc.bat skin SKIN_CONF
call xlsc.bat stat STAT_CONF
call xlsc.bat stat BASE_STATS
call xlsc.bat stat RESOURCE_CONF
call xlsc.bat item ITEM_CONF
call xlsc.bat item RECOMMEND_ITEM_CONF
call xlsc.bat attack_sound_tmp ATTACK_SOUND_CONF
call xlsc.bat UI_config UI_LOBBY
call xlsc.bat UI_config UI_ARENA
call xlsc.bat UI_config UI_COMMON
call xlsc.bat localization LOCALIZATION_CONF
call xlsc.bat signal SIGNAL_CONF
call xlsc.bat user_guide USER_GUIDE_CONF
call xlsc.bat arena_tips ARENA_TIPS

python xlsc_precompile.py
pause
