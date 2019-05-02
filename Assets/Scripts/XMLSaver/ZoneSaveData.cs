using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class SaveData
{
    public List<CommentData> comments = new List<CommentData>();
    public void AddComment(CommentType type,int targetId,string context)
    {
        CommentData data = new CommentData();
        data.userId = GameManager.Instance.curUserId;
        data.targetId = targetId;
        data.commentType = type;
        data.info = context;
        data.order = int.MaxValue;
        comments.Add(data);
        List<CommentData> datas = null;
        if (!ZoneManager.Instance.id2Comment.TryGetValue(data.userId,out datas))
        {
            datas = new List<CommentData>();
            ZoneManager.Instance.id2Comment.Add(data.userId,datas);
        }
        datas.Add(data);
    }
}
