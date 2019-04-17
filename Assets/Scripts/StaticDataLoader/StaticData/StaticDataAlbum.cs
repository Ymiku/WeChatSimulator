using static_data;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDataAlbum
{
	public static ALBUM_ARRAY Info;

	public static void Init()
	{
		Info = StaticDataLoader.ReadOneDataConfig<ALBUM_ARRAY>("album");
		TransToAlbumData ();
	}
		
	public static void TransToAlbumData()
	{
		ZoneManager.Instance.picArray = new AlbumPic[Info.items.Count];
		for (int i = 0; i < Info.items.Count; i++)
		{
			ALBUM a = Info.items [i];
			AlbumPic pic = new AlbumPic ();
			pic.id = a.pic_id;
			pic.userId = a.user_id;
			pic.picPath = a.pic_path;
			List<AlbumData> albums;
			if (ZoneManager.Instance.id2Album.TryGetValue (a.user_id,out albums)) {
			
			} else {
				albums = new List<AlbumData> ();
				ZoneManager.Instance.id2Album.Add (a.user_id,albums);
			}
			AlbumData datas = null;
			for (int m = 0; m < albums.Count; m++) {
				if(albums[m].albumName.Equals(a.album_name))
				{
					datas = albums[m];
				}
			}
			if(datas==null)
			{
				datas = new AlbumData();
				albums.Add(datas);
			}
			datas.pics.Add(pic);
			ZoneManager.Instance.picArray [i] = pic;
		}
	}
}
