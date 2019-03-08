import os
os.chdir(os.path.dirname(os.path.abspath(__file__)))
os.system("python SoundTool/attack_sound_export.py")
os.system("python xlsc.py attack_sound_tmp ATTACK_SOUND_CONF")

os.system("python xlsc_precompile.py")
