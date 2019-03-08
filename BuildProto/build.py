#!/usr/bin/env python
#coding=utf-8
import os
import sys, re
import string 
ROOT_DIR = os.path.dirname(os.path.abspath(__file__))
PROJ_DIR = os.path.abspath(os.path.join(ROOT_DIR, '..'))
SHAR_PROTO_PATH = os.path.join(ROOT_DIR, 'Protocol')
ENUM_PROTO_PATH = os.path.abspath(os.path.join(ROOT_DIR, '../BuildDataConfig/step1_xls2proto/'))
TOOL_PATH = os.path.join(ROOT_DIR, 'step1_proto2cs')
PROTO_DESC = "%s.protodesc"
def delete_with_extension(folder, extension):
	if type(extension) is list:
		pattern = re.compile(r'\.(%s)$'%('|'.join(extension)), re.IGNORECASE)
	elif type(extension) is str:
		pattern = re.compile(r'%s$'%(extension), re.IGNORECASE)
	for name in os.listdir(folder):
		if pattern.search(name) != None:
			target = "%s/%s"%(folder, name)
			os.remove(target)
			# print "DEL %s"%(target)
if __name__ == '__main__' :
	
	#---------------------------------------------------
	#step1 : remove temp file -> proto gen cs
	#---------------------------------------------------
	os.chdir(TOOL_PATH)
	delete_with_extension(os.getcwd(), ['protodesc', 'cs'])
	pattern = re.compile(r'\.proto$', re.IGNORECASE)
	for proto_file in os.listdir(SHAR_PROTO_PATH):
		if pattern.search(proto_file) == None:
			continue
		proto_name = pattern.sub('', proto_file)
		protoc_command = "protoc --descriptor_set_out=%s --proto_path=%s --proto_path=%s %s"%( \
							PROTO_DESC%(proto_name), SHAR_PROTO_PATH, ENUM_PROTO_PATH, "%s/%s"%(SHAR_PROTO_PATH, proto_file))
		if os.system(protoc_command) != 0:
			raise Exception("执行<%s>失败"%(protoc_command))
		print "command ====== %s"%(protoc_command)
		protogen_command = "mono ProtoGen/protogen.exe -p:detectMissing -i:%s -o:%s"%(\
							PROTO_DESC%(proto_name), "%s.cs"%(proto_name))
		if os.system(protogen_command) != 0:
			raise Exception("执行<%s>失败"%(protogen_command))
		print "command ====== %s"%(protogen_command)
	
	#---------------------------------------------------
	#step2 : data and cs to assets dir
	#---------------------------------------------------
	CS_OUTPUT_PATH = os.path.join(PROJ_DIR, "BuildProtoSolution", "ProtobufProtocol", "ProtoGen")
	print PROJ_DIR
	print CS_OUTPUT_PATH
	pattern = re.compile(r'\.cs$', re.IGNORECASE)
	for cs_file in os.listdir('.'):
		if pattern.search(cs_file) == None:
			continue
		src = open(cs_file, 'rb')
		dst = open("%s/%s"%(CS_OUTPUT_PATH, cs_file), 'wb')
		print "COPY %s -> %s"%(cs_file, "%s/%s"%(CS_OUTPUT_PATH, cs_file))
		dst.write(src.read())
		src.close()
		dst.close()
	#---------------------------------------------------
	#step3 : do del
	#---------------------------------------------------
	delete_with_extension(os.getcwd(), ['protodesc', 'cs'])
	
	#---------------------------------------------------
	#step3 : xbuild project and serializer
	#---------------------------------------------------
	os.chdir(ROOT_DIR)
	if os.system("python build_precompile.py") != 0:
		raise Exception("执行<python build_precompile.py>失败")
		
