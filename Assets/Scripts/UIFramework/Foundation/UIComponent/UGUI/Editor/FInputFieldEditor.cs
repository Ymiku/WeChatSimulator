using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(FInputField), true)]
public class FInputFieldEditor : InputFieldEditor {
    SerializedProperty f_InputType;
    // Use this for initialization
    protected override void OnEnable()
    {
        base.OnEnable();
        f_InputType = serializedObject.FindProperty("f_InputType");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(f_InputType);
        serializedObject.ApplyModifiedProperties();
    }
}
