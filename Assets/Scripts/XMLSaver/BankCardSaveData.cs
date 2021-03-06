﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class SaveData
{
    public List<BankCardSaveData> bankCardList = new List<BankCardSaveData>();
    public List<BankCardSaveData> curUseCardList = new List<BankCardSaveData>();

    /// <summary>
    /// 添加一个银行卡数据
    /// </summary>
    public BankCardSaveData AddBankCardData(int accountId, string cardId)
    {
        int existAccountIndex = -1;
        for (int i = 0; i < bankCardList.Count; i++)
        {
            if (bankCardList[i].accountId == accountId && bankCardList[i].cardId == cardId)
                existAccountIndex = i;
        }
        if (existAccountIndex >= 0)
            return bankCardList[existAccountIndex];
        BankCardSaveData data = new BankCardSaveData();
        data.accountId = accountId;
        data.cardId = cardId;
        data.realName = GetAccountData(accountId).realName;
        data.bankName = StaticDataBankCard.GetCardNameById(cardId);
        bankCardList.Add(data);
        if (GetBankCardDataList(accountId).Count == 1)
        {
            curUseCardList.Add(data);
        }
        return data;
    }

    /// <summary>
    /// 通过账户id获取银行卡数据
    /// </summary>
    public List<BankCardSaveData> GetBankCardDataList(int accountId) {
        List<BankCardSaveData> result = new List<BankCardSaveData>();
        for (int i = 0; i < bankCardList.Count; i++)
        {
            if (bankCardList[i].accountId == accountId)
                result.Add(bankCardList[i]);
        }
        return result;
    }

    /// <summary>
    /// 通过id获取一张卡的数据
    /// </summary>
    public BankCardSaveData GetBankCardData(string cardId) {
        for (int i = 0; i < bankCardList.Count; i++)
        {
            if (bankCardList[i].cardId == cardId)
                return bankCardList[i];
        }
        return null;
    }

    /// <summary>
    /// 获取默认使用的银行卡
    /// </summary>
    public BankCardSaveData GetCurUseCard(int accountId) {
        for (int i = 0; i < curUseCardList.Count; i++)
        {
            if (curUseCardList[i].accountId == accountId)
                return curUseCardList[i];
        }
        return null;
    }

    /// <summary>
    /// 检查银行卡是否存在
    /// </summary>
    public bool CheckCardExist(string cardId)
    {
        if (!string.IsNullOrEmpty(cardId))
        {
            for (int i = 0; i < bankCardList.Count; i++)
            {
                if (bankCardList[i].cardId == cardId)
                    return true;
            }
        }
        return false;
    }
}

/// <summary>
/// 银行卡数据类
/// </summary>
[System.Serializable]
public class BankCardSaveData
{
    public int accountId;       // 支付宝账户id
    public string cardId;       // 银行卡号
    public string bankName;     // 银行名字
    public double money;        // 银行卡余额
    public string realName;     // 持卡人
}
