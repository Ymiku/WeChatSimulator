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
    public PaywayType curPayway = PaywayType.None;

    public void Set(int id)
    {
        this._id = id;
        curPayway = PaywayType.None;
        assetsData = XMLSaver.saveData.GetAssetsData(id);
        bankCardsData = XMLSaver.saveData.GetBankCardDataList(id);
        curUseBankCard = XMLSaver.saveData.GetCurUseCard(id);
        _defaultCardIcon = Resources.Load<Sprite>(GameDefine.DefaultBankSprite);
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
                    curPayway = PaywayType.Banlance;
                else if (assetsData.yuEBao >= money)
                    curPayway = PaywayType.YuEBao;
                else if (curUseBankCard != null)
                    curPayway = PaywayType.BankCard;
                break;
            case PaywayType.Banlance:
                if (assetsData.balance < money)
                {
                    curPayway = assetsData.yuEBao >= money ? PaywayType.YuEBao :
                        curUseBankCard != null ? PaywayType.BankCard : PaywayType.None;
                }
                break;
            case PaywayType.YuEBao:
                if (assetsData.yuEBao < money)
                {
                    curPayway = assetsData.balance >= money ? PaywayType.Banlance :
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
}

public enum PaywayType
{
    None,
    Banlance,
    YuEBao,
    BankCard,
}
