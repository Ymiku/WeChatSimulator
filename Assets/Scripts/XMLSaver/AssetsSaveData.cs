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
}

/// <summary>
/// 账户资产数据类
/// </summary>
[System.Serializable]
public class AssetsSaveData
{
    public int accountId;           //账户id
    public double balance;          //余额
    public double yuEBao;           //余额宝
    public float yuEBaoProfit;      //余额宝总收益
    public float antPayValue;       //蚂蚁花呗可用
    public float antPayLimit;       //蚂蚁花呗额度
    public float yuEBaoYesterday;   //昨日收益
    public string lastOfflineTime;  //上次离线时间
}
