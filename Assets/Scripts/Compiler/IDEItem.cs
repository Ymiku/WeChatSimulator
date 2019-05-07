using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IDEItem : ItemBase {
	public TextProxy text;
	void Awake()
	{
		if(GetComponent<Dropdown>()!=null)
		GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChange);
	}
	public void SetText(string s)
	{
		text.text = s;
		width = text.preferredWidth;
	}
	public void OnValueChange(int i)
	{
		Debug.Log (i);
	}
}
