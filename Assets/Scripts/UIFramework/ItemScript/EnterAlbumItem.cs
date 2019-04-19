using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class EnterAlbumItem : ItemBase {
    public ImageProxy pic;
	AlbumPic apic;
    public override void SetData (object o)
	{
		base.SetData (o);
		apic = o as AlbumPic;
        pic.sprite = apic.pic;
        
	}
	public void OnClick()
	{
		UIManager.Instance.Push(new AlbumPicContext(){pic = this.apic});
	}
}