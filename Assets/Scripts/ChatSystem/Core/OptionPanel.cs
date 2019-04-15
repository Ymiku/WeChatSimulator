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
    public PoolableFScrollView scroll;
    public FScrollRect fscroll;
    public CanvasGroup optionGroup;
    public RectTransform optionRect;
    public RectTransform viewRect;
	// Use this for initialization
	void Awake () {
		
	}
    private void OnEnable()
    {
        fscroll.OnClickEvent += Hide;
    }
    private void OnDisable()
    {
        fscroll.OnClickEvent -= Hide;
    }
    bool isShow = false;
    float count = 0.0f;
    public void Show()
    {
        isShow = true;
        optionGroup.blocksRaycasts = true;
        scroll.OnOptionPanelOpen();
    }
    public void Hide()
    {
        isShow = false;
        optionGroup.blocksRaycasts = false;
    }
    void RefreshLayoutByCount()
    {
        optionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,Mathf.Lerp(104.0f,400.0f,count));
        viewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,1585.0f-optionRect.sizeDelta.y+104.0f);
        optionGroup.alpha = count;
    }
    // Update is called once per frame
    void Update () {
        if (isShow)
        {
            if (count != 1.0f)
            {
                count = Mathf.Lerp(count, 1.1f, 8.0f * Time.deltaTime);
                if (count >= 1.0f)
                {
                    count = 1.0f;
                    
                }
            }
        }
        else
        {
            if (count != 0.0f)
            { 
                count = Mathf.Lerp(count, -0.1f, 8.0f * Time.deltaTime);
                if (count <= 0.0f)
                    count = 0.0f;
            }
        }
        RefreshLayoutByCount();
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
