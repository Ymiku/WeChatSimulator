using static_data;
using System.Collections.Generic;

public static class StaticDataAccount
{
    public static ACCOUNT_ARRAY Info;

    public static void Init()
    {
        Info = StaticDataLoader.ReadOneDataConfig<ACCOUNT_ARRAY>("account");
    }

    public static List<string> GetAllPhoneNumbers()
    {
        List<string> result = new List<string>();
        foreach (var item in Info.items) {
            result.Add(item.phone_number);
        }
        return result;
    }
}
