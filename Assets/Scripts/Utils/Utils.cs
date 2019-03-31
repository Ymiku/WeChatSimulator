using System;
using UIFrameWork;
using UnityEngine;

public static class Utils
{
    public static GameObject FindChild(GameObject parent, string path)
    {
        if (parent != null)
        {
            Transform trans = parent.transform.Find(path);
            if (trans)
                return trans.gameObject;
            else
            {
                Debug.LogError(string.Format("Utils.FindChild parent go {0} not exist child {1}", parent.name, path));
                return null;
            }
        }
        else
        {
            Debug.LogError("Utils.FindChild parent is null");
            return null;
        }
    }
    public static T FindInChild<T>(GameObject parent, string path) where T : Component
    {
        if (parent != null)
        {
            Transform trans = parent.transform.Find(path);
            if (trans)
                return trans.GetComponent<T>();
            else
            {
                Debug.LogError(string.Format("Utils.FindInChild parent go {0} not exist child {1}", parent.name, path));
                return null;
            }
        }
        else
        {
            Debug.LogError("Utils.FindInChild parent is null");
            return null;
        }
    }
    public static bool CheckPhoneNumberExist(string phoneNumber) {
        foreach (var data in XMLSaver.saveData.accountList) {
            if (data.phoneNumber == phoneNumber) {
                return true;
            }
        }
        return false;
    }
    public static bool CheckIsSelfNumber(string number)
    {
        AccountSaveData data = XMLSaver.saveData.GetAccountData(GameManager.Instance.curUserId);
        return data.phoneNumber == number;
    }
    public static string FormatStringForInputField(string s,FInputType type)
    {
		string output = s.Replace(" ","");
        switch (type)
        {
            case FInputType.PhoneNumber:
                if (s.Length >=8)
                    output = output.Insert(7, " ");
                if(s.Length>=4)
                    output = output.Insert(3, " ");
                break;
            case FInputType.CardNumber:
				if (s.Length >=13)
					output = output.Insert(12, " ");
				if(s.Length>=9)
					output = output.Insert(8, " ");
				if(s.Length>=5)
					output = output.Insert(4, " ");
                break;
        }
        return output;
    }
	public static string FormatStringForSecrecy(string s,FInputType type)
	{
		string output = s;
		switch (type)
		{
		case FInputType.PhoneNumber:
			output = output.Substring (0,3)+"******"+output.Substring(9);
			break;
		case FInputType.CardNumber:
			output = "**** **** **** "+output.Substring(12);
			break;
		case FInputType.Name:
			output = "*" + output.Substring (1);
			break;
		}
		return output;
	}
	/// <summary>
	/// 获取汉字的拼音首字母
	/// </summary>
	/// <returns>The spell code.</returns>
	/// <param name="CnChar">Cn char.</param>
	public static string GetSpellCode(string CnChar)
	{
		long iCnChar;
		byte[] arrCN = System.Text.Encoding.Default.GetBytes(CnChar);

		//如果是字母，则直接返回
		if (arrCN.Length == 1)
		{
			CnChar = CnChar.ToUpper();
		}
		else
		{
			int area = (short)arrCN[0];
			int pos = (short)arrCN[1];
			iCnChar = (area << 8) + pos;

			// iCnChar match the constant
			string letter = "ABCDEFGHJKLMNOPQRSTWXYZ";
			int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 
				　　　　　　　　　　　　　　　　　　　　48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 
				　　　　　　　　　　　　　　　　　　　　51387, 51446, 52218, 52698, 52980, 53689, 54481, 55290 };
			for (int i = 0; i < 23; i++)
			{
				if (areacode[i] <= iCnChar && iCnChar < areacode[i + 1])
				{
					CnChar = letter.Substring(i, 1);
					break;
				}
			}
		}
		return CnChar;
	}
    public static ResultType TryPay(double money, PaywayType way, string cardId = "")
    {
        ResultType result = ResultType.Failed;
        AssetsSaveData data = XMLSaver.saveData.GetAssetsData(GameManager.Instance.curUserId);
        switch (way)
        {
            case PaywayType.Banlance:
                if (data.balance >= money)
                {
                    data.balance -= money;
                    result = ResultType.Success;
                }
                break;
            case PaywayType.YuEBao:
                if (data.yuEBao >= money)
                {
                    data.yuEBao -= money;
                    result = ResultType.Success;
                }
                break;
            case PaywayType.BankCard:
                if (string.IsNullOrEmpty(cardId))
                {
                    if (XMLSaver.saveData.curUseBankCard.money >= money)
                    {
                        XMLSaver.saveData.curUseBankCard.money -= money;
                        result = ResultType.Success;
                    }
                }
                else
                {
                    BankCardSaveData cardData = XMLSaver.saveData.GetBankCardData(GameManager.Instance.curUserId, cardId);
                    if (cardData != null && cardData.money >= money)
                    {
                        cardData.money -= money;
                        result = ResultType.Success;
                    }
                }
                break;
            default:
                Debug.LogError(string.Format("try use pay way {0}, but not handle this way"));
                break;
        }
        return result;
    }
}
public enum FInputType
{
	None = 1,
	PhoneNumber = 2,
	CardNumber = 4,
	Name = 8,
    Money = 16,
}

public enum ResultType {
    Success,
    Failed,
}