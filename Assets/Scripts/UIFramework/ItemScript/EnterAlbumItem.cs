using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class EnterAlbumItem : ItemBase {
    public ImageProxy pic;
	AlbumPic apic;
	public AlbumData album;
    public override void SetData (object o)
	{
		base.SetData (o);
		apic = o as AlbumPic;
        pic.sprite = apic.pic;
        
	}
	public void OnClick()
	{
		UIManager.Instance.Push(new AlbumPicContext(){index = album.pics.IndexOf(this.apic),album = this.album});
	}
}