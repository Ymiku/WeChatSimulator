#!/usr/bin/env python
#encoding: utf-8

import re, os, sys

def parse_str(class_file):
    cs = open(class_file, 'r')
    result = []
    pattern = re.compile(r'public\s+const', re.MULTILINE|re.IGNORECASE)
    for line in cs.readlines():
        if pattern.search(line) != None:
            value = line.split('"')[1].strip()
            value = value.decode(encoding='utf-8')
            if not (value in result):
                result.append(value)
    return result

def get_stat_types():
    location = "%s/%s"%(os.path.dirname(os.path.abspath(__file__)),'link/StatType.cs')
    return parse_str(os.path.abspath(location))

def get_status_types():
    location = "%s/%s"%(os.path.dirname(os.path.abspath(__file__)),'link/StatusType.cs')
    return parse_str(os.path.abspath(location))


def main():
    print get_status_types()
    print get_stat_types()

if __name__ == '__main__':
    main()
