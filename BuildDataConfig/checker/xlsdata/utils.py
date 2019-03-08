#!/usr/bin/env python
#encoding: utf-8
import re, os, sys, xlrd, time
import subprocess
import platform

STD_TIME_ZONE = 8 # 北京时间所在时区/东八区

def is_winsystem():
    return platform.system().lower() == "windows"

def check_command_exists(command):
    if is_winsystem():
        env_locations = re.split(r';', os.environ["PATH"])
    else:
        env_locations = re.split(r':', os.environ["PATH"])
    for location in env_locations:
        location = str(location).strip()
        if location == "":
            continue
        executable_file = os.path.join(location, command)
        if is_winsystem():
            executable_file = "%s.exe"%(executable_file)
        if os.path.exists(executable_file):
            return True
    return False

def is_svn_installed():
    if check_command_exists("svn"):
        return True
    if is_winsystem():
        os.environ["PATH"] += os.pathsep + "C:\\Program Files\\TortoiseSVN\\bin"
        os.environ["PATH"] += os.pathsep + "C:\\Program Files (x86)\\TortoiseSVN\\bin"
        return check_command_exists("svn")
    return False

def os_encode(str_data):
    if is_winsystem():
        str_data = str(str_data).decode(encoding='utf-8').encode(encoding='gbk', errors = 'ignore')
    return str_data

def get_sheet_names(xls_file):
    name_list = []
    book = xlrd.open_workbook(xls_file)
    for name in book.sheet_names():
        if name.isupper():
            name_list.append(name)
    book.release_resources()
    return name_list

def supported_date_formats():
    format_list = []
    format_list.append('%Y-%m-%dT%H:%M:%S')
    format_list.append('%Y-%m-%d %H:%M:%S')
    format_list.append('%Y/%m/%dT%H:%M:%S')
    format_list.append('%Y/%m/%d %H:%M:%S')
    return format_list

def supported_date_examples():
    examples = []
    nowtime = time.localtime()
    for date_format in supported_date_formats():
        examples.append(time.strftime(date_format, nowtime))
    return examples

def parse_local_date(date_string):
    date = None
    for date_format in supported_date_formats():
        try:
            date = time.strptime(date_string, date_format)
        except BaseException, e:
            continue
        return date
    return None

def parse_local_date_seconds(date_string):
    date = parse_local_date(date_string)
    if date != None:
        return int(time.mktime(date))
    return None

def parse_date(date_string, time_zone = STD_TIME_ZONE):
    seconds = parse_date_seconds(date_string, time_zone = time_zone)
    if seconds != None:
        return time.localtime(seconds)
    return None

def parse_date_seconds(date_string, time_zone = STD_TIME_ZONE):
    date = parse_local_date(date_string)
    if date != None:
        # time.timezone =  UTC - MACHINE_TIME
        # STD_TIME_ZONE_OFFSET = [北京时间] - UTC
        return int(time.mktime(date)) + ((-time.timezone) - time_zone * 3600)
    return None

def parse_duration_seconds(duration_string):
    seconds = 0
    components = re.split(r'\s*:\s*', duration_string)
    for i in range(len(components)):
        component = int(components[len(components) - i - 1])
        seconds += component * pow(60, i)
    return seconds
