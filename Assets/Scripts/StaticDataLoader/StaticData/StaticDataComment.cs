using static_data;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDataComment
{
    public static COMMENT_ARRAY Info;

    public static void Init()
    {
        Info = StaticDataLoader.ReadOneDataConfig<COMMENT_ARRAY>("Comment");
        TransToCommentData();
    }

    public static void TransToCommentData()
    {
        for (int i = 0; i < Info.items.Count; i++)
        {
            COMMENT a = Info.items[i];
            CommentData comment = new CommentData();
            comment.id = a.comment_id;
            comment.targetId = 0;
            comment.userId = a.user_id;
            comment.info = a.comment_info;
            comment.order = a.order;
            comment.commentType = (CommentType)a.target_type;

            List<CommentData> comments;
            int targetUserId = ZoneManager.Instance.GetCommentTargetId(comment);
            if (!ZoneManager.Instance.id2Comment.TryGetValue(targetUserId, out comments))
            {
                comments = new List<CommentData>();
                ZoneManager.Instance.id2Comment.Add(targetUserId, comments);
            }
            comments.Add(comment);
        }
    }
}
