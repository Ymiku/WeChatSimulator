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
}
public enum FInputType
{
	None,
	PhoneNumber,
	CardNumber,
	Name
}