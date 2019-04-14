using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class ChatItem : ItemBase {
	public ImageProxy head;
	public TextProxy userName;
	public TextProxy message;
	public GameObject redPoint;
	public TextProxy redCount;
    ChatInstance instance;
	public override void SetData (object o)
	{
		base.SetData (o);
        instance = o as ChatInstance;
        AccountSaveData data = XMLSaver.saveData.GetAccountData(instance.friendName);
        HeadSpriteUtils.Instance.SetHead(head,data.accountId);
        if (!string.IsNullOrEmpty(data.nickname))
            userName.text = data.nickname;
        else if (!string.IsNullOrEmpty(data.realName))
            userName.text = data.realName;
        else
            userName.text = ContentHelper.Read(ContentHelper.NotSetNickName);
        OnRefresh();
    }
    public void OnRefresh()
    {
        message.text = instance.lastSentence;
        if (instance.redNum == 0)
        {
            redPoint.SetActive(false);
        }
        else
        {
            redPoint.SetActive(true);
            redCount.text = instance.redNum.ToString();
        }
    }
    public void OnClick()
    {
        ChatManager.Instance.EnterChat(instance.friendName);
        UIManager.Instance.Push(new ChatContext() { friendName = instance.friendName});
    }
}
