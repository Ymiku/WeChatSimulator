using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UIFrameWork;

public class SelectPaywayItem : ItemBase
{
    private SelectPaywayItemData _data;
    private GameObject _notEnoughRoot;
    private GameObject _addCardRoot;
    private GameObject _canUseRoot;
    private GameObject _selectedObj;
    private Button _selectBtn;
    private Image _icon;
    private Text _payWayText;
    private Text _notEnoughTitleText;
    private Text _notEnoughText;

    public override void Init()
    {
        base.Init();
        _notEnoughRoot = FindChild("NotEnoughRoot");
        _addCardRoot = FindChild("AddCardRoot");
        _canUseRoot = FindChild("CanUseRoot");
        _selectedObj = FindChild("selected");
        _selectBtn = GetComponent<Button>();
        _icon = FindInChild<Image>("Icon");
        _payWayText = FindInChild<Text>("CanUseRoot/text");
        _notEnoughTitleText = FindInChild<Text>("NotEnoughRoot/titleText");
        _notEnoughText = FindInChild<Text>("NotEnoughRoot/text");
        _selectBtn.onClick.AddListener(OnClick);
    }

    public override void SetData(object o)
    {
        base.SetData(o);
        _data = o as SelectPaywayItemData;
        if (_data == null)
            return;
        Refresh();
    }

    public void OnClick()
    {
        if (_data == null)
            return;
        if (_data.isCanUse)
        {
            AssetsManager.Instance.curPayway = _data.payway;
            if (_data.payway == PaywayType.BankCard)
            {
                AssetsManager.Instance.SetCurUseCard(_data.cardId);
            }
            UIManager.Instance.Pop();
        }
        else if (_data.isAddCard)
        {
            UIManager.Instance.Push(new AddBankCardContext());
        }
    }

    public void Refresh()
    {
        _selectBtn = GetComponent<Button>();
        _selectBtn.interactable = _data.isCanUse || _data.isAddCard;
        bool selectFlag = false;
        if (!_data.isAddCard && _data.isCanUse)
        {
            if (_data.payway == AssetsManager.Instance.curPayway)
            {
                selectFlag = true;
                if (_data.payway == PaywayType.BankCard)
                    selectFlag = _data.cardId == AssetsManager.Instance.curUseBankCard.cardId;
            }
        }
        _selectedObj.SetActive(selectFlag);
        _addCardRoot.SetActive(_data.isAddCard);
        _canUseRoot.SetActive(!_data.isAddCard && _data.isCanUse);
        _notEnoughRoot.SetActive(!_data.isAddCard && !_data.isCanUse);
        _icon.gameObject.SetActive(!_data.isAddCard);

        if (_data.payway == PaywayType.BankCard)
        {
            BankCardSaveData data = AssetsManager.Instance.GetBankCardData(_data.cardId);
            _payWayText.text = Utils.FormatPaywayStr(_data.payway, _data.cardId);
            _icon.sprite = AssetsManager.Instance.GetBankSprite(data.bankName);
        }
        else if (_data.payway == PaywayType.Balance)
        {
            _payWayText.text = Utils.FormatPaywayStr(_data.payway) + "(" + ContentHelper.Read(ContentHelper.RemainText) +
                ":" + AssetsManager.Instance.assetsData.balance.ToString("0.00") + ")";
            _icon.sprite = Utils.GetBalanceSprite();
        }
        else if (_data.payway == PaywayType.YuEBao)
        {
            _payWayText.text = Utils.FormatPaywayStr(_data.payway) + "(" + ContentHelper.Read(ContentHelper.RemainText) + 
                ":" + AssetsManager.Instance.assetsData.yuEBao.ToString("0.00") + ")";
            _icon.sprite = Utils.GetYuEBaoSprite();
        }
        else if(_data.payway == PaywayType.Ant)
        {
            _payWayText.text = Utils.FormatPaywayStr(_data.payway) + "(" + ContentHelper.Read(ContentHelper.CanUseText) +
                ":" + AssetsManager.Instance.assetsData.antPay.ToString("0.00") + ")";
            _icon.sprite = Utils.GetAntSprite();
        }
        if (!_data.isCanUse)
        {
            _notEnoughTitleText.text = _payWayText.text;
            _notEnoughText.text = ContentHelper.Read(ContentHelper.MoneyNotEnough);
            if (_data.payway == PaywayType.YuEBao && (_data.spendType == SpendType.ToSelfAssets || _data.spendType == SpendType.ToSelfYuEBao))
                _notEnoughText.text = ContentHelper.Read(ContentHelper.PaywayNotSupport);
            else if(_data.payway == PaywayType.Balance && _data.spendType == SpendType.ToSelfAssets)
                _notEnoughText.text = ContentHelper.Read(ContentHelper.PaywayNotSupport);
        }
    }
}

public class SelectPaywayItemData: object
{
    public bool isAddCard;
    public PaywayType payway;
    public SpendType spendType;
    public bool isCanUse;
    public string cardId;

    public SelectPaywayItemData() { }
}