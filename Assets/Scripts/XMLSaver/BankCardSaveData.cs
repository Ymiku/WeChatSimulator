using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class SaveData
{
    private List<BankCardSaveData> bankCardList;

    /// <summary>
    /// 添加一个银行卡数据
    /// </summary>
    public void AddBankCardData(BankCardSaveData data)
    {
        int accountId = data.accountId;
        int cardId = data.cardId;
        int existAccountIndex = -1;
        for (int i = 0; i < bankCardList.Count; i++)
        {
            if (bankCardList[i].accountId == accountId && bankCardList[i].cardId == cardId)
                existAccountIndex = i;
        }
        if (existAccountIndex >= 0)
        {
            bankCardList[existAccountIndex] = null;
            bankCardList[existAccountIndex] = data;
        }
        else
        {
            bankCardList.Add(data);
        }
    }

    /// <summary>
    /// 通过唯一id获取银行卡数据
    /// </summary>
    public List<BankCardSaveData> GetBankCardDataListById(int accountId) {
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
    public List<BankCardSaveData> GetBankCardSaveDataListByName(string name)
    {
        int id = 0;  // todo 名字转id
        return GetBankCardDataListById(id);
    }
}


[System.Serializable]
public class BankCardSaveData
{
    /// <summary>
    /// 唯一id
    /// </summary>
    public int accountId {
        get { return accountId; }
        private set { accountId = value; }
    }

    /// <summary>
    /// 银行卡账户
    /// </summary>
    public int cardId {
        get { return cardId; }
        private set { cardId = value; }
    }

    /// <summary>
    /// 银行卡余额
    /// </summary>
    public float money {
        get { return money; }
        private set { money = value; }
    }

    public BankCardSaveData(int accountId, int bankId, float money = 0f) {
        this.accountId = accountId;
        this.cardId = bankId;
        this.money = money;
    }

    /// <summary>
    /// 存钱
    /// </summary>
    public float AddMoney(float value) {
        money += value;
        return money;
    }

    /// <summary>
    /// 减钱
    /// </summary>
    public float ReduceMoney(float value)
    {
        money = Math.Max(0, money - value);
        return money;
    }
}
