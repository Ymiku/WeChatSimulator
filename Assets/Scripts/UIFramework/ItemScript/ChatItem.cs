using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatItem : ItemBase {
	public ImageProxy head;
	public TextProxy name;
	public TextProxy message;
	public GameObject redPoint;
	public TextProxy redCount;
	public override void SetData (object o)
	{
		base.SetData (o);
	}
}
