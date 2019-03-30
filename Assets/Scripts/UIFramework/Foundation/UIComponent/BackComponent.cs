using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;
[RequireComponent(typeof(Button))]
public class BackComponent : MonoBehaviour {

	void Awake () {
		GetComponent<Button> ().onClick.AddListener (OnClick);
	}
	
	void OnClick()
	{
        GetComponentInParent<BaseView>().PopCallBack();
	}
}
