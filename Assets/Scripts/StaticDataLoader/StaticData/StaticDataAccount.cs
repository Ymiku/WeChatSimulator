using static_data;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDataAccount
{
    public static ACCOUNT_ARRAY Info;

    public static void Init()
    {
        Info = StaticDataLoader.ReadOneDataConfig<ACCOUNT_ARRAY>("account");
        TransToSaveData();
    }

    public static ACCOUNT GetAccountById(int id)
    {
        for (int i = 0; i < Info.items.Count; i++)
        {
            if (Info.items[i].id == id)
                return Info.items[i];
        }
        return null;
    }

    public static void TransToSaveData()
    {
        for (int i = 0; i < Info.items.Count; i++)
        {
            XMLSaver.saveData.AddAccountData(Info.items[i].id);
            XMLSaver.saveData.AddAssetsData(Info.items[i].id);
        }
    }
}
