using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

[RequireComponent(typeof(ScrollRect))]
public class PoolableScrollView : MonoBehaviour
{
    RectTransform _cachedRectTransform;
    public RectTransform cachedRectTransform
    {
        get
        {
            if (_cachedRectTransform == null)
            {
                _cachedRectTransform = GetComponent<RectTransform>();
            }
            return _cachedRectTransform;
        }
    }
    public ItemBase prefab;
	float prefabHeight;
    Stack<ItemBase> _pools = new Stack<ItemBase>();
    public List<ItemBase> _activeItems = new List<ItemBase>();
    RectTransform _contextTrans;
    List<object> _datas = new List<object>();
	public RectTransform UpPanel;
	float constHeight = 0.0f;
    public void Init<T>(List<T> datas)
    {
        Init(datas.ConvertAll<object>(Data => Data as object));
    }
	public void Init(List<object> datas)
    {
		if (prefab.gameObject.activeSelf) {
			prefabHeight = prefab.height;
			prefab.gameObject.SetActive(false);
		}
        _datas = datas;
		if(UpPanel!=null)
			constHeight = UpPanel.sizeDelta.y;
        
        _contextTrans = GetComponent<ScrollRect>().content;
		while (_activeItems.Count!=0) {
			Pool (_activeItems[0]);
		}
        _contextTrans.anchoredPosition = Vector2.zero;
        _contextTrans.sizeDelta = new Vector2(_contextTrans.sizeDelta.x, prefabHeight * datas.Count+constHeight);
    }
    void TryOpen()
    {
        if (_datas.Count == 0)
            return;
        ItemBase item = GetItem();
		item.id = 0;
        item.SetData(_datas[0]);
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, 0.0f - constHeight);
        _activeItems.Add(item);
        item.gameObject.SetActive(true);
    }
    void Update()
    {
        int i = 0;
        while (CheckBorder() && i < 100)
        {
            i++;
        }
    }
    bool CheckBorder()
    {
        if (_activeItems.Count == 0)
        {
            TryOpen();
            return false;
        }
        if (_activeItems[0].pos.y + _contextTrans.anchoredPosition.y < 0 && TryAddUp())
            return true;
        if (_activeItems[_activeItems.Count - 1].pos.y - _activeItems[_activeItems.Count - 1].height + _contextTrans.anchoredPosition.y >
            -cachedRectTransform.sizeDelta.y && TryAddDown())
            return true;
        if (_activeItems.Count <= 1)
            return false;
        if (NeedCull(_activeItems[0]))
        {
            PoolUp(_activeItems[0]);
            return true;
        }
        if (NeedCull(_activeItems[_activeItems.Count - 1]))
        {
            PoolDown(_activeItems[_activeItems.Count - 1]);
            return true;
        }

        return false;
    }
    bool TryAddDown()
    {
        int downId = _activeItems[_activeItems.Count - 1].id;
        if (downId == _datas.Count - 1)
            return false;
        ItemBase item = GetItem();
        item.id = downId + 1;
        item.SetData(_datas[downId+1]);
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, -(downId+1)*prefabHeight-constHeight);
        _activeItems.Add(item);
        item.gameObject.SetActive(true);
        return true;
    }
    bool TryAddUp()
    {
        int upId = _activeItems[0].id;
        if (upId == 0)
            return false;
        ItemBase item = GetItem();
        item.id = upId - 1;
        item.SetData(_datas[upId-1]);
		item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, -(upId - 1) * prefabHeight-constHeight);
        _activeItems.Insert(0, item);
        item.gameObject.SetActive(true);
        return true;
    }
    void PoolUp(ItemBase node)
    {
        Pool(node);
    }
    void PoolDown(ItemBase node)
    {
        Pool(node);
        //contextTrans.sizeDelta = new Vector2 (contextTrans.sizeDelta.x,contextTrans.sizeDelta.y-node.height);
    }
    ItemBase GetItem()
    {
        if (_pools.Count > 0)
            return _pools.Pop();
        else
        {
            RectTransform t = GameObject.Instantiate(prefab.cachedRectTransform);
            t.SetParent(_contextTrans);
            t.localScale = Vector3.one;
            t.anchoredPosition3D = Vector3.zero;
            return t.GetComponent<ItemBase>();
        }
    }
    void Pool(ItemBase node)
    {
        _activeItems.Remove(node);
        node.gameObject.SetActive(false);
        _pools.Push(node);
    }
    bool NeedCull(ItemBase node)
    {
        float buffer = 100.0f;
        if (_activeItems.IndexOf(node) == 0)
            if (node.pos.y - node.height + _contextTrans.anchoredPosition.y > 0 + buffer)
                return true;
        if (node.pos.y + _contextTrans.anchoredPosition.y < -cachedRectTransform.sizeDelta.y - buffer)
            return true;
        return false;
    }
}
