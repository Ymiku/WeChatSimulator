using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Preload : MonoBehaviour {
	public CanvasGroup[] groups;
	int index = 1;
	bool hasShow = false;
	float waitTime = 0.0f;
	// Use this for initialization
	void Start () {
		for (int i = 1; i < groups.Length; i++) {
			groups [i].alpha = 0.0f;
		}
		SceneManager.LoadSceneAsync ("Main",LoadSceneMode.Additive);

	}
	public void OnClick()
	{
		
	}
	// Update is called once per frame
	void Update () {
		if (!hasShow) {
			groups [index].alpha = Mathf.Lerp (groups [index].alpha, 1.1f, 2.0f * Time.deltaTime);
			if (groups [index].alpha >= 1.0f) {
				waitTime += Time.deltaTime;
				if (waitTime >= 2.5f) {
					hasShow = true;
					waitTime = 0.0f;
				}
			}
		} else {
			groups [index].alpha = Mathf.Lerp (groups [index].alpha, -0.1f, 2.0f * Time.deltaTime);
			if (groups [index].alpha <= 0.0f) {
				hasShow = false;
				if (index == groups.Length - 1) {
					hasShow = true;
					index = 0;
				}else if(index == 0)
				{
					SceneManager.UnloadSceneAsync ("Preload");
				}
				else {
					index++;
				}
			}
		}
	}
}
