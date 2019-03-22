using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionButton : MonoBehaviour {
	public Text text;
	int index = 0;
	public void SetText(string s)
	{
		text.text = s;
	}
	public void Choose()
	{
		ChatManager.Instance.curInstance.saveData.AddOption (ChatManager.Instance.curInstance.curRunningNode,index);
		ChatManager.Instance.TryGetOptionNode ().option = index;
	}
	public void AddListener(int i)
	{
		index = i;
		GetComponent<Button> ().onClick.AddListener (Choose);
	}
}
