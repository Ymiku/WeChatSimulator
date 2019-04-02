using System.Collections;
using UnityEngine;
using System.IO;
using System.Windows.Forms;

public class UploadTexture : MonoBehaviour {
    public Texture2D img = null;
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 500, 150), "选择文件"))
        {

            OpenFileDialog od = new OpenFileDialog();
            od.Title = "请选择头像图片";
            od.Multiselect = false;
            od.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            if (od.ShowDialog() == DialogResult.OK)
            {
                Debug.Log(od.FileName);
                StartCoroutine(GetTexture(od.FileName));
            }
        }
        if (img != null)
        {
            GUI.DrawTexture(new Rect(0, 20, img.width / 2, img.height / 2), img);
        }
    }

    IEnumerator GetTexture(string url)
    {
        WWW www = new WWW("file://" + url);
        yield return www;
        if (www.isDone)
        {
            img = www.texture;
        }
    } 
}
