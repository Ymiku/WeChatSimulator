using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;
[RequireComponent(typeof(Button))]
public class NoticeComponent : MonoBehaviour {
	public string notice;
	// Use this for initialization
	void Awake () {
		GetComponent<Button> ().onClick.AddListener (OnClick);
	}
	void OnClick()
	{
		UIManager.Instance.ShowNotice (notice);
	}
}
