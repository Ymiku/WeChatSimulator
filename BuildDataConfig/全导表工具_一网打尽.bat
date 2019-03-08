::自动识别所有可以导出的配置表以及子表
::不依赖其他导表工具，增加新配置表不需要手动配置，真正全部导表
cd /d %~dp0
python build_all.py
pause