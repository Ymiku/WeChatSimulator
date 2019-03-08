#!/usr/bin/env bash

cd $(dirname ${0})
set -xe

PROJECT_URL='http://tc-svn.tencent.com/ied/ied_narutoNext_rep/naruto_next_proj/trunk/TheNextMOBA'
PROJECT_DIR=$(python -c 'import os;print os.path.abspath("../..")')
function log_version()
{
	svn info ${PROJECT_DIR}
	svn log -l 2 --verbose ${PROJECT_DIR}
}

function log_chapter()
{
	echo "*****************************"
	echo "* ${1}"
	echo "*****************************"
}

REPORT_FILE='report.txt'
python check_dryrun.py | sed 's/</\&lt;/g'| sed 's/>/\&gt;/g' | tee ${REPORT_FILE}
num=$(cat ${REPORT_FILE} | grep '\[错误\]' | wc -l)
if [ ${num} -eq 0 ]
then
	rm -f *.txt
	exit
fi

sed -i '' 's/\(\[错误\]\)/<span style="color:red;font-weight:600;">\1<\/span>/g' ${REPORT_FILE}

MAIL_FILE='mail.txt'
rm -f ${MAIL_FILE} && touch ${MAIL_FILE}

function compose_mail()
{
	echo '<pre style="font-size:16px;line-height:1.6em;white-space:pre-wrap;font-family:SF Mono,Courier;">'
	echo "<div style='font-size:20px;color:red;'><strong>规范校验失败</strong></div>"
	cat ${REPORT_FILE} | grep '\[错误\]'

	echo '</br>'
	log_chapter '版本信息'
	log_version

	echo '</br>'
	log_chapter '配置表规范校验详情'
	cat ${REPORT_FILE}
	echo '</pre>'
}

compose_mail > ${MAIL_FILE}
python mail.py ${MAIL_FILE} 'NEXT<配置表规范检测>报告'

rm -f *.txt
exit 1 # 有错误发生