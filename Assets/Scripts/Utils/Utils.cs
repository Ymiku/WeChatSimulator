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
    public static string FormatStringForInputField(string s, FInputType type)
    {
        string output = s.Replace(" ", "");
        switch (type)
        {
            case FInputType.PhoneNumber:
                if (s.Length >= 8)
                    output = output.Insert(7, " ");
                if (s.Length >= 4)
                    output = output.Insert(3, " ");
                break;
            case FInputType.CardNumber:
                if (s.Length >= 17)
                    output = output.Insert(16, " ");
                if (s.Length >= 13)
                    output = output.Insert(12, " ");
                if (s.Length >= 9)
                    output = output.Insert(8, " ");
                if (s.Length >= 5)
                    output = output.Insert(4, " ");
                break;
        }
        return output;
    }
    public static string FormatStringForSecrecy(string s, FInputType type)
    {
        string output = s;
        switch (type)
        {
            case FInputType.PhoneNumber:
                output = output.Substring(0, 3) + "******" + output.Substring(9);
                break;
            case FInputType.CardNumber:
                output = "**** **** **** " + output.Substring(12);
                break;
            case FInputType.Name:
                output = "*" + output.Substring(1);
                break;
            case FInputType.Money:
                output = "****";
                break;
        }
        return output;
    }
    /// <summary>
    /// 获取汉字的拼音首字母
    /// </summary>
    /// <returns>The spell code.</returns>
    /// <param name="CnChar">Cn char.</param>
    public static string GetSpellCode(string name)
    {
        string CnChar = name.Substring(0,1);
        long iCnChar;
        string outChar = null;
        byte[] arrCN = System.Text.Encoding.Default.GetBytes(CnChar);

        //如果是字母，则直接返回
        if (arrCN.Length == 1)
        {
            outChar = CnChar.ToUpper();
            if (outChar[0] < 65 || outChar[0] > 90)
                outChar = "#";
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
                    outChar = letter.Substring(i, 1);
                    break;
                }
            }
            if (outChar == null)
                outChar = "#";
        }
        return outChar;
    }
    public static ResultType TryPay(double money, PaywayType way, string cardId = "")
    {
        ResultType result = ResultType.Failed;
        AssetsSaveData data = AssetsManager.Instance.assetsData;
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
                    BankCardSaveData cardData = XMLSaver.saveData.GetCurUseCard(GameManager.Instance.curUserId);
                    if (cardData.money >= money)
                    {
                        cardData.money -= money;
                        result = ResultType.Success;
                    }
                }
                else
                {
                    BankCardSaveData cardData = XMLSaver.saveData.GetBankCardData(cardId);
                    if (cardData != null && cardData.money >= money)
                    {
                        cardData.money -= money;
                        result = ResultType.Success;
                    }
                }
                break;
            default:
                Debug.LogError(string.Format("try use pay way {0}, but not handle this way", way));
                break;
        }
        return result;
    }
    public static string FormatPaywayStr(PaywayType payway, string cardId = "")
    {
        string result = "";
        switch (payway)
        {
            case PaywayType.Banlance:
                result = ContentHelper.Read(ContentHelper.BalanceText);
                break;
            case PaywayType.YuEBao:
                result = ContentHelper.Read(ContentHelper.YuEBaoText);
                break;
            case PaywayType.BankCard:
                BankCardSaveData data;
                if (string.IsNullOrEmpty(cardId))
                    data = AssetsManager.Instance.curUseBankCard;
                else
                    data = XMLSaver.saveData.GetBankCardData(cardId);                
                string cardStr = data.cardId.Substring(data.cardId.Length - 4, 4);
                result = data.bankName + "(" + cardStr + ")";
                break;
        }
        return result;
    }
    private static Sprite _balanceSprite;
    public static Sprite GetBalanceSprite()
    {
        if (_balanceSprite != null)
            return _balanceSprite;
        _balanceSprite = Resources.Load<Sprite>("CommonSprites/yu_e");
        return _balanceSprite;
    }
    private static Sprite _yuEBaoSprite;
    public static Sprite GetYuEBaoSprite()
    {
        if (_yuEBaoSprite != null)
            return _yuEBaoSprite;
        _yuEBaoSprite = Resources.Load<Sprite>("CommonSprites/yu_e_bao");
        return _yuEBaoSprite;
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