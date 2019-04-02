using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public class Upload : MonoBehaviour
{
    public Texture2D img = null;
    private bool click = false;

    void EnterImage()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "All Files\0*.*\0\0";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径  

        ofn.title = "Open Project";

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
            img = wwwTexture.texture;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 500, 150), "选择文件"))
        {
            EnterImage();
        }
        if (img != null)
            GUI.DrawTexture(new Rect(
                Screen.width / 2 - img.width / 2,
                Screen.height / 2 - img.height,
                img.width,
                img.height), img);
    }
}