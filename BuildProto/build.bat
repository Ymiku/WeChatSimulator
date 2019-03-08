@echo off

set XLS_NAME=%1
set SHEET_NAME=%2
set DATA_DEST=%3



echo.
echo =========Compilation of %XLS_NAME%.xls=========



::---------------------------------------------------
::第一步：把proto翻译成cs
::---------------------------------------------------

set PROTO_PATH=.\Protocol
set ENUM_PROTO_PATH=..\BuildDataConfig\step1_xls2proto\
set STEP1_PROTO2CS_PATH=.\step1_proto2cs
set PROTO_DESC_POSTFIX=.protodesc
set SRC_OUT=..\src

cd %STEP1_PROTO2CS_PATH%

@echo off
echo TRY TO DELETE TEMP FILES:
del *.cs
del *.protodesc
del *.txt


@echo on
dir ..\%PROTO_PATH%\*.proto /b  > protolist.txt

@echo on
for /f "delims=." %%i in (protolist.txt) do protoc --descriptor_set_out=%%i%PROTO_DESC_POSTFIX% ^
													--proto_path=..\%PROTO_PATH% ^
													--proto_path=..\%ENUM_PROTO_PATH% ^
													..\%PROTO_PATH%\%%i.proto 
for /f "delims=." %%i in (protolist.txt) do ProtoGen\protogen -p:detectMissing -i:%%i%PROTO_DESC_POSTFIX% -o:%%i.cs 

cd..

::---------------------------------------------------
::第二步：将cs拷到Assets里
::---------------------------------------------------

@echo off
set CS_DEST=..\BuildProtoSolution\ProtobufProtocol\ProtoGen


@echo on
copy %STEP1_PROTO2CS_PATH%\*.cs %CS_DEST%

::---------------------------------------------------
::第三步：清除中间文件
::---------------------------------------------------
@echo off
echo TRY TO DELETE TEMP FILES:
cd %STEP1_PROTO2CS_PATH%
del *.cs
del *.protodesc
del *.txt
cd ..

python build_precompile.py

@echo on
pause