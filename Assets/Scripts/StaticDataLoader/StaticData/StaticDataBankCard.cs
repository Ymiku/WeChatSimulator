using UnityEngine;
using static_data;
using System.Collections.Generic;

public static class StaticDataBankCard
{
    public static BANK_CARD_ARRAY Info;
    public static Dictionary<string, string> CardMarkToNameDict = new Dictionary<string, string>();
    public static Dictionary<string, string> BankSpriteDict = new Dictionary<string, string>();

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
            if (!BankSpriteDict.ContainsKey(Info.items[i].card_name))
                BankSpriteDict.Add(Info.items[i].card_name, Info.items[i].card_icon);
        }
    }

    public static string GetCardNameById(string cardId)
    {
        if (!string.IsNullOrEmpty(cardId))
        {
            string checkStr = cardId.Substring(0, 6);
            if (CardMarkToNameDict.ContainsKey(checkStr))
                return CardMarkToNameDict[checkStr];
            checkStr = cardId.Substring(0, 5);
            if (CardMarkToNameDict.ContainsKey(checkStr))
                return CardMarkToNameDict[checkStr];
        }
        return ContentHelper.Read(ContentHelper.DefaultCardName);
    }

    public static string GetBankSpriteByBankName(string bankName)
    {
        if (!string.IsNullOrEmpty(bankName))
            return GameDefine.DefaultBankSprite;
        else if (BankSpriteDict.ContainsKey(bankName))
            return BankSpriteDict[bankName];
        else
            return GameDefine.DefaultBankSprite;
    }
}
