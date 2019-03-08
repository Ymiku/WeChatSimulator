#!/usr/bin/env python
#encoding: utf-8
import os, re, sys, xlrd, string

# BeyondCompare 配置参数配置
# FileFormats - General => *.xlst
# FileFormats - Conversion - External program => /usr/local/bin/python [BASE_PATH]/xlscmp.py %s %t
# FileFormats - Type - Other => 勾选，并写入竖线(|)分隔符

def compact(text):
    text = re.sub(ur'\.0$', '', text)
    return text.encode('utf-8').replace('\n', '\\n').replace('\r', '\\r')

def index2abc(index):
    label = ""
    num = len(string.uppercase)
    if index >= num:
        label += string.uppercase[int(index / num) - 1]
        label += string.uppercase[index % num]
    else:
        label = string.uppercase[index]
    return label

def main():
    src_path = sys.argv[1]
    dst_path = sys.argv[2]
    book = xlrd.open_workbook(src_path, encoding_override="cp1252")
    output_file = open(dst_path, 'w')
    for sheet_name in book.sheet_names():
        sheet = book.sheet_by_name(sheet_name)
        for r in range(sheet.nrows):
            cell_list = sheet.row(r)
            text_list = [compact(unicode(x.value)) for x in cell_list]
            if r == 0:
                for n in range(len(text_list)):
                    text_list[n] = '%s-%s'%(index2abc(n), text_list[n])
            line_text = '%s-%s\n'%(sheet_name.encode('utf-8'), '|'.join(text_list))
            output_file.write(line_text)
    output_file.close()

if __name__ == '__main__':
    # log_file = open('/Users/larryhou/Downloads/cmp.txt', 'a')
    # log_file.write('%r\n'%(sys.argv))
    # log_file.close()
    main()