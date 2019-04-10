using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public class PoolableFScrollView : MonoBehaviour {
	public NodeItemProxy[] prefabs;
	Stack<NodeItemProxy>[] _pools;
	#if UNITY_EDITOR
	[SerializeField]
	#endif
	List<NodeItemProxy> _activeItems = new List<NodeItemProxy>();
	RectTransform viewPortTrans;
	RectTransform contextTrans;
	// Use this for initialization
	public void Init () {
		for (int i = 0; i < prefabs.Length; i++) {
			prefabs [i].gameObject.SetActive (false);
            prefabs[i].prefabId = i;
		}
		viewPortTrans = GetComponent<FScrollRect> ().viewport;
		contextTrans = GetComponent<FScrollRect> ().content;
		_activeItems.Clear ();
		_pools = new Stack<NodeItemProxy>[prefabs.Length];
		for (int i = 0; i < _pools.Length; i++) {
			_pools [i] = new Stack<NodeItemProxy> ();
		}
        /*
		if (false&&contextTrans.sizeDelta.y < viewPortTrans.sizeDelta.y) {
			contextTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, viewPortTrans.sizeDelta.y);
			borrowHeight = viewPortTrans.sizeDelta.y-contextTrans.sizeDelta.y;
		}
        */
	}
    bool needUpdate = false;
    public void OnEnter()
    {
        needUpdate = true;
        contextTrans.anchoredPosition = Vector2.zero;
        contextTrans.sizeDelta = new Vector2(contextTrans.sizeDelta.x, ChatManager.Instance.curInstance.saveData.totalRectHeight);
    }
    public void OnExit()
    {
        needUpdate = false;
        for (int i = 0; i < _activeItems.Count; i++)
        {
            Pool(_activeItems[i]);
            i--;
        }
    }
    void TryOpen()
    {
        ChatInstance instance = ChatManager.Instance.curExecuteInstance;
        Node runningNode = instance.curRunningNode;
		Node nextReadNode = instance.nextReadNode;
		Node front = null;
		if (CanShow (nextReadNode, runningNode)) {
			front = nextReadNode;
		} else {
			front = instance.GetFront (nextReadNode,true);
		}
		if (front == null)
        {
            front = instance.curSection.GetLast();
        }
		if (front == null)
			return;
		NodeItemProxy item = GetItem (front.enname==ChatManager.Instance.curName?1:0);
        ChatManager.Instance.curExecuteInstance.ActiveNode(front);
		float itemHeight = item.SetData (front);
		item.cachedRectTransform.anchoredPosition = new Vector2 (0.0f,itemHeight-contextTrans.sizeDelta.y);
		if (front==nextReadNode) {
			instance.ReadNext ();
			ChatManager.Instance.curInstance.saveData.totalRectHeight += itemHeight;
			contextTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, contextTrans.sizeDelta.y + itemHeight);
			item.cachedRectTransform.anchoredPosition = new Vector2 (0.0f, itemHeight - contextTrans.sizeDelta.y);
		}
		_activeItems.Add (item);
	}
	bool CanShow(Node readNode,Node curNode)
	{
		if (curNode == null)
			return true;
		if (curNode.sectionId >= readNode.sectionId && curNode.nodeId > readNode.nodeId)
			return true;
		return false;
	}
	float borrowHeight = 0.0f;
	// Update is called once per frame
	void AddActiveNodeY(float h)
	{
		for (int i = 0; i < _activeItems.Count; i++) {
			_activeItems [i].cachedRectTransform.anchoredPosition += new Vector2 (0.0f,h);
		}
	}
	void Update () {
        if (!needUpdate)
            return;
        ChatManager.Instance.curExecuteInstance = ChatManager.Instance.curInstance;
        int i = 0;
		while (CheckBorder()&&i<100) {
			i++;
		}
		if (false&&borrowHeight >111120.0f&&contextTrans.sizeDelta.y > viewPortTrans.sizeDelta.y) {
			float a = contextTrans.sizeDelta.y - viewPortTrans.sizeDelta.y;
			if (a > borrowHeight) {
				contextTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,contextTrans.sizeDelta.y-borrowHeight);
				borrowHeight = 0.0f;
				AddActiveNodeY (borrowHeight);
			} else {
				contextTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,contextTrans.sizeDelta.y-a);
				borrowHeight -= a;
				AddActiveNodeY (a);
			}
		}
		/*
		if (size.y < viewPortTrans.sizeDelta.y) {
			borrowHeight = viewPortTrans.sizeDelta.y - size.y;
			size = new Vector2 (size.x, size.y + borrowHeight);
			contextTrans.anchoredPosition = Vector2.zero;
		} 

		contextTrans.sizeDelta = size;
		*/
	}
	bool CheckBorder()
	{
		if (_activeItems.Count == 0) {
			TryOpen ();
			return false;
		}
		if (_activeItems [0].pos.y + contextTrans.anchoredPosition.y < 0 && TryAddUp ())
			return true;
		if (_activeItems [_activeItems.Count - 1].pos.y - _activeItems [_activeItems.Count - 1].height + contextTrans.anchoredPosition.y > -viewPortTrans.sizeDelta.y && TryAddDown ())
			return true;
		if (_activeItems.Count <= 1)
			return false;
		if (NeedCull (_activeItems [0])) {
			Pool (_activeItems [0]);
			return true;
		}
		if (NeedCull (_activeItems [_activeItems.Count-1])) {
			Pool (_activeItems [_activeItems.Count-1]);
			return true;
		}

		return false;
	}
	bool TryAddDown()
	{
		Node linkedNode = _activeItems [_activeItems.Count - 1].linkedNode;
		Node down = ChatManager.Instance.curExecuteInstance.GetNext(linkedNode,true);
		if (down == null)
			return false;

		if (ChatManager.Instance.curExecuteInstance.curRunningNode == down)
			return false;
        NodeItemProxy item = GetItem (down.enname==ChatManager.Instance.curName?1:0);
		float itemHeight = item.SetData (down);
		float itemY = _activeItems [_activeItems.Count - 1].cachedRectTransform.anchoredPosition.y - _activeItems [_activeItems.Count - 1].height;
		if (ChatManager.Instance.curExecuteInstance.curSection == down) {
			ChatManager.Instance.curExecuteInstance.ReadNext ();
            ChatManager.Instance.curInstance.saveData.totalRectHeight += itemHeight;
			contextTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, contextTrans.sizeDelta.y + itemHeight);
		}
		item.cachedRectTransform.anchoredPosition = new Vector2 (0.0f,itemY);
        ChatManager.Instance.curExecuteInstance.ActiveNode(down);
        _activeItems.Add (item);
		return true;
	}
	bool TryAddUp ()
	{
		Node up = ChatManager.Instance.curExecuteInstance.GetFront(_activeItems[0].linkedNode,true);
		if (up == null)
			return false;
        NodeItemProxy item = GetItem (up.enname==ChatManager.Instance.curName?1:0);
		float itemHeight = item.SetData (up);
		float itemY = _activeItems [0].cachedRectTransform.anchoredPosition.y + itemHeight;
		item.cachedRectTransform.anchoredPosition = new Vector2 (0.0f,itemY);
		_activeItems.Insert (0,item);
        ChatManager.Instance.curExecuteInstance.ActiveNode(up);
		if (ChatManager.Instance.curExecuteInstance.curSection == up) {
			ChatManager.Instance.curExecuteInstance.ReadNext ();
            ChatManager.Instance.curInstance.saveData.totalRectHeight += itemHeight;
			contextTrans.anchoredPosition +=  new Vector2(0.0f, itemHeight);
			for (int i = 0; i < _activeItems.Count; i++) {
				_activeItems [i].cachedRectTransform.anchoredPosition -= new Vector2 (0.0f,itemHeight);
			}
			contextTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, contextTrans.sizeDelta.y + itemHeight);
		}
		return true;
	}
	NodeItemProxy GetItem(int index)
	{
		if (_pools [index].Count > 0)
			return _pools [index].Pop ();
		else {
			RectTransform t = GameObject.Instantiate (prefabs [index].cachedRectTransform);
			t.SetParent (contextTrans);
			t.localScale = Vector3.one;
			t.anchoredPosition3D = Vector3.zero;
			return t.GetComponent<NodeItemProxy> ();
		}
	}
	void Pool(NodeItemProxy node)
	{
		_activeItems.Remove (node);
		node.gameObject.SetActive (false);
		_pools [node.prefabId].Push (node);
        ChatManager.Instance.curExecuteInstance.DeactiveNode(node.linkedNode);
    }
	bool NeedCull(NodeItemProxy node)
	{
		float buffer = 100.0f;
		if(_activeItems.IndexOf(node)==0)
		if (node.pos.y - node.height + contextTrans.anchoredPosition.y > 0+buffer)
			return true;
		if (node.pos.y + contextTrans.anchoredPosition.y < -viewPortTrans.sizeDelta.y-buffer)
			return true;
		return false;
	}
}
