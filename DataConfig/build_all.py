import os
import sys
import shutil

excel_path = "E:\projects\PaymentAppSimulator\DataConfig\Excel"
cs_path = "E:\projects\PaymentAppSimulator\Assets\Scripts\StaticDataLoader\PB"
data_path = "E:\projects\PaymentAppSimulator\Assets\Resources\StaticData"

is_exists = os.path.exists(excel_path)
if not is_exists:
    print 'excel path not exist, please set the right path first'
    sys.exit()

is_exists = os.path.exists(cs_path)
if not cs_path:
    print 'cs path not exist, please set the right path first'
    sys.exit()

is_exists = os.path.exists(data_path)
if not is_exists:
    print 'data path not exist, please set the right path first'
    sys.exit()

dirs = os.listdir(excel_path)
for i in dirs:
    if os.path.splitext(i)[1] == ".xlsx":
		excel_full_name = i
		excel_name = i[0:-5]
		upper_name = excel_name.upper()
		print 'Building... '+ excel_full_name
		bat_str = 'call python xls_deploy_tool.py ' + upper_name + ' Excel/' + excel_full_name
		os.system(bat_str)
		bat_str = 'protoc static_data_' + excel_name + '.proto --descriptor_set_out=' + excel_name + '.protodesc'
		os.system(bat_str)
		bat_str = 'ProtoGen\protogen -i:' + excel_name + '.protodesc -o:' + excel_name + '.cs'
		os.system(bat_str)

for root, dirs, files in os.walk(data_path):
    for name in files:
      if name.endswith(".data"):
        os.remove(os.path.join(root, name))
        print ("Delete Old Verson Data: " + os.path.join(root, name))

for root, dirs, files in os.walk(cs_path):
    for name in files:
      if name.endswith(".cs"):
        os.remove(os.path.join(root, name))
        print ("Delete Old Verson Cs: " + os.path.join(root, name))

for derName, subfolders, filenames in os.walk("."):
    for i in range(len(filenames)):
        if filenames[i].endswith('.data'):
            file_path = derName + '/' + filenames[i]
            new_path = data_path + '/' + filenames[i]
            is_exists = os.path.exists(new_path)
            if not is_exists:
              shutil.copy(file_path, new_path)
              print ("Add New Verson Data: " + filenames[i])

for derName, subfolders, filenames in os.walk("."):
    for i in range(len(filenames)):
        if filenames[i].endswith('.cs'):
            file_path = derName + '/' + filenames[i]
            new_path = cs_path + '/' + filenames[i]
            is_exists = os.path.exists(new_path)
            if not is_exists:
              shutil.copy(file_path, new_path)
              print ("Add New Verson Cs: " + filenames[i])

filelist=[]
filelist=os.listdir(".")
for flie in filelist:
    filepath = os.path.join(".", flie)
    if os.path.isfile(filepath):
      remove_flag = (filepath.endswith(".cs") or filepath.endswith(".protodesc") or
	  filepath.endswith(".log") or filepath.endswith(".data") or filepath.endswith(".txt") or
      filepath.endswith("_pb2.py") or filepath.endswith("_pb2.pyc") or filepath.endswith(".proto"))
      if remove_flag:
          os.remove(filepath)
          print ("Delete Template File: " + filepath)

print ("Build All Success")