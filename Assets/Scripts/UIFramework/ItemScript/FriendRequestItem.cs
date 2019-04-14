using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequestItem : ItemBase
{
    public ImageProxy head;
    public TextProxy nickName;
    public TextProxy realName;
    public TextProxy info;
    AccountSaveData data;
    int request;
    public GameObject sended;
    public GameObject accept;
    public GameObject accpected;
    public override void SetData(object o)
    {
        base.SetData(o);
        int i = (int)o;
        request = i;
        if ((i & 255) == GameManager.Instance.curUserId)
        {
            data = XMLSaver.saveData.GetAccountData((i>>8)&255);
            sended.SetActive(false);
            if ((i >> 16) != 0)
            {
                accpected.SetActive(true);
                accept.SetActive(false);
            }
            else
            {
                accept.SetActive(true);
                accpected.SetActive(false);
            }
        }
        else
        {
            data = XMLSaver.saveData.GetAccountData(i&255);
            accept.SetActive(false);
            if ((i >> 16) != 0)
            {
                accpected.SetActive(true);
                sended.SetActive(false);
            }
            else
            {
                sended.SetActive(true);
                accpected.SetActive(false);
            }
        }
        HeadSpriteUtils.Instance.SetHead(head, data.accountId);
        if (string.IsNullOrEmpty(data.nickname) && string.IsNullOrEmpty(data.realName))
        {
            nickName.text = ContentHelper.Read(ContentHelper.NotSetNickName);
        }
        else
        {
            if (!string.IsNullOrEmpty(data.nickname))
            {
                nickName.text = data.nickname;
            }
            else
            {
                nickName.text = data.realName;
            }
        }
        if (!string.IsNullOrEmpty(data.realName))
        {
            realName.text = ContentHelper.Read(ContentHelper.RealName) + ":" + data.realName;
        }
        else
        {
            realName.text = ContentHelper.Read(ContentHelper.NoCertification);
        }
        info.text = ContentHelper.Read(ContentHelper.Iam)+(nickName.text.Equals(ContentHelper.Read(ContentHelper.NotSetNickName))?"":nickName.text);
    }
    public void OnClickAccept()
    {
        ChatManager.Instance.HandleRequest(request);
        SetData(request+(1<<16));
    }
}
