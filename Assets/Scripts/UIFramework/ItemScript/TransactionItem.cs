using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIFrameWork;
using UnityEngine.UI;

public class TransactionItem : ItemBase
{
    private TransactionSaveData _data;
    private Button _btn;
    public ImageProxy _icon;
    public TextProxy _detailText;
    public TextProxy _remarkText;
    public TextProxy _timeText;
    public TextProxy _moneyText;

    public override void Init()
    {
        base.Init();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickBtn);
    }

    public override void SetData(object o)
    {
        base.SetData(o);
        _data = o as TransactionSaveData;
        RefreshTime();
        RefreshMoney();
        RefreshIcon();
        RefreshOther();
    }

    private void RefreshTime()
    {
        DateTime dealTime = DateTime.Parse(_data.timeStr);
        DateTime nowTime = DateTime.Now;
        TimeSpan timeSpan = nowTime - dealTime;
        double totalSeconds = timeSpan.TotalSeconds + dealTime.Hour * 3600 + dealTime.Minute * 60 + dealTime.Second;
        string hourTimeStr = " " + dealTime.Hour.ToString("00") + ":" + dealTime.Minute.ToString("00");
        if (dealTime.ToLongDateString() == nowTime.ToLongDateString())
        {
            _timeText.text = ContentHelper.Read(ContentHelper.TodayText) + hourTimeStr;
        }
        else if (totalSeconds >= 86400 && totalSeconds < 172800)
        {
            _timeText.text = ContentHelper.Read(ContentHelper.YesterdayText) + hourTimeStr;
        }
        else
        {
            _timeText.text = dealTime.ToString("m") + hourTimeStr;
        }
    }
    private void RefreshMoney()
    {
        string money = _data.money.ToString("0.00");
        if (_data.moneyType == TransactionMoneyType.Expend)
        {
            _moneyText.text = "<color=#000000>-" + money + "</color>";
        }
        else if (_data.moneyType == TransactionMoneyType.Income)
        {
            _moneyText.text = "<color=#F90000>+" + money + "</color>";
        }
        else
        {
            _moneyText.text = "<color=#000000>" + money + "</color>";
        }
    }
    private void RefreshIcon()
    {
        switch (_data.iconType)
        {
            case TransactionIconType.BankCard:
                if (!string.IsNullOrEmpty(_data.cardId))
                    _icon.sprite = AssetsManager.Instance.GetBankSprite(XMLSaver.saveData.GetBankCardData(_data.cardId).bankName);
                break;
            case TransactionIconType.YuEBao:
                _icon.sprite = Utils.GetYuEBaoSprite();
                break;
            case TransactionIconType.UserHead:
                HeadSpriteUtils.Instance.SetHead(_icon, _data.accountId);
                break;
        }
    }
    private void RefreshOther()
    {
        _detailText.text = _data.detailStr;
        _remarkText.text = _data.remarkStr;
    }

    private void OnClickBtn()
    {

    }
}