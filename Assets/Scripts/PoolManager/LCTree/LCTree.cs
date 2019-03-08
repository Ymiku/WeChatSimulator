using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LCTree<T> : LCGameObject
	where T:LCTree<T>{
	public static Dictionary<Type,string> pathDic = new Dictionary<Type, string>()
	{
		{typeof(LCTree0),"Tree/Tree_5"}
	};

	public static T CreateObject()
	{
		if (PoolManager.Instance.HasPoolObject(typeof(T)))
		{
			T temp = PoolManager.Instance.TakePoolObject(typeof(T)) as T;
			return temp;
		}
		else
		{
			GameObject obj = Instantiate(Resources.Load(pathDic[typeof(T)])) as GameObject;
			obj.hideFlags = HideFlags.HideInHierarchy;
			return obj.GetComponent<T>();
		}
	}
}