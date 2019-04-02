using static_data;
using UnityEngine;

public static class StaticDataContent
{
    public static CONTENT_ARRAY Info;

    public static void Init()
    {
        Info = StaticDataLoader.ReadOneDataConfig<CONTENT_ARRAY>("content");
    }

    public static CONTENT GetDataById(int id)
    {
        for (int i = 0; i < Info.items.Count; i++)
        {
            if (Info.items[i].id == id)
                return Info.items[i];
        }
        Debug.LogError(string.Format("content data not exist id {0}", id));
        return null;
    }

    public static string GetContent(int id)
    {
        for (int i = 0; i < Info.items.Count; i++)
        {
            if (Info.items[i].id == id)
                return Info.items[i].content;
        }
        Debug.LogError(string.Format("content data not exist id {0}", id));
        return "";
    }
}
