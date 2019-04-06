using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
