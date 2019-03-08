import os

#---------------------------------------------------
#step3 : xbuild project and serializer
#---------------------------------------------------
PROTOBUF_PATH = os.path.join("..","BuildProtoSolution","ProtobufProtocol")
PROTOBUF_PROJECT = "ProtobufProtocol"
PROTOBUF_OUT_PATH = os.path.join("..","Assets","Plugins")
#os.chdir(STEP3_SERIALIZER)
os.system("python precompile.py " + PROTOBUF_PATH + " " + PROTOBUF_PROJECT + " " + PROTOBUF_OUT_PATH)
#os.chdir("..")