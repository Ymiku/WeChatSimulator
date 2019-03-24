using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class SaveData
{
    public List<string> names = new List<string>();
    //0是主角
    public List<CharacterData> charDatas = new List<CharacterData>();

}
[System.Serializable]
public class CharacterData
{
    public List<CardData> cardDatas = new List<CardData>();
}
[System.Serializable]
public class CardData
{
    public int cardId;
    public float balance;
}