using UnityEngine;
using System.Collections;

public class DontDestoryOnLoad : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

}
