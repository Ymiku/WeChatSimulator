using UnityEngine;
using static_data;

public static class StaticDataBankCard
{
    public static BANK_CARD Info;

    public static void Init()
    {
        Info = StaticDataLoader.ReadOneDataConfig<BANK_CARD>("bank_card");
    }

}
