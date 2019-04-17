using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class AlbumItem : ItemBase {
	public override void SetData (object o)
	{
		base.SetData (o);
		AlbumData album = o as AlbumData;

	}
	public void OnClick()
	{
	}
}