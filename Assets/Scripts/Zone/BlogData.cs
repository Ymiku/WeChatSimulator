using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlogData {
	public int id;
	public WeakReference txtRef;
	public TextAsset txt{
		get{
			TextAsset s = txtRef as object as TextAsset;
			if (s != null)
				return s;
			s = Resources.Load <TextAsset>("");
			txtRef = new WeakReference (s,false);
			return s;
		}
	}
}
