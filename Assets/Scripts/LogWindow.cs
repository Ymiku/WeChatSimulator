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
	public class FLog
	{
		public string fcondition; 
		public string fstackTrace; 
		public LogType ftype;
	}
	List<FLog> logs = new List<FLog>();
	void Awake() {
		sb.Length = 0;
		Application.logMessageReceived += LogCallback;
	}
	public void LogCallback (string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Error && condition.StartsWith ("String too long for TextMeshGenerator")) {
			sb.Remove (0,(int)(sb.Length*0.4f));
			return;
		}
		logs.Add (new FLog(){fcondition = condition,fstackTrace=stackTrace,ftype = type});

	}
	public void OnClick()
	{
		logWin.SetActive (!logWin.activeSelf);
		button.text = "LOG";
	}
	void Update()
	{
		if (logs.Count == 0)
			return;
		for (int i = 0; i < logs.Count; i++) {
			if (logs[i].ftype != LogType.Error) {
				sb.Append (logs[i].fcondition + logs[i].fstackTrace + "\n");
			} else {
				button.text = "<color=red>ERROR</color>";
				sb.Append ("<color=red>"+logs[i].fcondition+"</color>" + logs[i].fstackTrace + "\n");
			}

		}
		log.text = sb.ToString ();
		logs.Clear();
	}
	public void Clear()
	{
		sb.Length = 0;
		log.text = sb.ToString ();
	}
}
