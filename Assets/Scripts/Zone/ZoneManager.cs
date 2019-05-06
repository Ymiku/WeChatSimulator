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
            int targetUserId = GetCommentTargetId(comment);
            List<CommentData> comments;
            if (!id2Comment.TryGetValue(targetUserId, out comments))
            {
                comments = new List<CommentData>();
                id2Comment.Add(targetUserId, comments);
            }
            comments.Insert(0,comment);
        }
	}
    public void AddComment(CommentType type, int targetId, string context)
    {
        List<CommentData> comments = XMLSaver.saveData.comments;
        if (context == null)
        {
            for (int i = 0; i < comments.Count; i++)
            {
                if (comments[i].targetId == targetId && comments[i].commentType == type && comments[i].userId == GameManager.Instance.curUserId && comments[i].info == null)
                    return;
            }
        }
        CommentData data = new CommentData();
        data.userId = GameManager.Instance.curUserId;
        data.targetId = targetId;
        data.commentType = type;
        data.info = context;
        data.order = int.MaxValue;
        comments.Add(data);
        List<CommentData> datas = null;
        int targetUserId = GetCommentTargetId(data);
        if (!ZoneManager.Instance.id2Comment.TryGetValue(targetUserId, out datas))
        {
            datas = new List<CommentData>();
            ZoneManager.Instance.id2Comment.Add(targetUserId, datas);
        }
        datas.Add(data);
    }
    public int GetCommentTargetId(CommentData comment)
    {
        switch (comment.commentType)
        {
            case CommentType.Tweet:
                foreach (var item in id2Tweet.Values)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item[i].id == comment.targetId)
                            return item[i].userId;
                    }
                }
                break;
            case CommentType.Album:
                foreach (var item in id2Album.Values)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item[i].pics[0].id == comment.targetId)
                            return item[i].pics[0].userId;
                    }
                }
                break;
            case CommentType.Blog:
                foreach (var item in id2Blog.Values)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item[i].id == comment.targetId)
                            return item[i].userId;
                    }
                }
                break;
            default:
                Debug.LogError("error");
                break;
        }
        return -1;
    }
}
