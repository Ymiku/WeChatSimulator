using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class TransferSelectFriendItem : ItemBase
{
    public ImageProxy head;
    public TextProxy realName;
    public TextProxy nickName;
    public TextProxy singleName;
    AccountSaveData data;
    public override void Init()
    {
        base.Init();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickBtn);
    }
    public override void SetData(object o)
    {
        base.SetData(o);
        data = o as AccountSaveData;
        HeadSpriteUtils.Instance.SetHead(head, data.accountId);
        if (string.IsNullOrEmpty(data.nickname) || string.IsNullOrEmpty(data.realName))
        {
            if (string.IsNullOrEmpty(data.nickname) && string.IsNullOrEmpty(data.realName))
            {
                singleName.text = ContentHelper.Read(ContentHelper.NotSetNickName);
            }
            else if (string.IsNullOrEmpty(data.nickname))
            {
                singleName.text = data.realName;
            }
            else
            {
                singleName.text = data.nickname;
            }
            realName.gameObject.SetActive(false);
            nickName.gameObject.SetActive(false);
            singleName.gameObject.SetActive(true);
        }
        else
        {
            realName.text = data.realName;
            nickName.text = data.nickname;
            realName.gameObject.SetActive(true);
            nickName.gameObject.SetActive(true);
            singleName.gameObject.SetActive(false);
        }
    }
    private void OnClickBtn()
    {
        UIManager.Instance.Pop();
        UIManager.Instance.Push(new TransferToAccountContext(data.phoneNumber));
    }
}
