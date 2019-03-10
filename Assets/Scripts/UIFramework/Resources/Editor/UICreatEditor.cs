using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class UICreatWindow : EditorWindow
{
    [MenuItem("Tools/CreatUI")]
    // Use this for initialization
    static void CreatUI()
    {
        UICreatWindow window = (UICreatWindow)EditorWindow.GetWindow(typeof(UICreatWindow), false, "UICreatWindow", true);
        window.Show();
    }
    enum FadeType
    {
        None,
        AlphaFade,
        AnimationFade
    }
    FadeType _fadeType = FadeType.None;
    string _uiName = "";
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("UIName(ForExample,MainMenu):");
        _uiName = EditorGUILayout.TextField(_uiName);
        EditorGUILayout.LabelField("FadeType");
        _fadeType = (FadeType)EditorGUILayout.EnumPopup(_fadeType);
        if (GUILayout.Button("CreatUI"))
        {
            Creat();
        }
        GUILayout.EndVertical();
    }
    void Creat()
    {
        if (_uiName == "")
            return;
        CreatViewScript();
        AddUIType();
        CreatPrefab();
        AssetDatabase.Refresh();
    }
    void AddUIType()
    {
        string typePath = Application.dataPath + "/Scripts/UIFramework/Foundation/UIBase/UIType.cs";
        string[] file = File.ReadAllLines(typePath);
        StringBuilder sb = new StringBuilder();
        sb.Length = 0;
        for (int i = 0; i < file.Length; i++)
        {
            sb.Append(file[i]+"\n");
            if (i == file.Length - 3)
                sb.Append("		public static readonly UIType "+_uiName+" = new UIType(\"View/"+_uiName+"View\");"+"\n");
        }
        File.Delete(typePath);
        File.AppendAllText(typePath, sb.ToString());
    }
    void CreatViewScript()
    {
        string viewName = _uiName + "View";
        string scriptPath = Application.dataPath + "/Scripts/UIFramework/Scripts/";
        string prefabPath = Application.dataPath + "/Scripts/UIFramework/Resources/View/";

        string scriptFilePath = (scriptPath + viewName).Replace("/", "\\") + ".cs";
        if (File.Exists(scriptFilePath))
        {
            Debug.LogError("HasExist!");
            return;
        }
        string[] templetClass = File.ReadAllLines((scriptPath + "TempletView").Replace("/", "\\") + ".cs");
        StringBuilder classStr = new StringBuilder();
        classStr.Length = 0;
        for (int i = 1; i < templetClass.Length - 1; i++)
        {
            classStr.Append(templetClass[i] + "\n");
        }
        string finalClassStr = classStr.ToString().Replace("Templet", _uiName);
        switch (_fadeType)
        {
            case FadeType.None:
                finalClassStr = finalClassStr.Replace("ParentView", "EnabledView");
                break;
            case FadeType.AlphaFade:
                finalClassStr = finalClassStr.Replace("ParentView", "AlphaView");
                break;
            case FadeType.AnimationFade:
                finalClassStr = finalClassStr.Replace("ParentView", "AnimateView");
                break;
        }
        File.AppendAllText(scriptFilePath, finalClassStr);
    }
    void CreatPrefab()
    {
        GameObject go = Resources.Load<GameObject>("View/Templet") as GameObject;
        go.name = _uiName + "View";
        PrefabUtility.CreatePrefab("Assets/Scripts/UIFramework/Resources/View/"+_uiName+"View.prefab", go);
    }
}