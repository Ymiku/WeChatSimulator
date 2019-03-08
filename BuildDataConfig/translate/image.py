#!/usr/bin/env python
#encoding: utf-8

from __init__ import ROOT_PATH, FieldIndex
from checker.xlsdata.utils import os_encode
import os, re, sys, md5

LOCALE_ROOT_PATH = os.path.abspath(os.path.join(ROOT_PATH, '..', 'Assets/ArtResource/locale'))
RESOURCE_PATH = os.path.abspath(os.path.join(ROOT_PATH, '..', 'Assets/Resources'))
IMAGE_PATTERN = re.compile(r'\.(png|jpg)$', re.IGNORECASE)

class ImageTranslator(object):
    def __init__(self, root_path = LOCALE_ROOT_PATH):
        self.root_path = root_path

    def __md5(self, file_path):
        with open(file_path, 'rb') as fp:
            return md5.md5(fp.read()).hexdigest()

    def __translate_image(self, src_image_path, dst_image_path):
        if self.__md5(src_image_path) != self.__md5(dst_image_path):
            with open(src_image_path, 'rb') as src:
                with open(dst_image_path, 'wb') as dst:
                    dst.write(src.read())
            print os_encode('TRANSLATE %s => %s'%(src_image_path, dst_image_path))
        
    def translate(self, field_index = FieldIndex.JAPANESE):
        locale_name = FieldIndex.name(field_index)
        import os.path as p
        if not p.exists(p.join(self.root_path, locale_name)):
            raise Exception(os_encode('多语言资源[%s]不存在'%(locale_name)))
            return
        image_root_path = p.join(self.root_path, locale_name)
        for base_path, _, file_name_list in os.walk(image_root_path):
            for file_name in file_name_list:
                if not IMAGE_PATTERN.search(file_name):
                    continue
                src_image_path = p.join(base_path, file_name)
                relative_image_path = re.sub(r'%s\/'%(image_root_path), '', src_image_path)
                dst_image_path = p.join(RESOURCE_PATH, relative_image_path)
                if not p.exists(dst_image_path):
                    continue
                self.__translate_image(src_image_path, dst_image_path)

def main():
    translator = ImageTranslator(root_path = LOCALE_ROOT_PATH)
    translator.translate()

if __name__ == '__main__':
    main()