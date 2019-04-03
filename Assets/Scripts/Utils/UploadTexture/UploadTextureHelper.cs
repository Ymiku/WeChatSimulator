using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class UploadTextureHelper : MonoBehaviour
{
    private Texture2D _sourceTexture = null;
    public RawImage image;
    public Button openDialogBtn;

    public void OpenDialog()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "All Files\0*.*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径  
        ofn.title = "上传头像";
        ofn.defExt = "JPG";//显示文件的类型  
                           //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  
        if (WindowDll.GetOpenFileName(ofn))
        {
            StartCoroutine(GetTexture(ofn.file));
        }
    }

    IEnumerator GetTexture(string url)
    {
        WWW wwwTexture = new WWW("file://" + url);
        yield return wwwTexture;
        if (wwwTexture.isDone)
        {
            _sourceTexture = wwwTexture.texture;
            if (_sourceTexture.width != _sourceTexture.height)
            {
                Texture2D desTexture = null;
                Color[] desColors;
                if (_sourceTexture.width > _sourceTexture.height)
                {
                    desTexture = new Texture2D(_sourceTexture.height, _sourceTexture.height);
                    desColors = _sourceTexture.GetPixels((_sourceTexture.width  - _sourceTexture.height) / 2, 0, 
                        _sourceTexture.height, _sourceTexture.height);
                }
                else
                {
                    desTexture = new Texture2D(_sourceTexture.width, _sourceTexture.width);
                    desColors = _sourceTexture.GetPixels(0, (_sourceTexture.height - _sourceTexture.width) / 2,
                        _sourceTexture.width, _sourceTexture.width);
                }
                desTexture.SetPixels(desColors);
                desTexture.Apply();
                image.texture = desTexture;
            }
            else
                image.texture = _sourceTexture;
        }
    }
}


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class WindowDll
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }
}