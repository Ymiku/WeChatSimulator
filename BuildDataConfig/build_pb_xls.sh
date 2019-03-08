#!/usr/bin/env bash

cd $(dirname ${0})
set -xe

ROOT_DIR=${PWD}
python SoundTool/attack_sound_export.py

function get_conf_name()
{
	python <<-SPT
	import sys, xlrd
	book = xlrd.open_workbook("${1}")
	for name in book.sheet_names():
	    if name.isupper():
	        print name.encode('utf-8')
	SPT
}

function update_svn_dir()
{
	svn cleanup ${1}
	svn revert --depth infinity ${1}
	svn update --depth infinity ${1}
}

update_svn_dir ${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig
update_svn_dir ${ROOT_DIR}/../Assets/Resources/DataConfig
update_svn_dir ${ROOT_DIR}/../Assets/Plugins

cd step1_xls2proto

echo -e "\n# Export config *.proto files from source xls" 
echo "-----------------------------------------------------"

chmod +x protoc

cp -fv pbsrc/ProtoFScalar.proto .
./protoc --python_out=. ProtoFScalar.proto
find ${ROOT_DIR}/DataConfig -iname '*.xls*' | while read xls
do
	get_conf_name ${xls} | while read name
	do
		if [ "${name}" = "" ]
		then
			continue
		fi
		echo -e '+++++++++++++++++++++++++++++++++++++++++++++++++'
		echo "Serializing ${xls}::${name}"
		echo
		python xls_deploy_tool.py ${name} ${xls}
		if [ $? -ne 0 ]
		then
			exit 1
		fi
		echo -e '----------\n'
	done
done

echo -e "\n# Generate csharp protobuf message files" 
echo "-----------------------------------------------------"
find . -iname 'dataconfig_*.proto' -maxdepth 1 | while read proto
do
	name=$(echo ${proto} | awk -F'/' '{print $NF}' | awk -F'.' '{print $1}')
	desc=${name}.protodesc
	
	# echo "#---------------------------------------------------"
	echo "${proto} -> ${name}.cs"
	echo
	${ROOT_DIR}/step2_proto2cs/protoc \
		--descriptor_set_out=${desc} ${proto}
	mono ${ROOT_DIR}/step2_proto2cs/ProtoGen/protogen.exe -i:${desc} -o:./${name}.cs
	echo
done

echo -e "\n# Build protobuf-serialized dll" 
echo "-----------------------------------------------------"
mv -fv dataconfig_*.cs ${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig/ProtoGen/
cd ${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig

name=$(grep AssemblyName *.csproj | awk -F'>' '{print $2}' | awk -F'<' '{print $1}')
xbuild /property:Configuration=Release /toolsversion:3.5

output='bin/Release'
mkdir -pv ${output}

mono ${ROOT_DIR}/Precompile/precompile.exe ${output}/${name}.dll \
	-t:${name}Serializer -o:${output}/${name}Serializer.dll

PROJ_DIR=${PWD}

cd ${ROOT_DIR}/..
echo -e "\n# Move *.dll files to Unity project" 
echo "-----------------------------------------------------"
cp -fv ${PROJ_DIR}/${output}/${name}.dll ${PROJ_DIR}/${output}/${name}Serializer.dll Assets/Plugins/

echo -e "\n# Move dataconfig_*_conf.bytes files to Unity project" 
echo "-----------------------------------------------------"

find ${ROOT_DIR}/step1_xls2proto -iname '*.data' | while read data
do
	name=$(echo ${data} | awk -F'/' '{print $NF}' | sed 's/\.data$/.bytes/')
	mv -fv ${data} Assets/Resources/DataConfig/${name}
done

cd ${ROOT_DIR}/step1_xls2proto
cp -f *.proto pbsrc
svn add --force pbsrc
svn commit -m '提交配置对应*.proto文件' pbsrc

rm -f dataconfig_*.proto* *.txt *.log *_pb2.py*

if [ -n "${WORKSPACE}" ]
then
	svn add --force \
		${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig \
		${ROOT_DIR}/../Assets/Resources/DataConfig
	svn commit -m '配置表序列化工具自动提交' \
		${ROOT_DIR}/../Assets/Plugins/ProtoBufConfig*.dll \
		${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig  \
		${ROOT_DIR}/../Assets/Resources/DataConfig
fi

