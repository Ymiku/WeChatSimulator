using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PairIdCalculator : EditorWindow {

	[MenuItem("Tools/计算对话组合ID")]
	static void PairIdCalculatorWindow()
	{
		PairIdCalculator window = (PairIdCalculator)EditorWindow.GetWindow<PairIdCalculator>();
		window.Show ();

	}
	int id1;
	int id2;
	void OnGUI()
	{
		id1 = EditorGUILayout.IntField (id1);
		id2 = EditorGUILayout.IntField (id2);
		int i;
		if (id1 < id2) {
			i = (id1 << 8) + id2;
		} else {
			i = (id2 << 8) + id1;
		}
		GUILayout.Label (i.ToString());
	}
}
