using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PoolManager : UnitySingleton<PoolManager> {
	public Transform root;
	public static Dictionary<Type, UnityEngine.Object> prefabsPoolDic = new Dictionary<Type, UnityEngine.Object>();
	public static Dictionary<Type, Stack<IPoolable>> ObjectPoolDic = new Dictionary<Type, Stack<IPoolable>>();
	public static Dictionary<Type, int> ObjectPoolSizeDic = new Dictionary<Type,int>();
	public static Dictionary<Type, int> ObjectPoolCollectDic = new Dictionary<Type,int>();
	public float collectTime =30f;
	private float _time = 0;
	private List<Type> typeList= new List<Type>();
	public override void Awake(){
		base.Awake ();
	}
	void Start () {
		if (collectTime <= 20f)
			Debug.Log ("pool's collection will perform terrible with such a short time!");
	}
	void Update()
	{
		_time += Time.deltaTime;
		if (_time >= collectTime) {
			PoolCollection ();
			_time = 0;
		}
	}
	public GameObject Instantiate(Type type,string path)
	{
		if (!prefabsPoolDic.ContainsKey (type)) {
			prefabsPoolDic [type] = Resources.Load (path);
		}
		return Instantiate (prefabsPoolDic [type]) as GameObject;
	}
	public void RemovePrefabs(Type type)
	{
		if (prefabsPoolDic.ContainsKey (type)) {
			prefabsPoolDic.Remove (type);
		}
	}
	public bool CheckOrRegistType(Type type)
	{
		if (ObjectPoolDic.ContainsKey (type)) {
			return ObjectPoolDic[type].Count > 0;
		} else {
			RegistPoolableType (type,0);
			return false;
		}
	}

	public void RegistPoolableType(Type type, int poolSize)
	{
		if (!ObjectPoolDic.ContainsKey(type))
		{
			ObjectPoolDic[type] = new Stack<IPoolable>();
			ObjectPoolSizeDic[type] = poolSize;
			if (poolSize == 0) {
				ObjectPoolCollectDic [type] = 1;
				typeList.Add (type);
			}
		}
	}

	public bool HasPoolObject(Type type)
	{
		return ObjectPoolDic.ContainsKey(type) && ObjectPoolDic[type].Count > 0;
	}
	
	public bool IsPoolFull(Type type)
	{
		if (!ObjectPoolDic.ContainsKey(type))
			return true;
		else if (ObjectPoolDic[type].Count >= ObjectPoolSizeDic[type]&&ObjectPoolSizeDic[type]!=0)
			return true;
		return false;
	}
	
	public IPoolable TakePoolObject(Type type)
	{
		if (ObjectPoolDic.ContainsKey(type) && ObjectPoolDic[type].Count > 0)
		{
			if (ObjectPoolSizeDic [type] == 0)
				ObjectPoolCollectDic [type]++;
			return ObjectPoolDic[type].Pop();
		}
		else
		{
			return null;
		}
	}
	
	public bool PutPoolObject(Type type, IPoolable obj)
	{
		if (!ObjectPoolDic.ContainsKey(type) || (ObjectPoolDic[type].Count >= ObjectPoolSizeDic[type]&&ObjectPoolSizeDic[type]!=0))
		{
			if(obj is MonoBehaviour)
			GameObject.Destroy((obj as MonoBehaviour).gameObject);
			return false;
		}
		else
		{
			if (obj is MonoBehaviour) {
				(obj as MonoBehaviour).gameObject.SetActive (false);
				(obj as MonoBehaviour).transform.parent = transform;
			}
			ObjectPoolDic[type].Push(obj);
			return true;
		}
	}

	public void PoolCollection()
	{
		object obj;
		float f;
		int coll;
		Type t;
		for (int i = 0; i < typeList.Count; i++) {
			t= typeList[i];
			coll = ObjectPoolCollectDic [t];
			f = ObjectPoolDic [t].Count / coll;
			if (f>=0.5f) {
				Drop (t,ObjectPoolDic [t].Count>>1);
			}
			ObjectPoolCollectDic[t] = (coll>>1)+1;
		}
	}
	public void Drop(Type t,int num)
	{
		if (!(t is MonoBehaviour))
			return;
		object obj;
		for (int i = 0; i < num; i++) {
			obj = TakePoolObject (t);
			if (obj == null)
				return;
			GameObject.Destroy ((obj as MonoBehaviour).gameObject);
		}
	}
}
