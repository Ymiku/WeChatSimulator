using System.IO;
using ProtoBuf;
using UnityEngine;

public static class StaticDataLoader
 {
    public static T ReadOneDataConfig<T>(string dataName)
    {
        FileStream fileStream;
        fileStream = GetDataFileStream(dataName);
        if (null != fileStream)
        {
            T t = Serializer.Deserialize<T>(fileStream);
            fileStream.Close();
            return t;
        }

        return default(T);
    }

    private static FileStream GetDataFileStream(string dataName)
    {
        string filePath = GetDataConfigPath(dataName);
        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            return fileStream;
        }
        return null;
    }

    private static string GetDataConfigPath(string fileName)
    {
        return Application.dataPath + "/Resources/StaticData/static_data_" + fileName + ".data";
    }

    public static void Load()
    {
        StaticDataTest.Init();
        StaticDataContent.Init();
        StaticDataAccount.Init();
    }
}
