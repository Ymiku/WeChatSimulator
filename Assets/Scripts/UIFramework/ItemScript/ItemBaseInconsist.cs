using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseInconsist : MonoBehaviour
{
    public Vector2 pos
    {
        get
        {
            return cachedRectTransform.anchoredPosition;
        }
    }
    public float width
    {
        get
        {
            return cachedRectTransform.sizeDelta.x;
        }
    }
    public float height
    {
        get
        {
            return cachedRectTransform.sizeDelta.y;
        }
    }
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

    public int id;
    private bool _initFlag;
    public virtual float SetData(object o)
    {
        if (!_initFlag)
        {
            Init();
            _initFlag = true;
        }
        return 0.0f;
    }
    public virtual void Init()
    {
    }
    public GameObject FindChild(string path)
    {
        return Utils.FindChild(gameObject, path);
    }
    public T FindInChild<T>(string path) where T : Component
    {
        return Utils.FindInChild<T>(gameObject, path);
    }
}
