using static_data;
using System.Collections.Generic;
using UnityEngine;

public partial class SaveData
{
    public List<FortuneSaveData> fortuneList = new List<FortuneSaveData>();

    public FortuneSaveData AddFortuneData(int accountId)
    {
        int existIndex = -1;
        for (int i = 0; i < fortuneList.Count; i++)
        {
            if (fortuneList[i].accountId == accountId)
                existIndex = i;
        }
        if (existIndex >= 0)
        {
            return fortuneList[existIndex];
        }
        FortuneSaveData data = new FortuneSaveData();
        ACCOUNT staticData = StaticDataAccount.GetAccountById(accountId);
        if (staticData != null)
            data.gold = staticData.gold;
        data.accountId = accountId;
        fortuneList.Add(data);
        return data;
    }

    public FortuneSaveData GetFortuneDataById(int accountId)
    {
        for (int i = 0; i < fortuneList.Count; i++)
            if (fortuneList[i].accountId == accountId)
                return fortuneList[i];
        return null;
    }
}

public class FortuneSaveData
{
    public int accountId;
    public double gold;
}