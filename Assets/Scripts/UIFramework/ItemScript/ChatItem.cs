using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatItem : ItemBase {
	public ImageProxy head;
	public TextProxy userName;
	public TextProxy message;
	public GameObject redPoint;
	public TextProxy redCount;
    ChatInstance instance;
	public override void SetData (object o)
	{
		base.SetData (o);
        instance = o as ChatInstance;
	}
    public void OnRefresh()
    {

    }
}
