using System;
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
        data.cardName = StaticDataBankCard.GetCardNameById(cardId);
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
        for (int i = 0; i < bankCardList.Count; i++)
        {
            if (bankCardList[i].accountId == accountId && bankCardList[i].cardId == cardId)
                return bankCardList[i];
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

    /// <summary>
    /// 获取默认使用的银行卡
    /// </summary>
    public BankCardSaveData GetCurUseCard(int accountId) {
        for (int i = 0; i < curUseCardList.Count; i++)
        {
            if (curUseCardList[i].accountId == accountId)
                return curUseCardList[i];
        }
        return new BankCardSaveData();
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
    public string cardName;     // 银行名字
    public double money;        // 银行卡余额
}

public static class BankCardDefine
{
    public const int cardIdMinLength = 16;  // 卡号最低长度
    public const int cardIdMaxLength = 19;  // 卡号最高长度
}