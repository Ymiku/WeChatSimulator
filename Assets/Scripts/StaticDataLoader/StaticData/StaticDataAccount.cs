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
            result.Add(item.phone_number.Substring(0,11));
        }
        return result;
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

    public static ACCOUNT GetAccountByPhoneNumber(string phoneNumber)
    {
        foreach (var item in Info.items)
        {
            if (item.phone_number.Substring(0, 11) == phoneNumber)
                return item;
        }
        return null;
    }

}
