#!/usr/bin/env bash

cd $(dirname ${0})
set -xe

ROOT_DIR=${PWD}
cd ${ROOT_DIR}/step1_xls2proto
rm -f dataconfig_*.proto* *.txt *.log *_pb2.py*
cd ${ROOT_DIR}

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

cd step1_xls2proto
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
		python xls_deploy_tool.py ${name} ${xls} client false
		if [ $? -ne 0 ]
		then
			exit 1
		fi
	done
done

echo -e "\n# Generate csharp protobuf message files" 
echo "-----------------------------------------------------"
find . \( -iname 'dataconfig_*.proto' -o -iname 'xls_enum.proto' \) -maxdepth 1 | while read proto
do
	name=$(echo ${proto} | awk -F'/' '{print $NF}' | awk -F'.' '{print $1}')
	desc=${name}.protodesc
	
	${ROOT_DIR}/step2_proto2cs/protoc \
		--descriptor_set_out=${desc} ${proto}
	mono ${ROOT_DIR}/step2_proto2cs/ProtoGen/protogen.exe -i:${desc} -o:./${name}.cs
	echo
done

mv -fv dataconfig_*.cs xls_enum.cs ${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig/ProtoGen/
cd ${ROOT_DIR}/../BuildProtoSolution/ProtobufConfig

name=$(grep AssemblyName *.csproj | awk -F'>' '{print $2}' | awk -F'<' '{print $1}')
xbuild /property:Configuration=Release /toolsversion:3.5 /property:Platform=AnyCPU ProtobufConfig.csproj

output='bin/Release'
mkdir -pv ${output}

mono ${ROOT_DIR}/Precompile/precompile.exe ${output}/${name}.dll \
	-t:${name}Serializer -o:${output}/${name}Serializer.dll

PROJ_DIR=${PWD}

cd ${ROOT_DIR}/..
cp -fv ${PROJ_DIR}/${output}/${name}.dll ${PROJ_DIR}/${output}/${name}Serializer.dll Assets/Plugins/

find ${ROOT_DIR}/step1_xls2proto -iname '*.data' | while read data
do
	name=$(echo ${data} | awk -F'/' '{print $NF}' | sed 's/\.data$/.bytes/')
	mv -fv ${data} Assets/Resources/DataConfig/${name}
done

set +x
cd ${ROOT_DIR}/step1_xls2proto
rm -f dataconfig_*.proto* *.txt *.log *_pb2.py*