#! /usr/bin/env python
#coding=utf-8

import os
import sys, re
import string
import platform

ERROR_LOG = "/tmp/parse_error.log"
if __name__ == '__main__' :
    XLS_NAME 	= sys.argv[1]
    SHEET_NAME 	= sys.argv[2]
    if len(sys.argv) >= 4:
        DATA_NAME_SUFFIX = sys.argv[3]
    else:
        DATA_NAME_SUFFIX = None
    STEP1_XLS2PROTO_PATH = "step1_xls2proto"
    STEP3_SERIALIZER = "step3_serializer"

    print "=========Compilation of %s.xls=========" %XLS_NAME

    os.chdir(STEP1_XLS2PROTO_PATH)
    print("current run dir : %s" %os.getcwd())
    print "TRY TO DELETE TEMP FILES IN STEP1:"

    def del_files_in_step1(dir,topdown=True):
        abs_dir = os.path.abspath(dir)
        pattern = re.compile(r'(_pb2\.py[c]?|dataconfig_.*\.proto|\.data|\.log|\.txt)$', re.IGNORECASE)
        for name in os.listdir(dir):
            if pattern.search(name) != None:
                os.remove(name)
                print "DELETE %s"%(name)

    del_files_in_step1(os.getcwd())
    print("del end")

    protoFScalar_command = "protoc --python_out=./ ProtoFScalar.proto"
    os.system(protoFScalar_command)
    
    xls_file = os.path.abspath('../DataConfig/%s.xls'%(XLS_NAME))
    if not os.path.exists(xls_file):
        xls_file = '%sx'%(xls_file)
    xls_deploy_tool_command = 'python xls_deploy_tool.py %s %s'%(SHEET_NAME, xls_file)
    print "command ====== " + xls_deploy_tool_command
    deploy_ret = os.system(xls_deploy_tool_command)
    deploy_ret >>= 8

    if (0 != deploy_ret):
        print("before Open File")
        file = open(ERROR_LOG, 'w')
        file.write(xls_deploy_tool_command + " " + "Parse error.\n")
        file.close()
        print("Open File")
        print(xls_deploy_tool_command + " " + "Parse error.\n")
        sys.exit(deploy_ret)


#---------------------------------------------------
#  step2: proto->cs
#---------------------------------------------------
    os.chdir("..")
    print("step2 current run dir : %s" %os.getcwd())

    STEP2_PROTO2CS_PATH = os.path.join(".","step2_proto2cs")
    PROTO_DESC = "proto.protodesc"
    SRC_OUT = os.path.join("..","src")

    os.chdir(STEP2_PROTO2CS_PATH);
    print("current run dir : %s" %os.getcwd())
    print "TRY TO DELETE TEMP FILES in STEP2:"

    def del_files_in_step2(dir,topdown=True):
        pattern = re.compile(r'\.(cs|protodesc|txt)$', re.IGNORECASE)
        for name in os.listdir(dir):
            if pattern.search(name) != None:
                os.remove(name)
                print "DELETE %s"%(name)

    del_files_in_step2(os.getcwd())
    print("del end")
    
    proto_file_list = []
    proto_dir = os.path.join("..",STEP1_XLS2PROTO_PATH)
    
    for name in os.listdir(proto_dir):
        if (-1 != name.find(".proto")):
            fileName,fileExt = os.path.splitext(os.path.basename(name))
            proto_file_list.append(fileName)
            print("proto file : %s" %fileName)
    
    for proto_file in proto_file_list:
        print proto_file
        protoc_command = os.path.join(".","protoc --descriptor_set_out=" + PROTO_DESC + " --proto_path=..",STEP1_XLS2PROTO_PATH + " ..",STEP1_XLS2PROTO_PATH,proto_file + ".proto")
        print "command ====== " + protoc_command
        os.system(protoc_command)

        if(proto_file.find("ProtoFScalar") == -1) :
            if platform.system().lower() == "windows":
                protogen_command = os.path.join("ProtoGen","protogen.exe -i:" + PROTO_DESC + " -o:" + proto_file + ".cs")
            else:
                protogen_command = os.path.join("mono ProtoGen","protogen.exe -i:" + PROTO_DESC + " -o:" + proto_file + ".cs")
            print "command ====== " + protogen_command
            protogen_ret = os.system(protogen_command)
            protogen_ret >>= 8;
            if (0 != protogen_ret):
                file = open(ERROR_LOG, 'w')
                file.write(protogen_command + " " + "Parse error.\n")
                file.close()
                print(protogen_command + " " + "Parse error.\n")
                sys.exit(protogen_ret)

    os.chdir("..")
    print("step2 current run dir : %s" %os.getcwd())

#---------------------------------------------------
#step3 : data and cs to assets dir
#---------------------------------------------------
    OUT_PATH=os.path.join("..","Assets")
    DATA_DEST=os.path.join("Resources","DataConfig")
    CS_DEST=os.path.join("..","BuildProtoSolution","ProtoBufConfig","ProtoGen")

    def copy(src_file, dst_file):
        src_file = os.path.abspath(src_file).replace(os.getcwd() + '/', '')
        dst_file = os.path.abspath(dst_file)
        if(os.path.isfile(src_file)):
            open(dst_file,"wb").write(open(src_file, "rb").read())
            print 'COPY %s -> %s'%(src_file, dst_file)

    def get_with_extensions(dir, extension):
        file_list = []
        pattern = re.compile(r'\.%s$'%(extension), re.IGNORECASE)
        for name in os.listdir(dir):
            if pattern.search(name) == None:
                continue
            file_list.append(os.path.join(dir, name))
        return file_list

    for src_file in get_with_extensions(STEP1_XLS2PROTO_PATH, "data"):
        file_name = os.path.basename(re.sub(r'\.data$', '.bytes', src_file))
        if DATA_NAME_SUFFIX != None: # 分表专用
            file_name = re.sub(r'(\.bytes)$', r'%s\1'%(DATA_NAME_SUFFIX), file_name)
        dst_file = os.path.join(OUT_PATH, DATA_DEST, file_name)
        copy(src_file, dst_file)

    for src_file in get_with_extensions(STEP2_PROTO2CS_PATH, "cs"):
        file_name = os.path.basename(src_file)
        dst_file = os.path.join(OUT_PATH, CS_DEST, file_name)
        copy(src_file, dst_file)

#---------------------------------------------------
#step4 : do del
#---------------------------------------------------

    print "TRY TO DEL TEMP FILE:"
    print "step1:"
    os.chdir(STEP1_XLS2PROTO_PATH)
    del_files_in_step1(os.getcwd())
    print "step2:"
    os.chdir("..")
    os.chdir(STEP2_PROTO2CS_PATH)
    del_files_in_step2(os.getcwd())
    os.chdir("..")
