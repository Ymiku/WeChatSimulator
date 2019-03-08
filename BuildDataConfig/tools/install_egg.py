#!/usr/bin/env python
#encoding: gbk

import os, sys, re, platform
import shutil

def install_package(package_file):
    print '>>> installing %s'%(package_file)
    pattern = re.compile(r'\.tar\.gz$', re.IGNORECASE)
    if pattern.search(package_file) == None:
        return
    dist = 'dist'
    package_name = pattern.sub('', package_file)
    def clean():
        if os.path.exists(dist):
            shutil.rmtree(dist)
        if os.path.exists(package_name):
            shutil.rmtree(package_name)
    clean()
    command = '7z\\7za x %s'%(package_file)
    if os.system(command) == 0:
        command = '7z\\7za x %s\%s.tar'%(dist, package_name)
        if os.system(command) == 0:
            work_dir = os.getcwd()
            os.chdir(package_name)
            command = 'python setup.py install'
            os.system(command)
            os.chdir(work_dir)
    clean()

def main():
    if platform.system().lower() != 'windows':
        return
    work_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(work_dir)
    python_installer = os.path.join(work_dir, 'python-2.7.13.msi')
    pattern = re.compile(r'Python27[\\]?$', re.IGNORECASE)
    for location in re.split(r'\s*;\s*', os.environ['PATH']):
        location = str(location).strip()
        if pattern.search(location) == None:
            continue
        # 安装导表工具依赖库
        pattern = re.compile(r'\.tar\.gz$', re.IGNORECASE)
        for package_file in os.listdir(work_dir):
            if pattern.search(package_file) != None:
                install_package(package_file)
        # 安装*.egg
        installer = os.path.join(location, 'Scripts\\easy_install.exe')
        if not os.path.exists(installer):
            raise Exception('*.egg安装工具<%s>不存在，请双击<%s>重新安装Python运行环境'%(installer, python_installer))
            return
        pattern = re.compile(r'\.egg$', re.IGNORECASE)
        for egg_file in os.listdir(work_dir):
            if pattern.search(egg_file) == None:
                continue
            command = '%s -Z %s'%(installer, egg_file)
            print '==> %s'%(command)
            os.system(command)
        return
    raise Exception('未安装python脚本运行环境，请双击<%s>安装'%(python_installer))

if __name__ == '__main__':
    main()