using UnityEngine;

public class FortuneManager : Singleton<FortuneManager>
{
    private int _id;
    public FortuneSaveData fortuneData;

    public void Set(int id)
    {
        _id = id;
        fortuneData = XMLSaver.saveData.GetFortuneDataById(_id);
    }
}