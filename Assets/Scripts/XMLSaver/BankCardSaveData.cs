using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class SaveData
{
    public List<BankCardSaveData> bankCardList = new List<BankCardSaveData>();
    public BankCardSaveData curUseBankCard;  //当前使用的银行卡

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
        bankCardList.Add(data);
        if (bankCardList.Count == 1) {
            curUseBankCard = data;
        }
        return data;
    }

    /// <summary>
    /// 通过账户id获取银行卡数据
    /// </summary>
    public List<BankCardSaveData> GetBankCardDataList(int accountId) {
        List<BankCardSaveData> result = new List<BankCardSaveData>();
        foreach (var data in bankCardList)
        {
            if (data.accountId == accountId)
                result.Add(data);
        }
        return result;
    }

    /// <summary>
    /// 通过唯一名字获取银行卡数据
    /// </summary>
    public List<BankCardSaveData> GetBankCardDataList(string name)
    {
        int id = 0;  // todo 名字转id
        return GetBankCardDataList(id);
    }

    /// <summary>
    /// 通过id获取一张卡的数据
    /// </summary>
    public BankCardSaveData GetBankCardData(int accountId, string cardId) {
        foreach (var data in bankCardList)
        {
            if (data.accountId == accountId && data.cardId == cardId)
                return data;
        }
        return null;
    }

    /// <summary>
    /// 通过名字获取一张卡的数据
    /// </summary>
    public BankCardSaveData GetBankCardData(string name, string cardId)
    {
        int accountId = 0; // todo 名字转id
        return GetBankCardData(accountId, cardId);
    }
}

/// <summary>
/// 银行卡数据类
/// </summary>
[System.Serializable]
public class BankCardSaveData
{
    public int accountId;       // 账户id
    public string cardId;       // 银行卡号
    public string bankName;     // 银行名字
    public double money;         // 银行卡余额
}

public static class BankCardDefine
{
    public const int cardIdMinLength = 16;  // 卡号最低长度
    public const int cardIdMaxLength = 19;  // 卡号最高长度
}