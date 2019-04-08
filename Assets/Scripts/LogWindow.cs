using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
public class LogWindow : MonoBehaviour {
	// Use this for initialization
	StringBuilder sb = new StringBuilder();
	public GameObject logWin;
	public Text log;
	public Text button;
	void Awake() {
		sb.Length = 0;
		Application.logMessageReceived += LogCallback;
	}
	public void LogCallback (string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Log) {
			sb.Append (condition + stackTrace + "\n");
		} else {
			button.text = "<color=red>ERROR</color>";
			sb.Append ("<color=red>"+condition + stackTrace+"</color>" + "\n");
		}
		log.text = sb.ToString ();
	}
	public void OnClick()
	{
		logWin.SetActive (!logWin.activeSelf);
		button.text = "LOG";
	}
}
