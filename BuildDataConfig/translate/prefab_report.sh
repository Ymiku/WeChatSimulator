#!/usr/bin/env bash

cd $(dirname ${0})

text=$(mktemp -u -t prefab_report.txt)
python prefab.py mail > ${text}

num=$(cat ${text} | wc -l | awk '{print $NF}')
let num=num-1
if [ ${num} -gt 0 ]
then
	mail=$(mktemp -u -t prefab_mail.txt)
	echo '<pre style="font-size:16px;line-height:1.6em;white-space:pre-wrap;font-family:Courier;">' > ${mail}
	echo "<div style='font-size:24px;color:red;'>本周新增<strong>${num}</strong>条UI中文</div>" >> ${mail}
	cat ${text} >> ${mail}
	echo '</pre>' >> ${mail}
	cat ${text}
	python ../checker/mail.py ${mail} 'NEXT<单周新增UI汉字>扫描'
	rm ${text} ${mail}
fi

svn commit -m '更新UI汉字基线库' database/prefab.txt