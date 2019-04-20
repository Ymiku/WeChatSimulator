using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AlbumPic{
	public int id;
	public int order;
	public int userId;
	public string picPath;
	public WeakReference picRef;
	public Sprite pic{
		get{
			Sprite s = picRef as object as Sprite;
			if (s != null)
				return s;
			s = Resources.Load <Sprite>("Album/"+picPath);
			picRef = new WeakReference (s,false);
			return s;
		}
	}
	public System.DateTime time;
	public string place;
	public string note;
}
public class AlbumData
{
    public bool isSecret;
	public string albumName = "";
	public List<AlbumPic> pics = new List<AlbumPic> ();
}
