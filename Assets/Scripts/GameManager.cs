using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : UnitySingleton<GameManager>
{
    long _timeStamp;
    float _localTime;
    public string curAccountName = "bull";
    public long time
    {
        get { return _timeStamp + (long)_localTime; }
    }
    public int localTime
    {
        get { return (int)_localTime; }
    }
    void Awake()
    {
        OnEnterGame();

        FrostRX.Instance.EndRxById(FrostRX.Instance.StartRX().ExecuteAfterTime(() => {
            Debug.Log(1f);
        }, 1f).ExecuteAfterTime(() => {
            Debug.Log(2f);
        }, 1f).ExecuteAfterTime(() => {
            Debug.Log(3f);
        }, 1f).ExecuteWhen(() => {
            Debug.Log(4f);
        }, () => {
            return true;
        }).GoToBegin().GetId());

    }
    void OnEnterGame()
    {
        //load
        XMLSaver.Load();
        _timeStamp = GetTimeStamp();
        _localTime = 0.0f;
    }
	void OnDestory()
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

}
