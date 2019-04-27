using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

[RequireComponent(typeof(ScrollRect))]
public class PoolableScrollViewInconsist : MonoBehaviour
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
    public ItemBaseInconsist prefab;
    float prefabHeight;
    Stack<ItemBaseInconsist> _pools = new Stack<ItemBaseInconsist>();
    public List<ItemBaseInconsist> _activeItems = new List<ItemBaseInconsist>();
    RectTransform _contextTrans;
    List<object> _datas = new List<object>();
    public RectTransform UpPanel;
    float constHeight = 0.0f;
    bool hasInit = false;
    public void SetDatas<T>(List<T> datas)
    {
        if (hasInit)
        {
            Refresh(datas.ConvertAll<object>(Data => Data as object));
        }
        else
        {
            Init(datas.ConvertAll<object>(Data => Data as object));
            hasInit = true;
        }
    }
    public void Reset()
    {
        while (_activeItems.Count != 0)
        {
            Pool(_activeItems[0]);
        }
        _contextTrans.anchoredPosition = Vector2.zero;
    }
    private void Init(List<object> datas)
    {
        if (prefab.gameObject.activeSelf)
        {
            prefabHeight = prefab.height;
            prefab.gameObject.SetActive(false);
        }
        _datas = datas;
        if (UpPanel != null)
            constHeight = UpPanel.sizeDelta.y;

        _contextTrans = GetComponent<ScrollRect>().content;
        while (_activeItems.Count != 0)
        {
            Pool(_activeItems[0]);
        }
        _contextTrans.anchoredPosition = Vector2.zero;
        _contextTrans.sizeDelta = new Vector2(_contextTrans.sizeDelta.x, constHeight);
    }
    private void Refresh(List<object> datas)
    {
        _datas = datas;
        _contextTrans.sizeDelta = new Vector2(_contextTrans.sizeDelta.x, constHeight);
        for (int i = 0; i < _activeItems.Count; i++)
        {
            _activeItems[i].SetData(_datas[_activeItems[i].id]);
        }
    }
    void TryOpen()
    {
        if (_datas.Count == 0)
            return;
        ItemBaseInconsist item = GetItem();
        item.id = 0;
        float h = item.SetData(_datas[0]);
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, 0.0f - constHeight);
        _contextTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,_contextTrans.sizeDelta.y+h);
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
        if (_activeItems[0].anchoredPosition.y + _contextTrans.anchoredPosition.y < 0 && TryAddUp())
            return true;
        if (_activeItems[_activeItems.Count - 1].anchoredPosition.y - _activeItems[_activeItems.Count - 1].height + _contextTrans.anchoredPosition.y >
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
        ItemBaseInconsist item = GetItem();
        item.id = downId + 1;
        float h = item.SetData(_datas[downId + 1]);
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, _activeItems[downId].anchoredPosition.y - _activeItems[downId].height);
        if (_contextTrans.sizeDelta.y <= -item.anchoredPosition.y + h * 0.5f)
        {
            _contextTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _contextTrans.sizeDelta.y + h);
        }
        _activeItems.Add(item);
        item.gameObject.SetActive(true);
        return true;
    }
    bool TryAddUp()
    {
        int upId = _activeItems[0].id;
        if (upId == 0)
            return false;
        ItemBaseInconsist item = GetItem();
        item.id = upId - 1;
        float h = item.SetData(_datas[upId - 1]);
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, _activeItems[0].anchoredPosition.y+h);
        _activeItems.Insert(0, item);
        item.gameObject.SetActive(true);
        return true;
    }
    void PoolUp(ItemBaseInconsist node)
    {
        Pool(node);
    }
    void PoolDown(ItemBaseInconsist node)
    {
        Pool(node);
        //contextTrans.sizeDelta = new Vector2 (contextTrans.sizeDelta.x,contextTrans.sizeDelta.y-node.height);
    }
    ItemBaseInconsist GetItem()
    {
        if (_pools.Count > 0)
            return _pools.Pop();
        else
        {
            RectTransform t = GameObject.Instantiate(prefab.cachedRectTransform);
            t.SetParent(_contextTrans);
            t.localScale = Vector3.one;
            t.anchoredPosition3D = Vector3.zero;
            return t.GetComponent<ItemBaseInconsist>();
        }
    }
    void Pool(ItemBaseInconsist node)
    {
        _activeItems.Remove(node);
        node.gameObject.SetActive(false);
        _pools.Push(node);
    }
    bool NeedCull(ItemBaseInconsist node)
    {
        float buffer = 100.0f;
        if (_activeItems.IndexOf(node) == 0)
            if (node.anchoredPosition.y - node.height + _contextTrans.anchoredPosition.y > 0 + buffer)
                return true;
        if (node.anchoredPosition.y + _contextTrans.anchoredPosition.y < -cachedRectTransform.sizeDelta.y - buffer)
            return true;
        return false;
    }
}
