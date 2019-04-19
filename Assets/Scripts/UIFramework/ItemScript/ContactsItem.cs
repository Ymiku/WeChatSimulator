using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class ContactsItem : ItemBase
{
    public ImageProxy head;
    public TextProxy realName;
    public TextProxy nickName;
    public TextProxy singleName;
    AccountSaveData data;
    public override void SetData(object o)
    {
        base.SetData(o);
        data = o as AccountSaveData;
        HeadSpriteUtils.Instance.SetHead(head,data.accountId);
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
	public void OnClickBtn()
	{
		UIManager.Instance.Push (new PersonalHomePageContext());
	}
}
