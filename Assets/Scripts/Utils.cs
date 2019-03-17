using System;
using UIFrameWork;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// 点击首页事件
    /// </summary>
    public static void OnClickHone()
    {
        UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
        UIManager.Instance.Push(new HomeContext());
    }

    /// <summary>
    /// 点击财富事件
    /// </summary>
    public static void OnClickFortune() {
        UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
        UIManager.Instance.Push(new FortuneContext());
    }

    /// <summary>
    /// 点击口碑事件
    /// </summary>
    public static void OnClickKoubei() {
        UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
        UIManager.Instance.Push(new KoubeiContext());
    }

    /// <summary>
    /// 点击朋友事件
    /// </summary>
    public static void OnClickFriends() {
        UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
        UIManager.Instance.Push(new FriendsContext());
    }

    /// <summary>
    /// 点击我的事件
    /// </summary>
    public static void OnClickMe()
    {
        UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
        UIManager.Instance.Push(new MeContext());
    }
}
