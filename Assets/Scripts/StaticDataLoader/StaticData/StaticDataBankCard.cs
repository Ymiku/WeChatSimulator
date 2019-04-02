using UnityEngine;
using static_data;
using System.Collections.Generic;

public static class StaticDataBankCard
{
    public static BANK_CARD_ARRAY Info;
    public static Dictionary<string, string> CardMarkToNameDict = new Dictionary<string, string>();

    public static void Init()
    {
        Info = StaticDataLoader.ReadOneDataConfig<BANK_CARD_ARRAY>("bank_card");
        InitDict();
    }

    private static void InitDict()
    {
        CardMarkToNameDict.Clear();
        for (int i = 0; i < Info.items.Count; i++)
        {
            if (!CardMarkToNameDict.ContainsKey(Info.items[i].card_mark.ToString()))
                CardMarkToNameDict.Add(Info.items[i].card_mark.ToString(), Info.items[i].card_name);
        }
    }

    public static string GetCardNameById(string cardId)
    {
        if (!string.IsNullOrEmpty(cardId))
        {
            string checkStr = cardId.Substring(0, 6);
            if (!string.IsNullOrEmpty(CardMarkToNameDict[checkStr]))
                return CardMarkToNameDict[checkStr];
            checkStr = cardId.Substring(0, 5);
            if (!string.IsNullOrEmpty(CardMarkToNameDict[checkStr]))
                return CardMarkToNameDict[checkStr];
        }
        return ContentHelper.Read(ContentHelper.DefaultCardName);
    }
}
