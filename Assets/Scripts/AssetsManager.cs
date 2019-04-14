using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AssetsManager : Singleton<AssetsManager>
{
    private int _id;
    private Sprite _defaultCardIcon;
    private Dictionary<string, Sprite> _bankIconDict = new Dictionary<string, Sprite>();
    public AssetsSaveData assetsData;
    public List<BankCardSaveData> bankCardsData;
    public BankCardSaveData curUseBankCard;
    public string curUseCardId = "";
    public PaywayType curPayway = PaywayType.None;

    public void Set(int id)
    {
        this._id = id;
        curPayway = PaywayType.None;
        assetsData = XMLSaver.saveData.GetAssetsData(id);
        bankCardsData = XMLSaver.saveData.GetBankCardDataList(id);
        curUseBankCard = XMLSaver.saveData.GetCurUseCard(id);
        _defaultCardIcon = Resources.Load<Sprite>(GameDefine.DefaultBankSprite);
        if (curUseBankCard != null)
            curUseCardId = curUseBankCard.cardId;
        RecalculationOfflineProfit();
    }

    /// <summary>
    /// 更新当前默认使用的支付方式
    /// </summary>
    public PaywayType SetCurPaywayByMoney(double money)
    {
        switch (curPayway)
        {
            case PaywayType.None:
                if (assetsData.balance >= money)
                    curPayway = PaywayType.Balance;
                else if (assetsData.yuEBao >= money)
                    curPayway = PaywayType.YuEBao;
                else if (curUseBankCard != null)
                    curPayway = PaywayType.BankCard;
                break;
            case PaywayType.Balance:
                if (assetsData.balance < money)
                {
                    curPayway = assetsData.yuEBao >= money ? PaywayType.YuEBao :
                        curUseBankCard != null ? PaywayType.BankCard : PaywayType.None;
                }
                break;
            case PaywayType.YuEBao:
                if (assetsData.yuEBao < money)
                {
                    curPayway = assetsData.balance >= money ? PaywayType.Balance :
                        curUseBankCard != null ? PaywayType.BankCard : PaywayType.None;
                }
                break;
            case PaywayType.BankCard:
                break;
        }
        return curPayway;
    }

    /// <summary>
    /// 获取银行卡
    /// </summary>
    public BankCardSaveData GetBankCardData(string cardId)
    {
        for (int i = 0; i < bankCardsData.Count; i++)
        {
            if (bankCardsData[i].cardId == cardId)
                return bankCardsData[i];
        }
        return null;
    }

    /// <summary>
    /// 设置当前默认使用的银行卡
    /// </summary>
    public BankCardSaveData SetCurUseCard(string cardId)
    {
        BankCardSaveData data = GetBankCardData(cardId);
        if (data != null)
        {
            if (curUseBankCard != null)
                curUseBankCard = data;
            else
                XMLSaver.saveData.curUseCardList.Add(data);
            return data;
        }
        else
            return null;
    }

    /// <summary>
    /// 刷新银行卡列表
    /// </summary>
    public void UpdateCardsList()
    {
        bankCardsData = XMLSaver.saveData.GetBankCardDataList(_id);
        curUseBankCard = XMLSaver.saveData.GetCurUseCard(_id);
    }

    /// <summary>
    /// 获取银行卡icon
    /// </summary>
    public Sprite GetBankSprite(string bankName)
    {
        if (string.IsNullOrEmpty(bankName))
            return _defaultCardIcon;
        if (_bankIconDict.ContainsKey(bankName))
            return _bankIconDict[bankName];
        else
        {
            Sprite sprite = Resources.Load<Sprite>("BankCardSprites/" + StaticDataBankCard.GetBankSpriteByBankName(bankName));
            _bankIconDict.Add(bankName, sprite);
            return sprite;
        }
    }

    /// <summary>
    /// 保存离线时间
    /// </summary>
    public void SaveOfflineTime()
    {
        if (assetsData != null)
            assetsData.lastOfflineTime = DateTime.Now.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 一天收益
    /// </summary>
    public void RecalculationOneDayProfit()
    {
        assetsData.yuEBaoYesterday = (float)Math.Round(assetsData.yuEBao * GameDefine.TenThousandProfit / 10000 > 0.01 ? assetsData.yuEBao * GameDefine.TenThousandProfit / 10000 : 0, 2);
        assetsData.yuEBaoProfit += assetsData.yuEBaoYesterday;
        assetsData.yuEBao += assetsData.yuEBaoYesterday;
    }

    /// <summary>
    /// 离线期间收益
    /// </summary>
    private void RecalculationOfflineProfit()
    {
        if (string.IsNullOrEmpty(assetsData.lastOfflineTime))
            return;
        DateTime now = DateTime.Now;
        DateTime lastTime = DateTime.Parse(assetsData.lastOfflineTime);
        TimeSpan timeSpan = lastTime - now;
        int day = timeSpan.Days;
        day = day < 3650 ? day : 3650;
        if(day > 0)
        {
            for(int i = 0; i < day; i++)
            {
                RecalculationOneDayProfit();
            }
        }
    }

    /// <summary>
    /// 增加一条账单记录
    /// </summary>
    public void AddTransactionData(TransactionSaveData data)
    {
        XMLSaver.saveData.AddTranstionData(_id, data);
    }

    /// <summary>
    /// 获取总支出
    /// </summary>
    public double GetExpendTotalMoney()
    {
        double result = 0;
        for(int i = 0; i < assetsData.transactionList.Count; i++)
        {
            if (assetsData.transactionList[i].transactionType == TransactionType.Expend)
                result += assetsData.transactionList[i].money;
        }
        return result;
    }

    /// <summary>
    /// 获取总收入
    /// </summary>
    public double GetIncomeTotalMoney()
    {
        double result = 0;
        for (int i = 0; i < assetsData.transactionList.Count; i++)
        {
            if (assetsData.transactionList[i].transactionType == TransactionType.Income)
                result += assetsData.transactionList[i].money;
        }
        return result;
    }
}

public enum PaywayType
{
    None,
    Balance,
    YuEBao,
    BankCard,
}

public enum SpendType
{
    TransferToBalance,  //转账到支付宝账户
    TransferToBankCard, //转账到银行卡
    ToSelfYuEBao,       //转到自己余额宝
    ToSelfBankCard,     //转到自己银行卡
    ToSelfAssets,       //转到自己余额、余额宝
}

public enum RechargeType
{
    Balance,
    YuEBao,
}

public enum TransactionType
{
    NoChange,
    Expend,
    Income,
}