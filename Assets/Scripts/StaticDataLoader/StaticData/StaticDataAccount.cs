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
        foreach (var item in Info.items)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }

    public static void TransToSaveData()
    {
        foreach (var item in Info.items)
        {
            XMLSaver.saveData.AddAccountData(item.id);
            XMLSaver.saveData.AddAssetsData(item.id);
        }
    }
}
