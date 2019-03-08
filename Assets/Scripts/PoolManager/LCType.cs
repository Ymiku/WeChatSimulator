using UnityEngine;
using System.Collections;

public class LCType:IPoolable{

	public virtual void Destroy()
	{
		if (PoolManager.Instance == null)
			return;
		if (!PoolManager.Instance.IsPoolFull(GetType()))
		{
			PoolManager.Instance.PutPoolObject(GetType(), this);
		}
	}
}
