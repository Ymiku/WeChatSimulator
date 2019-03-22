using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
public class OptionPanel : MonoBehaviour {
	public OptionButton prefab;
	ChatOptionNode _optionNode;
	List<OptionButton> buttonLst = new List<OptionButton>();
	// Use this for initialization
	void Awake () {
		
	}

	// Update is called once per frame
	void Update () {
		 _optionNode = ChatManager.Instance.TryGetOptionNode ();
		if (_optionNode == null||_optionNode.option==-2) {
			Show(0);
			return;
		}
		Show (_optionNode.labels.Count);
	}
	void Show(int num)
	{
		if (num < buttonLst.Count) {
			for (int i = num; i < buttonLst.Count; i++) {
				buttonLst [i].gameObject.SetActive (false);
			}
		}
		for (int i = 0; i < num; i++) {
			if (i >= buttonLst.Count) {
				OptionButton b = GameObject.Instantiate (prefab);
				b.gameObject.SetActive (true);
				b.transform.SetParent (prefab.transform.parent);
				b.transform.localScale = Vector3.one;
				b.AddListener (i);
				buttonLst.Add (b);
			}
			buttonLst [i].gameObject.SetActive (true);
			buttonLst [i].SetText (_optionNode.labels[i]);
		}
	}
}
