#!/usr/bin/env python
#encoding: utf-8

# from __future__ import print_function
import re, urllib, sys, os

class NEXTReportPostman(object):
    def __init__(self, sender):
        """
        初始化邮件信息
        sender: 发件人RTX英文名
        """
        self.server = "http://test.next.qq.com:8081/mail_report"
        self.params = {"token":"Next2016", "content_type":0}
        self.params["mail_id"] = sender
        
    def send(self, title, msg_location):
        """
        初始化邮件信息
        title: 邮件标题
        msg_location: 待发发送文本文件路径
        """
        self.params["title"] = title # "NEXT配置表规范检测报告"
        msg = open(msg_location, "r")
        self.params["content"] = msg.read()
        msg.close()

        url = "%s?%s"%(self.server, urllib.urlencode(self.params).replace('+', '%20'))
        print "sending >>> %s"%(url)
        rsp = urllib.urlopen(url)
        for (key, value) in rsp.info().items():
            print "%s:%s"%(key, value)
        print rsp.read()
        return rsp.getcode()

def main():
    
    postman = NEXTReportPostman("next_all")
    if len(sys.argv) <= 1:
        print "缺少邮件文本文件路径"
        exit(1)
    else:
        msg_location = sys.argv[1]
        msg_location = os.path.abspath(msg_location)
        if os.path.exists(msg_location):
            print postman.send(sys.argv[2], msg_location)
        else:
            print "文件[%s|%s]不存在"%(sys.argv[1], msg_location)

if __name__ == '__main__':
    main()
    
