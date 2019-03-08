::智能导表工具: 自动对已修改的配置表导表
::需要安装svn命令行，否则无效
cd /d %~dp0
cd ..
svn revert --depth infinity .
svn update --depth infinity .
cd /d %~dp0

python translate\translate.py
python build_all.py
pause