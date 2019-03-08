import os
import sys
import platform

def copy_file(srcFile, desFile):
	if(os.path.isfile(srcFile)):
		open(desFile,"wb").write(open(srcFile, "rb").read())

def build_and_serialize(proj_folder, assembly_name, out_folder):
	csproj = assembly_name + ".csproj"
	ori_dll = assembly_name + ".dll"
	serializer_param = assembly_name + "Serializer"
	serializer_dll = assembly_name + "Serializer.dll"

	csproj_path = os.path.join(proj_folder, csproj)
	ori_dll_path = os.path.join(proj_folder, "bin", "Release", ori_dll)
	serializer_dll_path = os.path.join(proj_folder, "bin", "Release", serializer_dll)

	sysstr = platform.system()
	if(sysstr=="Windows"):
		ret = os.system("C:/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe " + csproj_path + " /property:Configuration=Release" + " /toolsversion:3.5")
	else:
		build_command = "xbuild %s /property:Configuration=Release /toolsversion:3.5 /property:Platform='AnyCPU'"%(csproj_path)
		print build_command
		ret = os.system(build_command)

	if ret != 0:
		raise Exception("Fail xbuild " + assembly_name)

	if(sysstr=="Windows"):
		os.chdir("Precompile")
		serializer_command = "precompile.exe " + os.path.join("..", ori_dll_path) + " -t:" + serializer_param + " -o:" + os.path.join("..", serializer_dll_path)
		ret = os.system(serializer_command)
		os.chdir("..")
	else:
		serializer_command = "mono Precompile/precompile.exe " + ori_dll_path + " -t:" + serializer_param + " -o:" + serializer_dll_path
		ret = os.system(serializer_command)

	if ret != 0:
		raise Exception("Fail precompile " + assembly_name)

	copy_file(ori_dll_path, os.path.join(out_folder, ori_dll))
	copy_file(serializer_dll_path, os.path.join(out_folder, serializer_dll))

if __name__ == "__main__":
	build_and_serialize(sys.argv[1], sys.argv[2], sys.argv[3])