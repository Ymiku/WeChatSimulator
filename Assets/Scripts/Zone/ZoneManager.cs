using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : Singleton<ZoneManager> {
	public Dictionary<int,List<AlbumData>> id2Album = new Dictionary<int, List<AlbumData>>();
	public AlbumPic[] picArray;
	public Dictionary<int,List<BlogData>> id2Blog = new Dictionary<int, List<BlogData>>();
	public Dictionary<int,List<TweetData>> id2Tweet = new Dictionary<int, List<TweetData>>();
	public AlbumData albumData;
	public void LoadData()
	{
		
	}
}
