using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using UnityEngine.UI;
public class NodeItemProxy : MonoBehaviour {
	public ContentSizeFitter fitter;
	public int padding = 20;
	public int prefabId;
	public Image avatar;
	public RectTransform root;

	public RectTransform backGround;
	public Text text;
	public Image image;
	public Vector2 pos{
		get
		{
			return cachedRectTransform.anchoredPosition;
		}	
	}
	public float width{
		get
		{ 
			return cachedRectTransform.sizeDelta.x;
		}
	}
	public float height{
		get
		{ 
			return cachedRectTransform.sizeDelta.y;
		}
	}
	RectTransform _cachedRectTransform;
	public RectTransform cachedRectTransform{
		get{
			if (_cachedRectTransform == null) {
				_cachedRectTransform = GetComponent<RectTransform> ();
			}
			return _cachedRectTransform;
		}
	}
	public Node linkedNode;
	public float SetData(Node node)
	{
		text.rectTransform.sizeDelta = new Vector2 (660.0f,800.0f);
		linkedNode = node;
		avatar.rectTransform.anchoredPosition = new Vector2 (0.0f,-padding);
		if (prefabId == 0) {
			HeadSpriteUtils.Instance.SetHead (avatar, ChatManager.Instance.curInstance.friendName);
		} else {
			HeadSpriteUtils.Instance.SetHead (avatar, ChatManager.Instance.curName);
		}
		root.anchoredPosition = new Vector2 (root.anchoredPosition.x, -padding);
		if (node is ChatNode) {
			text.enabled = true;
			image.enabled = false;
			text.text = (node as ChatNode).DialogLine;
			//text.rectTransform.sizeDelta = new Vector2 (text.preferredWidth, Mathf.Max(avatar.rectTransform.sizeDelta.y, text.preferredHeight + 20.0f));
			backGround.sizeDelta = new Vector2 (Mathf.Max(80.0f,text.preferredWidth+40.0f),Mathf.Max(avatar.rectTransform.sizeDelta.y,text.preferredHeight+40.0f));
            text.rectTransform.sizeDelta = new Vector2(backGround.sizeDelta.x-20.0f,backGround.sizeDelta.y);
        }
		if (node is ChatImageNode) {
			image.sprite = (node as ChatImageNode).CharacterPotrait;
			text.enabled = false;
			image.enabled = true;
			backGround.sizeDelta = new Vector2 (Mathf.Max(80.0f, image.rectTransform.sizeDelta.x+40.0f), Mathf.Max(avatar.rectTransform.sizeDelta.y, image.rectTransform.sizeDelta.y+20.0f));
		}
        if (node is ChatOptionNode)
        {
            text.enabled = true;
            image.enabled = false;
            text.text = (node as ChatOptionNode).labels[ChatManager.Instance.curInstance.saveData.GetOption(node)];
            //text.rectTransform.sizeDelta = new Vector2 (text.preferredWidth, Mathf.Max(avatar.rectTransform.sizeDelta.y, text.preferredHeight + 20.0f));
            backGround.sizeDelta = new Vector2(Mathf.Max(80.0f, text.preferredWidth + 40.0f), Mathf.Max(avatar.rectTransform.sizeDelta.y, text.preferredHeight + 40.0f));
            text.rectTransform.sizeDelta = new Vector2(backGround.sizeDelta.x - 20.0f, backGround.sizeDelta.y);
        }
        if (node is RootGraphNode)
        {
            text.enabled = true;
            image.enabled = false;
            text.text = "我们已经是好友啦，一起来聊天吧！";
            //text.rectTransform.sizeDelta = new Vector2 (text.preferredWidth, Mathf.Max(avatar.rectTransform.sizeDelta.y, text.preferredHeight + 20.0f));
            Debug.Log(text.preferredHeight);
            backGround.sizeDelta = new Vector2(Mathf.Max(80.0f, text.preferredWidth + 40.0f), Mathf.Max(avatar.rectTransform.sizeDelta.y, text.preferredHeight + 40.0f));
            text.rectTransform.sizeDelta = new Vector2(backGround.sizeDelta.x - 20.0f, backGround.sizeDelta.y);
        }
		cachedRectTransform.sizeDelta = backGround.sizeDelta + new Vector2 (0.0f,padding*2.0f);
		gameObject.SetActive (true);
		return cachedRectTransform.sizeDelta.y;
	}
}
