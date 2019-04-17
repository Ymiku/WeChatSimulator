using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class RecentTransferItem : ItemBase
{
    public Image _bankIcon;
    public Image _headIcon;
    public GameObject _headRoot;
    public Text _nameText;
    public Text _idText;
    private Button _btn;
    private TransactionSaveData _data;
    private AccountSaveData _accData;
    private BankCardSaveData _cardData;

    public override void Init()
    {
        base.Init();
        _accData = null;
        _cardData = null;
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickBtn);
    }

    public override void SetData(object o)
    {
        base.SetData(o);
        _data = o as TransactionSaveData;
        _headRoot.SetActive(_data.iconType == TransactionIconType.UserHead);
        _bankIcon.gameObject.SetActive(_data.iconType == TransactionIconType.BankCard);
        if(_data.iconType == TransactionIconType.UserHead)
        {
            HeadSpriteUtils.Instance.SetHead(_headIcon, _data.accountId);
            _accData = XMLSaver.saveData.GetAccountData(_data.accountId);
            if (string.IsNullOrEmpty(_accData.nickname))
                _nameText.text = _accData.nickname + "(" + Utils.FormatStringForSecrecy(_accData.realName, FInputType.Name) + ")";
            else
                _nameText.text = _accData.realName + "(" + Utils.FormatStringForSecrecy(_accData.realName, FInputType.Name) + ")";
            _idText.text = Utils.FormatStringForSecrecy(_accData.phoneNumber, FInputType.PhoneNumber);
        }
        else
        {
            _cardData = XMLSaver.saveData.GetBankCardData(_data.cardId);
            _bankIcon.sprite = AssetsManager.Instance.GetBankSprite(_cardData.bankName);
            _nameText.text = _cardData.realName;
            _idText.text = _cardData.bankName.Replace(ContentHelper.Read(ContentHelper.SavingCardText), "")
                + "(" + _data.cardId.Substring(_data.cardId.Length - 4, 4) + ")";
        }
    }

    private void OnClickBtn()
    {
        if (_accData != null)
        {
            UIManager.Instance.Push(new InputTransferAmountContext(_accData));
        }
        else if (_cardData != null)
        {
            UIManager.Instance.Push(new TransferToBankCardContext(_cardData));
        }
    }
}