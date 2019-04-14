using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : UnitySingleton<GameManager>
{
    long _timeStamp;
    float _localTime;
    bool _lastSecond;
    DateTime _curTime;
	public int curUserId;
	public string curEnName;
    public long time
    {
        get { return _timeStamp + (long)_localTime; }
    }
    public int localTime
    {
        get { return (int)_localTime; }
    }
    public AccountSaveData accountData;
    public override void Awake()
    {
        base.Awake();
        curUserId = -1;
        OnEnterGame();
    }
	public void SetUser(int userId)
	{
		curUserId = userId;
		curEnName = XMLSaver.saveData.GetAccountData (userId).enname;
		XMLSaver.saveData.lastUser = userId;
        accountData = XMLSaver.saveData.GetAccountData(userId);
        if (!XMLSaver.saveData.canLoginUserIds.Contains (userId))
			XMLSaver.saveData.canLoginUserIds.Add (userId);
        ChatManager.Instance.OnExit();
        ChatManager.Instance.OnEnter(XMLSaver.saveData.GetAccountData(userId).enname);
        AssetsManager.Instance.SaveOfflineTime();
        AssetsManager.Instance.Set(userId);
	}
    void OnEnterGame()
    {
        //load
        XMLSaver.Load();
        StaticDataLoader.Load();
		gameObject.AddComponent<HeadSpriteUtils> ();
        _timeStamp = GetTimeStamp();
        _localTime = 0.0f;
        _lastSecond = false;
        _curTime = DateTime.Now;
    }
	void OnDestroy()
	{
		OnExitGame ();
	}
    void OnExitGame()
    {
        AssetsManager.Instance.SaveOfflineTime();
        XMLSaver.Save();
		XMLSaver.saveData = null;
    }
    void Update()
    {
        _localTime += Time.deltaTime;
        StoryManager.Instance.Execute();
        ChatManager.Instance.OnExcute();
        FrostRX.Instance.Execute();
        TimeTick();
    }
    void TimeTick()
    {
        _curTime = DateTime.Now;
        if(_lastSecond && _curTime.Hour == 0)
        {
            AssetsManager.Instance.RecalculationOneDayProfit();
            _lastSecond = false;
        }
        if (_curTime.Hour == 23 && _curTime.Minute == 59 && _curTime.Second == 59)
            _lastSecond = true;
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
