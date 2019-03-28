using static_data;
using UnityEngine;

public static class StaticDataContent
{
    public static CONTENT_ARRAY Infos;

    public static void Init()
    {
        Infos = StaticDataLoader.ReadOneDataConfig<CONTENT_ARRAY>("content");
    }

    public static CONTENT GetDataById(int id)
    {
        foreach (var item in Infos.items) {
            if (item.id == id)
                return item;
        }
        Debug.LogError(string.Format("content data not exist id {0}", id));
        return null;
    }

    public static string GetContent(int id)
    {
        foreach (var item in Infos.items)
        {
            if (item.id == id)
                return item.content;
        }
        Debug.LogError(string.Format("content data not exist id {0}", id));
        return "";
    }
}
