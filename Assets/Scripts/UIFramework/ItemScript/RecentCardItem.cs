using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

public class RecentCardItem : ItemBase
{
    public ImageProxy _bankIcon;
    public TextProxy _bankName;
    public TextProxy _realName;
    BankCardSaveData data;

    public override void SetData(object o)
    {
        base.SetData(o);
        data = o as BankCardSaveData;
        _bankIcon.sprite = AssetsManager.Instance.GetBankSprite(data.bankName);
        _realName.text = data.realName;
        _bankName.text = Utils.FormatStringForSecrecy(data.cardId, FInputType.CardNumber) + " " +
            data.bankName.Replace(ContentHelper.Read(ContentHelper.SavingCardText), "");
    }

    public void OnClick()
    {
        UIManager.Instance.Pop();
        UIManager.Instance.Push(new TransferToBankCardContext(data));
    }
}

public class RecentCardTitleItem : ItemBase
{
    public TextProxy _titleText;

    public void SetData(string o)
    {
        _titleText.text = o.ToString();
    }
}