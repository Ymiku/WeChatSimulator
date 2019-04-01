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
    Stack<ItemBase> _pools = new Stack<ItemBase>();
#if UNITY_EDITOR
    [SerializeField]
#endif
    [HideInInspector]
    List<ItemBase> _activeItems = new List<ItemBase>();
    RectTransform viewPortTrans;
    RectTransform contextTrans;
    List<object> _datas = new List<object>();

    private void Test()
    {
        List<object> os = new List<object>();
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        os.Add(new object());
        Init(os);   
    }
    public void Init(List<object> datas)
    {
        _datas = datas;
        prefab.gameObject.SetActive(false);
        contextTrans = GetComponent<ScrollRect>().content;
        _activeItems.Clear();
        _pools.Clear();
        contextTrans.anchoredPosition = Vector2.zero;
        contextTrans.sizeDelta = new Vector2(contextTrans.sizeDelta.x, prefab.height*datas.Count);
    }
    void TryOpen()
    {
        if (_datas.Count == 0)
            return;
        ItemBase item = GetItem();
        item.SetData(_datas[0]);
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
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
        if (_activeItems[0].pos.y + contextTrans.anchoredPosition.y < 0 && TryAddUp())
            return true;
        if (_activeItems[_activeItems.Count - 1].pos.y - _activeItems[_activeItems.Count - 1].height + contextTrans.anchoredPosition.y > -cachedRectTransform.sizeDelta.y && TryAddDown())
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
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, -(downId+1)*prefab.height);
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
        item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, -(upId - 1) * prefab.height);
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
            t.SetParent(contextTrans);
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
            if (node.pos.y - node.height + contextTrans.anchoredPosition.y > 0 + buffer)
                return true;
        if (node.pos.y + contextTrans.anchoredPosition.y < -cachedRectTransform.sizeDelta.y - buffer)
            return true;
        return false;
    }
}
