using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public static class CalTextSize{

	[MenuItem("CONTEXT/Text/LogPreferredSize", priority = 0)]
	static void LogPreferred(MenuCommand mc)
	{
		Text t = mc.context as Text;
		Debug.Log ("width"+t.preferredWidth.ToString()+"height"+t.preferredHeight.ToString());
	}
	[MenuItem("CONTEXT/Text/SetPreferredSize", priority = 0)]
	static void SetPreferred(MenuCommand mc)
	{
		Text t = mc.context as Text;
		t.rectTransform.sizeDelta = new Vector2 (t.preferredWidth,t.preferredHeight);
	}
}
