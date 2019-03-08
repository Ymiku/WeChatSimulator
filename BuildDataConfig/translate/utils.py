#!/usr/bin/env python
#encoding: utf-8
import os, re, sys, xlrd, xlwt, xlutils, md5
from __init__ import DATA_PATH, PATTERN_XLS_SUFFIX
from __init__ import FieldIndex, HeaderIndex, TranslationDatabase, TranslationTable, DatabaseType

WORK_DIR = os.path.dirname(os.path.abspath(__file__))

def load_book(xls_file_path):
    return xlrd.open_workbook(xls_file_path, encoding_override="cp1252")

class TranslationSpliter(object):
    def __init__(self):
        pass

    def __compact(self, text):
        return text.replace('\n', '\\n').replace('\r', '\\r')

    def split(self, xls_file_path):
        book = load_book(xls_file_path)
        output_path = re.sub(r'\.xls[x]?$', '', xls_file_path)
        if os.path.exists(output_path):
            import shutil
            shutil.rmtree(output_path)
        os.makedirs(output_path)
        for sheet_name in book.sheet_names():
            sheet_text_file = open(os.path.join(output_path, sheet_name + '.xlst'), 'w')
            sheet = book.sheet_by_name(sheet_name)
            for r in range(sheet.nrows):
                text_list = [self.__compact(unicode(x.value).encode('utf-8')) for x in sheet.row(r)]
                sheet_text_file.write('|'.join(text_list) + '\n')
            sheet_text_file.close()

class TranslationUploader(object):
    def __init__(self):
        self.commits_dir = os.path.join(WORK_DIR, 'commits')
        self.merge = TranslationDatabase(type = DatabaseType.MERGE)
        self.merge.clear()

    def run(self):
        for file_name in os.listdir(self.commits_dir):
            if not PATTERN_XLS_SUFFIX.search(file_name) or re.search(r'^~', file_name):
                continue
            print file_name, re.search(r'^~', file_name)
            book = load_book(os.path.join(self.commits_dir, file_name))
            for sheet_name in book.sheet_names():
                if not sheet_name.isupper():
                    continue
                # print file_name, sheet_name
                sheet = book.sheet_by_name(sheet_name)
                self.merge.add_table(sheet_name, sheet)
        self.merge.save()

class JapaneseTranslate(object):
    def __init__(self):
        self.collect_book_path = os.path.join(WORK_DIR, 'database/collect.xls')

    def dump(self):
        book = load_book(self.collect_book_path)
        sheet_name_list = book.sheet_names()
        sheet_name_list.sort()

        newline = re.compile(r'(\r|\n|\r\n)', re.MULTILINE)
        color = re.compile(r'<(/|#)?color(="?#[0-9a-f]{6,8}\s*"?)?>', re.IGNORECASE)
        size = re.compile(r'</?size(=\d+)?>', re.IGNORECASE)
        dump_file = open(os.path.join(WORK_DIR, 'cn_content.txt'), 'w')
        for sheet_name in sheet_name_list:
            print sheet_name
            dump_file.write('<-SHEET->%s\n'%(sheet_name.encode('utf-8')))
            sheet = book.sheet_by_name(sheet_name)
            column_index = -1
            for c in range(sheet.ncols):
                field_name = unicode(sheet.cell(HeaderIndex.NAME, c).value).strip().lower()
                if field_name == FieldIndex.name(FieldIndex.CHINESE).lower():
                    column_index = c
                    break
            if column_index == -1:
                continue
            for r in range(HeaderIndex.DATA, sheet.nrows):
                text = newline.sub(r'\\n', unicode(sheet.cell(r, c).value).strip())
                text = size.sub('', color.sub('', text))
                dump_file.write('%s\n'%(text.encode('utf-8')))
        dump_file.flush()
        dump_file.close()

    def repair(self):
        merge = TranslationDatabase(type = DatabaseType.MERGE)
        chapter = re.compile(r'^<-SHEET->\s*', re.IGNORECASE)
        newline = re.compile(r'\\\s*n', re.IGNORECASE)
        book = load_book(self.collect_book_path)
        with open(os.path.join(WORK_DIR, 'jp_content.txt'), 'r') as f:
            for text in f:
                text = newline.sub(r'\n', text.replace('\n', ''))
                if chapter.search(text) != None:
                    sheet_name = chapter.sub('', text)
                    sheet = book.sheet_by_name(sheet_name)
                    table = merge.get_table(sheet_name)
                    row_index = HeaderIndex.DATA
                    continue
                label = None
                for c in FieldIndex.indice():
                    cell_value = unicode(sheet.cell(row_index, c).value)
                    if c == FieldIndex.LABEL:
                        label = cell_value
                        continue
                    table.write(label, c, cell_value)
                table.write(label, FieldIndex.JAPANESE, text.decode('utf-8'))
                row_index += 1
        merge.save()

class LibraryHashConverter(object):
    def __init__(self):
        pass

    def run(self, sheet_name, field_names):
        print sheet_name, field_names
        library = TranslationDatabase(type = DatabaseType.LIBRARY)
        if not library.has_table(sheet_name):
            return
        table = library.get_table(sheet_name)
        pattern = re.compile(r'^([^-]+)-([^-]+)-([^-]+)-([^-]+)$')
        for label in table.data.keys():
            match = pattern.match(label)
            name = match.group(4)
            if name in field_names:
                value_list = table.get(label)
                del table.data[label]
                text = value_list[FieldIndex.CHINESE]
                label = u'%s-%s-%s'%(match.group(1), name, md5.md5(text.encode('utf-8')).hexdigest())
                for c in range(1, len(value_list)):
                    table.write(label, c, value_list[c])
        library.save()

def main():
    translate = JapaneseTranslate()
    if len(sys.argv) == 1:
        exit(1)
    command = sys.argv[1].strip().lower()
    if command in ('d', 'dump'):
        translate.dump()
    elif command in ('r', 'repair'):
        translate.repair()
    elif command in ('u', 'upload'):
        uploader = TranslationUploader()
        uploader.run()
    elif command in ('s', 'split'):
        spliter = TranslationSpliter()
        for xls_file_path in sys.argv[2:]:
            spliter.split(os.path.abspath(xls_file_path))
    elif command in ('c', 'count'):
        counter = TranslationDatabase(type = DatabaseType.MEMORY)
        book = load_book(os.path.abspath(sys.argv[2]))
        for sheet_name in book.sheet_names():
            sheet = book.sheet_by_name(sheet_name)
            table = counter.add_table(sheet_name, sheet)
            print '%6s = %s'%('{:,}'.format(table.character_count), sheet_name.encode('utf-8'))
        print '{:,}'.format(counter.character_count)
    elif command in ('h', 'hash'):
        converter = LibraryHashConverter()
        converter.run(sheet_name = sys.argv[2], field_names = sys.argv[3:])

if __name__ == '__main__':
    main()
    