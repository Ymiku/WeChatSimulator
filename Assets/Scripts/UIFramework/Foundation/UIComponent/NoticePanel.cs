using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoticePanel : MonoBehaviour {
    public Text text;
    public CanvasGroup group;
    Stack<string> _noticeStack = new Stack<string>();
 
    public void AddNotice(string notice)
    {
        _noticeStack.Push(notice);
    }
    public void Close()
    {
        group.alpha = 0.0f;
        group.blocksRaycasts = false;
    }

}
