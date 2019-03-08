#!/usr/bin/env bash

cd $(dirname ${0})
# set -x
svn revert database/*.* commits/*.*
svn update database commits

function filter_commit_list()
{
    python <<-script
	import os, re
	from lxml import etree
	commit_list= []
	for node in etree.parse('${1}').xpath('//path'):
	    if re.match(r'^[AMR]$', node.xpath('@action')[0]):
	        split = node.xpath('text()')[0].split('/')
	        if split[-2] == u'commits':
	            commit_list.append(split[-1].encode('utf-8'))
	print 'commit_list:%r'%(commit_list)
	import os.path as p
	for file_name in os.listdir('commits'):
	    if file_name in commit_list:
	        continue
	    prev_file = p.abspath(p.join('commits', file_name))
	    os.remove(prev_file)
	    print 'rm %s'%(prev_file)
	script
}

TEMP_FILE=$(mktemp -t commits)
svn log -rPREV:HEAD --verbose commits --xml > ${TEMP_FILE}

cat ${TEMP_FILE} | xmllint --format -

filter_commit_list ${TEMP_FILE}
python merge.py

svn commit -m '自动翻译入库' database