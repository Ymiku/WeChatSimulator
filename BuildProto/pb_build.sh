#!/usr/local/bin/bash
set -e

PROTO_PROJ_PATH='BuildProtoSolution'
PROTO2CS_TOOLS_PATH='step1_proto2cs'
TMP_OUTPUT_PATH='_output'

PROTO_DESC_SUFFIX='protodesc'
PROTO_PATH='Protocol'

function log_step()
{
	echo "#================================================="
	echo "# ${1}"
	echo "#================================================="
}

cd $(dirname ${0})
function abspath()
{
	python -c "import os;print os.path.abspath('$1')"
}

SCRIPT_PATH=${PWD}
PROJECT_PATH=$(abspath ${SCRIPT_PATH}/..)
ENUM_PROTO_PATH=$(abspath ${PROJECT_PATH}/BuildDataConfig/step1_xls2proto/)

function update_svn_dir()
{
	svn cleanup ${1}
	svn revert --depth infinity ${1}
	svn update --depth infinity ${1}
	svn log -l 5 --verbose ${1}
}

log_step "Update *.proto files"
update_svn_dir ${PROTO_PATH}
update_svn_dir ${PROJECT_PATH}/BuildProtoSolution/ProtobufProtocol
svn revert ${PROJECT_PATH}/Assets/Plugins/ProtobufProtocol*.dll
svn update ${PROJECT_PATH}/Assets/Plugins/ProtobufProtocol*.dll

log_step "1.Export proto to csharp messages"
rm -fr ${TMP_OUTPUT_PATH}

mkdir ${TMP_OUTPUT_PATH}
find ${PROTO_PATH} -iname '*.proto' | while read proto
do
	name=$(echo ${proto} | awk -F'/' '{print $NF}' | sed 's/\.proto$//')
	desc=${TMP_OUTPUT_PATH}/${name}.${PROTO_DESC_SUFFIX}
	
	${PROTO2CS_TOOLS_PATH}/protoc       \
		--descriptor_set_out=${desc}    \
		--proto_path=${PROTO_PATH}      \
		--proto_path=${ENUM_PROTO_PATH}	${proto}
	if [ $? -ne 0 ];then exit 1;fi
	
	mono ${PROTO2CS_TOOLS_PATH}/ProtoGen/protogen.exe -p:detectMissing \
		-i:${desc} -o:${TMP_OUTPUT_PATH}/${name}.cs
	if [ $? -ne 0 ];then exit 1;fi
done
echo

cd ..
log_step "2. Move *.cs file to ProtobufProtocol project"
mv -fv ${SCRIPT_PATH}/${TMP_OUTPUT_PATH}/*.cs ${PROTO_PROJ_PATH}/ProtobufProtocol/ProtoGen/

log_step "3. Build *.dll library files"
find ${PROTO_PROJ_PATH}/ProtobufProtocol -mindepth 1 -maxdepth 1 -iname '*.csproj' | while read csproj
do
	name=$(grep AssemblyName ${csproj} | awk -F'>' '{print $2}' | awk -F'<' '{print $1}')
	xbuild ${csproj} /property:Configuration=Release /toolsversion:3.5
	if [ $? -ne 0 ];then exit 1;fi
	
	output=$(dirname ${csproj})/bin/Release
	mkdir -pv ${output}
	
	mono ${SCRIPT_PATH}/Precompile/precompile.exe ${output}/${name}.dll \
		-t:${name}Serializer -o:${output}/${name}Serializer.dll
	if [ $? -ne 0 ];then exit 1;fi
	
	echo -e "\n# Move *.dll files to Unity project" 
	echo "-----------------------------------"
	if [ -f "${output}/${name}.dll" ]
	then
		cp -fv ${output}/${name}.dll ${output}/${name}Serializer.dll Assets/Plugins/
	fi
done

rm -fr ${SCRIPT_PATH}/${TMP_OUTPUT_PATH}

if [ -n "${WORKSPACE}" ]
then
	svn commit -m '更新协议脚本自动提交' \
		${SCRIPT_PATH}/../Assets/Plugins/ProtobufProtocol*.dll \
		${SCRIPT_PATH}/../BuildProtoSolution/ProtobufProtocol
fi

