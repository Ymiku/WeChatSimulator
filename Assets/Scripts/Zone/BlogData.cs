using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlogData {
	public int id;
	public int order;
	public int userId;
	public WeakReference txtRef;
	public string path;
	public TextAsset txt{
		get{
			TextAsset s = txtRef as object as TextAsset;
			if (s != null)
				return s;
			s = Resources.Load <TextAsset>("blog/"+path);
			txtRef = new WeakReference (s,false);
			return s;
		}
	}
}
