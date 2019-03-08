using UnityEngine;
using System.Collections;

public class LCTimeLimit : LCGameObject {
	private float _lifeTime;
	private float _timeCount;
	public virtual void OnEnable()
	{
		_timeCount = 0f;
	}
	// Update is called once per frame
	public virtual void Update () {
		if (_timeCount >= _lifeTime)
			Bomb ();
		_timeCount += Time.deltaTime;
	}
	public virtual void Bomb()
	{
		Destroy ();
	}
	public void SetLifeTime(float f)
	{
		_lifeTime = f;
	}
}
