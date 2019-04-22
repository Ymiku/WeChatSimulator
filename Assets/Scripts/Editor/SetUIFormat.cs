using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public static class SetUIFormat
{

    [MenuItem("GameObject/设置button", priority = 0)]
    static void SetB()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].navigation = new Navigation();
        }
    }
    [MenuItem("GameObject/将Image和Text转为Proxy", priority = 0)]
    static void SetUI()
    {
		Button[] buttons = GameObject.FindObjectsOfType<Button> ();
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].navigation = new Navigation ();
		}
        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            string s = texts[i].text;
            int m = texts[i].fontSize;
            Font f = texts[i].font;
            Color c = texts[i].color;
            TextAnchor a = texts[i].alignment;
            GameObject go = texts[i].gameObject;
            GameObject.DestroyImmediate(texts[i]);
            TextProxy p = go.AddComponent<TextProxy>();
            p.text = s;
            p.font = f;
            p.fontSize = m;
            p.color = c;
            p.alignment = a;
        }
        Image[] images = Selection.activeGameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
			if (images [i] is ImageProxy || images [i] is UnityEngine.UI.ProceduralImage.ProceduralImage)
				continue;
            Sprite s = images[i].sprite;
            Color c = images[i].color;
            Material m = images[i].material;
			Image.Type t = images [i].type;
            GameObject go = images[i].gameObject;
            GameObject.DestroyImmediate(images[i]);
            ImageProxy p = go.AddComponent<ImageProxy>();
			p.type = t;
            p.sprite = s;
            p.material = m;
            p.color = c;
        }
    }
    [MenuItem("GameObject/清除RaycastTarget", priority = 0)]
    static void ClearRayCastTarget()
    {
        Graphic[] g = Selection.activeGameObject.GetComponentsInChildren<Graphic>();
        for (int i = 0; i < g.Length; i++)
        {
            if (g[i].GetComponent<Button>() != null || g[i].transform.parent.GetComponent<Button>() != null)
                continue;
            g[i].raycastTarget = false;
        }
    }
    [MenuItem("Tools/定位到UI素材路径", priority = 0)]
    static void SetProjectViewToArts()
    {
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Arts/Sprites/ArtsFolders.prefab");
        Selection.activeGameObject = go;
    }
    [MenuItem("Tools/定位到UI Prefab路径", priority = 0)]
    static void SetProjectViewToUIPrefabs()
    {
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/UIFramework/Resources/View/Templet.prefab");
        Selection.activeGameObject = go;
    }
	[MenuItem("GameObject/UI/TextProxy", false, 2000)]
	static public void AddText(MenuCommand menuCommand)
	{
		GameObject go = new GameObject("TextProxy");
		TextProxy txt = go.AddComponent<TextProxy>();
		go.transform.SetParent (Selection.activeGameObject.transform);
		txt.raycastTarget = false;
		txt.font = AssetDatabase.LoadAssetAtPath<Font> ("Assets/Arts/tif/default.ttf");
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		Selection.activeObject = go;
	}
	[MenuItem("GameObject/UI/ImageProxy", false, 2000)]
	static public void AddImage(MenuCommand menuCommand)
	{
		GameObject go = new GameObject("ImageProxy");
		ImageProxy txt = go.AddComponent<ImageProxy>();
		go.transform.SetParent (Selection.activeGameObject.transform);
		txt.raycastTarget = false;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		Selection.activeObject = go;
	}
	[MenuItem("GameObject/UI/ClippingImage", false, 2000)]
	static public void AddCImage(MenuCommand menuCommand)
	{
		GameObject go = new GameObject("ClippingImage");
		ClippingImage txt = go.AddComponent<ClippingImage>();
		go.transform.SetParent (Selection.activeGameObject.transform);
		txt.raycastTarget = false;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		Selection.activeObject = go;
	}
}