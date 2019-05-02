using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum CommentType
{
    Tweet = 0,
    Album = 1,
    Blog = 2,
}
public class CommentData : IComparable<CommentData>
{
    public int id;
    public int targetId;
    public int order;
    public int userId;
    public string info;
    public CommentType commentType;

    public int CompareTo(CommentData t)
    {
        return order.CompareTo(t.order);
    }
}
