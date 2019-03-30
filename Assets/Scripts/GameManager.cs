using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : UnitySingleton<GameManager>
{
    long _timeStamp;
    float _localTime;
	public int curUserId;
    public string curAccountName = "bull";
    public long time
    {
        get { return _timeStamp + (long)_localTime; }
    }
    public int localTime
    {
        get { return (int)_localTime; }
    }
    public override void Awake()
    {
        base.Awake();
        OnEnterGame();
    }
	public void SetUser(int userId)
	{
		curUserId = userId;
		XMLSaver.saveData.lastUser = userId;
		if (!XMLSaver.saveData.canLoginUserIds.Contains (userId))
			XMLSaver.saveData.canLoginUserIds.Add (userId);
	}
    void OnEnterGame()
    {
        //load
        StaticDataLoader.Load();
        XMLSaver.Load();
        _timeStamp = GetTimeStamp();
        _localTime = 0.0f;
    }
	void OnDestroy()
	{
		OnExitGame ();
	}
    void OnExitGame()
    {
        XMLSaver.Save();
    }
    void Update()
    {
        _localTime += Time.deltaTime;
        ChatManager.Instance.OnExcute();
        FrostRX.Instance.Execute();
    }
    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(2018, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }
	public void ClearData()
	{
		XMLSaver.saveData = new SaveData ();
	}
}
