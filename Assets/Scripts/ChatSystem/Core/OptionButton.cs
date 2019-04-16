using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionButton : MonoBehaviour {
	public Text text;
	int index = 0;
    public OptionPanel panel;
	public void SetText(string s)
	{
		text.text = s;
	}
	public void Choose()
	{
        panel.Choose(index);
	}
	public void AddListener(int i)
	{
		index = i;
		GetComponent<Button> ().onClick.AddListener (Choose);
	}
}
