using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : Singleton<Player>
{
    private int id;
    #region 当前player的存档数据
    public AccountSaveData accountData
    {
        get { return XMLSaver.saveData.GetAccountData(id); }
    }
    public AssetsSaveData assetsData
    {
        get { return XMLSaver.saveData.GetAssetsData(id); }
    }
    public List<BankCardSaveData> bankCardsData
    {
        get { return XMLSaver.saveData.GetBankCardDataList(id); }
    }
    public BankCardSaveData curUseBankCard
    {
        get { return XMLSaver.saveData.GetCurUseCard(id); }
        set { BankCardSaveData data = XMLSaver.saveData.GetCurUseCard(id); data = value; }
    }
    #endregion

    public PaywayType curPayway = PaywayType.None;

    public void OnEnter()
    {
        id = GameManager.Instance.curUserId;
    }

    public void OnExit()
    {
        id = 0;
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
    public BankCardSaveData GetBankCardData(string cardId) {
        return XMLSaver.saveData.GetBankCardData(id, cardId);
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
}

public enum PaywayType
{
    None,
    Banlance,
    YuEBao,
    BankCard,
}
