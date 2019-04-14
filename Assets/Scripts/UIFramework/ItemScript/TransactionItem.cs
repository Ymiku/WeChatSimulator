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
        _detailText.text = _data.detailStr;
        _remarkText.text = _data.remarkStr;
        DateTime dealTime = DateTime.Parse(_data.timeStr);
        DateTime nowTime = DateTime.Now;
        TimeSpan timeSpan = nowTime - dealTime;
        if (timeSpan.Days == 0)
        {
            _timeText.text = ContentHelper.Read(ContentHelper.TodayText) + " " + dealTime.ToShortTimeString();
        }
        else if (timeSpan.Days == 1)
        {
            _timeText.text = ContentHelper.Read(ContentHelper.YesterdayText) + " " + dealTime.ToShortTimeString();
        }
        else
        {
            _timeText.text = dealTime.ToString("m") + " " + dealTime.ToShortTimeString();
        }
        if (_data.transactionType == TransactionType.Expend)
        {
            _moneyText.text = "<color=#000000>-" + _data.money + "</color>";
        }
        else if(_data.transactionType == TransactionType.Income)
        {
            _moneyText.text = "<color=#F90000>+" + _data.money + "</color>";
        }
        else
        {
            _moneyText.text = "<color=#000000>" + _data.money + "</color>";
        }
    }

    private void OnClickBtn()
    {

    }
}