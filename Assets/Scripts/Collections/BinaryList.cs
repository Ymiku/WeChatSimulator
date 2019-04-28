using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BinaryList<T>
    where T:IComparable<T>
{
    public bool isDescend = true;//降序
	public int Count{
		get{ return list.Count;}
	}
    public List<T> orderedList{
        get { return list; }
    }
    List<T> list = new List<T>();
    public void Add(T t)
    {
        int start = 0;
        int end = list.Count-1;
        if (list.Count == 0)
        {
            list.Add(t);
            return;
        }
        Add(t,start,end);
    }
    void Add(T t, int start, int end)
    {
        if (start == end)
        {
            if (isDescend)
            {
                if (t.CompareTo(list[start]) >= 0)
                {
                    list.Insert(start, t);
                    return;
                }
                else
                {
                    list.Insert(start + 1, t);
                    return;
                }
            }
            else
            {
                if (t.CompareTo(list[start]) <= 0)
                {
                    list.Insert(start, t);
                    return;
                }
                else
                {
                    list.Insert(start + 1, t);
                    return;
                }
            }
        }
        if (isDescend)
        {
            if (t.CompareTo(list[start]) >= 0)
            {
                list.Insert(start, t);
                return;
            }
            if (t.CompareTo(list[end]) < 0)
            {
                list.Insert(end, t);
                return;
            }
        }
        else
        {
            if (t.CompareTo(list[start]) <= 0)
            {
                list.Insert(start, t);
                return;
            }
            if (t.CompareTo(list[end]) > 0)
            {
                list.Insert(end, t);
                return;
            }
        }
        int half = start+((end-start) >> 1);
        if (isDescend)
        {
            if (t.CompareTo(list[half]) >= 0)
            {
                Add(t, start, half);
            }
            else
            {
                Add(t, half, end);
            }
        }
        else
        {
            if (t.CompareTo(list[half]) <= 0)
            {
                Add(t, start, half);
            }
            else
            {
                Add(t, half, end);
            }
        }
    }
    public void Clear()
    {
        list.Clear();
    }
}
