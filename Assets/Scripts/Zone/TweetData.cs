using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TweetData :IComparable<TweetData>{
	public int id;
	public int order;
	public int userId;
	public string info;
	public int[] pics;
	public string location;
	public bool isSecret;
    public int CompareTo(TweetData t)
    {
        return order.CompareTo(t.order);
    }
}
