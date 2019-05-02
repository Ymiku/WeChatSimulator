using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class SaveData
{
    public List<CommentData> comments = new List<CommentData>();
    public void AddComment(CommentType type,string context)
    {
        CommentData data = new CommentData();
        data.userId = GameManager.Instance.curUserId;
        data.commentType = type;
        data.info = context;
        data.order = int.MaxValue;
        comments.Add(data);
    }
}
