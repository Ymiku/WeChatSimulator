using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonProxy : Button {
    TextProxy _text;
    public TextProxy text
    {
        get { if (_text == null) _text = GetComponentInChildren<TextProxy>();return _text; }
    }
	public string info {
        get { if (text == null) return "";return text.text; }
        set { if (text == null) return; text.text = value; }
    }

    public Vector2 anchoredPosition
    {
        get
        {
            return cachedRectTransform.anchoredPosition;
        }
        set
        {
            cachedRectTransform.anchoredPosition = value;
        }
    }
    public float width
    {
        get
        {
            return cachedRectTransform.sizeDelta.x;
        }
        set
        {
            cachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        }
    }
    public float height
    {
        get
        {
            return cachedRectTransform.sizeDelta.y;
        }
        set
        {
            cachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
        }
    }
    public Vector2 sizeDelta
    {
        get { return cachedRectTransform.sizeDelta; }
        set { cachedRectTransform.sizeDelta = value; }
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
}
