using static_data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class SaveData
{
    public List<AssetsSaveData> assetsDataList = new List<AssetsSaveData>();

    public AssetsSaveData AddAssetsData(int accountId)
    {
        int existIndex = -1;
        for (int i = 0; i < assetsDataList.Count; i++)
        {
            if (assetsDataList[i].accountId == accountId)
                existIndex = i;
        }
        if (existIndex >= 0)
        {
            return assetsDataList[existIndex];
        }
        AssetsSaveData data = new AssetsSaveData();
        data.transactionList = new List<TransactionSaveData>();
        ACCOUNT staticData = StaticDataAccount.GetAccountById(accountId);
        if (staticData != null)
            data.balance = staticData.money;
        data.accountId = accountId;
        assetsDataList.Add(data);
        return data;
    }

    public AssetsSaveData GetAssetsData(int accountId)
    {
        foreach (var data in assetsDataList) {
            if (data.accountId == accountId)
                return data;
        }
        AssetsSaveData _data = new AssetsSaveData();
        _data.accountId = accountId;
        return _data;
    }

    public AssetsSaveData GetAssetsData(string name) {
        int id = 0;  // todo 名字转id
        return GetAssetsData(id);
    }

    public void AddTransactionData(int id, TransactionSaveData data)
    {
        for (int i = 0; i < assetsDataList.Count; i++)
        {
            if(assetsDataList[i].accountId == id)
            {
                if (assetsDataList[i].transactionList.Count == 50)
                    assetsDataList[i].transactionList[49] = data;
                else
                    assetsDataList[i].transactionList.Add(data);
                break;
            }
        }
    }
}

/// <summary>
/// 账户资产数据类
/// </summary>
[System.Serializable]
public class AssetsSaveData
{
    public int accountId;               //账户id
    public double balance;              //余额
    public double yuEBao;               //余额宝
    public float yuEBaoProfit;          //余额宝总收益
    public float antPayValue;           //蚂蚁花呗可用
    public float antPayLimit;           //蚂蚁花呗额度
    public float yuEBaoYesterday;       //昨日收益
    public string lastOfflineTime;      //上次离线时间
    public List<TransactionSaveData> transactionList;   //转账记录 todo 排序
}

[Serializable]
public class TransactionSaveData
{
    public TransactionMoneyType moneyType;
    public TransactionIconType iconType;
    public string timeStr;
    public string detailStr;
    public string remarkStr;
    public double money;
    public string bankName;
    public int accountId;
}
