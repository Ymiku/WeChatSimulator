#!/usr/bin/env python
#encoding: utf-8
import os, re, sys, json, xlrd, io, string
import os.path as p

SCAN_ROW_INDEX = 1
SCAN_COL_INDEX = 1
UNITY_RICH_TEXT_TAGS = ['size', 'color', 'b', 'i']

class HTMLValidator(object):
    def __init__(self, scan_row_index = SCAN_ROW_INDEX, scan_col_index = SCAN_COL_INDEX):
        self.__close_pattern = re.compile(r'^<\/([^>\/]+)>$')
        self.__start_pattern = re.compile(r'^<([^>\/]+)>$')
        self.__color_pattern = re.compile(r'^<color\s*=\s*["\']#[a-f0-9]{6,8}["\']\s*>$', re.IGNORECASE)
        self.error_reports = []
        self.scan_row_index = scan_row_index
        self.scan_col_index = scan_col_index

    def __index2abc(self, index):
        label = ""
        num = len(string.uppercase)
        if index >= num:
            label += string.uppercase[int(index / num) - 1]
            label += string.uppercase[index % num]
        else:
            label = string.uppercase[index]
        return label

    def __println(self, msg, depth = 0):
        msg = '%s%s'%(' ' * 4 * depth, msg)
        self.error_reports.append(msg)
        print msg

    def __is_tag(self, tag):
        tag = re.sub(r'[= \'\"#<>\/]+', '', tag)
        return re.match(r'^[0-9A-Za-z]+$', tag) != None
    
    def __process_cell_text(self, text):
        buffer = io.BytesIO()
        buffer.write(text)
        buffer.seek(0, os.SEEK_SET)
        tag, char, stack, is_found, element, snapshot, error_list = None, None, [], False, '','', []
        while char == None or char != '':
            char = buffer.read(1)
            if char == '<':
                is_found = True
                tag = ''
            if is_found:
                tag += char
            snapshot += char
            element += char
            if char == '>':
                if not is_found:continue
                is_found = False
                if not self.__is_tag(tag):continue
                tag_name = re.split(r'[= <>\/]+', tag)[1]
                if self.__start_pattern.match(tag):
                    if not tag_name.islower():
                        error_list.append('标签(%r)不全是小写'%(tag))
                    stack.append((tag, tag_name))
                if self.__close_pattern.match(tag):
                    if len(stack) == 0:
                        error_list.append('多余的标签("%s") text:"%s" position:%d snapshot:"%s"'%(tag, element, buffer.tell(), snapshot))
                    else:
                        if tag_name == stack[-1][1]:
                            del stack[-1]
                        else:
                            error_list.append('结束标签("%s")不匹配("%s") text:"%s" position:%d snapshot:"%s"'%(tag, stack[-1][0], element, buffer.tell(), snapshot))
                element = ''
        return error_list

    def __process_sheet(self, sheet, book_name):
        sheet_report = []
        for r in range(self.scan_row_index, sheet.nrows):
            column_report = []
            for c in range(self.scan_col_index, sheet.ncols):
                field_name = unicode(sheet.cell(0, c).value).encode('utf-8')
                cell = sheet.cell(r, c)
                if cell.ctype == xlrd.XL_CELL_EMPTY:
                    continue
                text = unicode(cell.value).encode('utf-8')
                position = '%d行:%s列'%(r+1, self.__index2abc(c))
                error_list = self.__process_cell_text(text)
                if len(error_list) > 0:
                    cell_report = ['[%s] %s | [%s]errors'%(position, field_name, len(error_list))]
                    for error in error_list:
                        cell_report.append(error)
                    column_report.append(cell_report)
            if len(column_report) > 0:
                sheet_report.append(column_report)
        if len(sheet_report) == 0:
            return
        self.__println('[%s] %s'%(book_name, sheet.name.encode('utf-8')))
        for column_report in sheet_report:
            for cell_report in column_report:
                for n in range(len(cell_report)):
                    self.__println(cell_report[n], 1 if n == 0 else 2)
        print

    def process(self, xls_path):
        self.error_reports = []
        xls_path = p.abspath(xls_path)
        book = xlrd.open_workbook(filename=xls_path)
        for sheet_name in book.sheet_names():
            sheet = book.sheet_by_name(sheet_name)
            self.__process_sheet(sheet, p.basename(xls_path))

def validate():
    os.chdir(p.dirname(p.abspath(__file__)))
    validator = HTMLValidator()
    validator.process(xls_path = 'database/library.xls')

def main():
    validate()

if __name__ == '__main__':
    main()