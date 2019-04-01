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
    private Text _notEnoughText;

    void Awake()
    {
        _notEnoughRoot = FindChild("NotEnoughRoot");
        _addCardRoot = FindChild("AddCardRoot");
        _canUseRoot = FindChild("CanUseRoot");
        _selectedObj = FindChild("selected");
        _selectBtn = GetComponent<Button>();
        _icon = FindInChild<Image>("Icon");
        _payWayText = FindInChild<Text>("CanUseRoot/text");
        _notEnoughText = FindInChild<Text>("NotEnoughRoot/titleText");
        _selectBtn.onClick.AddListener(OnClick);
    }

    public override void SetData(object o)
    {
        _data = o as SelectPaywayItemData;
        Refresh();
    }

    public void OnClick()
    {
        if (_data.isEnough)
        {
            Player.Instance.curPayway = _data.payway;
            if (_data.payway == PaywayType.BankCard)
            {
                Player.Instance.SetCurUseCard(_data.cardId);
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
        _selectBtn.interactable = _data.isEnough || _data.isAddCard;
        bool selectFlag = false;
        if (!_data.isAddCard && _data.isEnough)
        {
            if (_data.payway == Player.Instance.curPayway)
            {
                selectFlag = true;
                if (_data.payway == PaywayType.BankCard)
                    selectFlag = _data.cardId == Player.Instance.curUseBankCard.cardId;
            }
        }
        _selectedObj.SetActive(selectFlag);
        _addCardRoot.SetActive(_data.isAddCard);
        _canUseRoot.SetActive(!_data.isAddCard && _data.isEnough);
        _notEnoughRoot.SetActive(!_data.isAddCard && !_data.isEnough);
        _icon.gameObject.SetActive(!_data.isAddCard);

        if (_data.payway == PaywayType.BankCard)
            _payWayText.text = Utils.FormatPaywayStr(_data.payway, _data.cardId);
        else if (_data.payway == PaywayType.Banlance)
            _payWayText.text = Utils.FormatPaywayStr(_data.payway) + "(剩余:" + Player.Instance.assetsData.balance + ")";
        else if (_data.payway == PaywayType.YuEBao)
            _payWayText.text = Utils.FormatPaywayStr(_data.payway) + "(剩余:" + Player.Instance.assetsData.yuEBao + ")";
        if (!_data.isEnough)
            _notEnoughText.text = _payWayText.text;
    }
}

public class SelectPaywayItemData: object
{
    public bool isAddCard;
    public PaywayType payway;
    public bool isEnough;
    public string cardId;

    public SelectPaywayItemData() { }
}