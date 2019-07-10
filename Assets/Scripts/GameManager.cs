using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameManager : UnitySingleton<GameManager>
{
    long _timeStamp;
    float _localTime;
    bool _lastSecond;
    DateTime _curTime;
	public int curUserId;
	public string curEnName;
	Image loading;
    public long time
    {
        get { return _timeStamp + (long)_localTime; }
    }
    public int localTime
    {
        get { return (int)_localTime; }
    }
    [NonSerialized]
    public AccountSaveData accountData = null;
    public override void Awake()
    {
        base.Awake();
        curUserId = -1;
		loading = transform.Find ("AlwaysFront/Loading").GetComponent<Image>();
        OnEnterGame();
    }
	public void SetUser(int userId)
	{
		curUserId = userId;
		accountData = XMLSaver.saveData.GetAccountData(userId);
		curEnName = accountData.enname;
		XMLSaver.saveData.lastUser = userId;
        if (!XMLSaver.saveData.canLoginUserIds.Contains (userId))
			XMLSaver.saveData.canLoginUserIds.Add (userId);
        ChatManager.Instance.OnFriendsLstChange();
        AssetsManager.Instance.SaveOfflineTime();
        AssetsManager.Instance.Set(userId);
        FortuneManager.Instance.Set(userId);
	}
    void OnEnterGame()
    {
        //load
        XMLSaver.Load();
        StaticDataLoader.Load();
        ZoneManager.Instance.LoadData();
        gameObject.AddComponent<HeadSpriteUtils> ();
        _timeStamp = GetTimeStamp();
        _localTime = 0.0f;
        _lastSecond = false;
        _curTime = DateTime.Now;
		FrostRX.Start (this).
		Execute(()=>{loading.gameObject.SetActive(false);}).
		Wait(4.0f).
		Execute(()=>{loading.gameObject.SetActive(true);loading.SetAlpha(0.0f);}).
		AlphaFade(loading,1.0f,8.0f).
		ExecuteAfterTime (() => {OnSaveData ();}, 2.0f).
		Wait(2.0f).
		AlphaFade(loading,0.0f,8.0f).
		GoToBegin ();
    }
	void OnDestroy()
	{
		
	}
    void OnSaveData()
    {
        AssetsManager.Instance.SaveOfflineTime();
        XMLSaver.Save();
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
            AssetsManager.Instance.CheckAutoRepayAntOnLine();
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
