using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class BankCardItem : ItemBase
{
    private ImageProxy _bankIcon;
    private ImageProxy _bg;
    private TextProxy _cardName;
    private TextProxy _lastNumber;
    private BankCardSaveData _data;
    private Button _btn;

    public override void Init()
    {
        base.Init();
        _bankIcon = FindInChild<ImageProxy>("icon");
        _bg = FindInChild<ImageProxy>("bg");
        _cardName = FindInChild<TextProxy>("bankName");
        _lastNumber = FindInChild<TextProxy>("lastNum");
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickBtn);
    }

    public override void SetData(object o)
    {
        base.SetData(o);
        _data = o as BankCardSaveData;
        _cardName.text = _data.cardName.Replace(ContentHelper.Read(ContentHelper.SavingCardText), "");
        _lastNumber.text = _data.cardId.Substring(_data.cardId.Length - 4, 4);
    }

    private void OnClickBtn()
    {

    }
}