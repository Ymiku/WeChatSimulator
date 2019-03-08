#!/bin/bash
cd $(dirname ${0})

PROTO_PATH='step1_xls2proto/pbsrc'
BYTES_PATH='../Assets/Resources/DataConfig'

SHEET_NAME='SKIN_CONF'
while getopts :n:h OPTION
do
	case ${OPTION} in
		n) SHEET_NAME=${OPTARG};;
		h) echo "Usage: $(basename $0) -n [SHEET_NAME] -h [HELP]"
		   exit;;
		:) echo "ERR: -${OPTARG} 缺少参数, 详情参考: $(basename $0) -h" 1>&2
		   exit 1;;
		?) echo "ERR: 输入参数-${OPTARG}不支持, 详情参考: $(basename $0) -h" 1>&2
		   exit 1;;
	esac
done

BYTE_FILE=$(python -c "print 'dataconfig_%s.bytes'%('${SHEET_NAME}'.lower())")
SHEET_NAME=$(python -c "print '${SHEET_NAME}'.upper()") 

printf "$(protoc --proto_path=${PROTO_PATH} --decode=dataconfig.${SHEET_NAME}_ARRAY ${PROTO_PATH}/*.proto < ${BYTES_PATH}/${BYTE_FILE} 2>/dev/null | sed 's/%/%%/g')\n"