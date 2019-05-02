using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : Singleton<ZoneManager> {
	public Dictionary<int,List<AlbumData>> id2Album = new Dictionary<int, List<AlbumData>>();
	public AlbumPic[] picArray;
	public Dictionary<int,List<BlogData>> id2Blog = new Dictionary<int, List<BlogData>>();
	public Dictionary<int,List<TweetData>> id2Tweet = new Dictionary<int, List<TweetData>>();
    public Dictionary<int, List<CommentData>> id2Comment = new Dictionary<int, List<CommentData>>();
    public AlbumData albumData;
	public void LoadData()
	{
        for (int i = 0; i < XMLSaver.saveData.comments.Count; i++)
        {
            CommentData comment = XMLSaver.saveData.comments[i];
            List<CommentData> comments;
            if (!id2Comment.TryGetValue(comment.userId, out comments))
            {
                comments = new List<CommentData>();
                id2Comment.Add(comment.userId, comments);
            }
            comments.Insert(0,comment);
        }
	}
}
