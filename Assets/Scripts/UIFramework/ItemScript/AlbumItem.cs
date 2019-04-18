using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class AlbumItem : ItemBase {
    public ImageProxy pic;
    public TextProxy albumName;
    public TextProxy note;
    AlbumData album;
    public override void SetData (object o)
	{
		base.SetData (o);
		album = o as AlbumData;
        pic.sprite = album.pics[0].pic;
        albumName.text = album.albumName;
        note.text = album.pics.Count.ToString() + "张 ";
        if (album.isSecret)
            note.text = note.text + "私密";

	}
	public void OnClick()
	{
	}
}