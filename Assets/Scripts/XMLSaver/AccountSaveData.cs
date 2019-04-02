using static_data;
using System.Collections.Generic;
using UnityEngine;

public partial class SaveData {
    public List<AccountSaveData> accountList = new List<AccountSaveData>();
    
    /// <summary>
    /// 添加一个账户信息
    /// </summary>
	public AccountSaveData AddAccountData(int accountId)
    {
		AccountSaveData data = new AccountSaveData();

        int existAccountIndex = -1;
        for (int i = 0; i < accountList.Count; i++)
        {
            if (accountList[i].accountId == accountId)
                existAccountIndex = i;
        }
        if (existAccountIndex >= 0)
        {
			data = accountList[existAccountIndex];
        }
        else
        {
            ACCOUNT staticData = StaticDataAccount.GetAccountById(accountId);
            if (staticData != null)
            {
                data.enname = staticData.en_name;
                data.password = staticData.password;
                data.realname = staticData.name;
                data.phoneNumber = staticData.phone_number;
            }
            data.accountId = accountId;
            data.nickname = StaticDataContent.GetContent(5);
            accountList.Add(data);
        }
		return data;
    }

    /// <summary>
    /// 通过唯一id获取账户信息
    /// </summary>
    public AccountSaveData GetAccountData(int id) {
        for (int i = 0; i < accountList.Count; i++)
        {
            if (accountList[i].accountId == id)
                return accountList[i];
        }
        return null;
    }

    /// <summary>
    /// 通过唯一名字获取账户信息
    /// </summary>
    public AccountSaveData GetAccountData(string name) {
        for (int i = 0; i < accountList.Count; i++)
        {
            if (accountList[i].enname == name)
                return accountList[i];
        }
        return null;
    }

    /// <summary>
    /// 通过电话号码获取账户信息
    /// </summary>
    public AccountSaveData GetAccountDataByPhoneNumber(string phoneNumber) {
        foreach (var data in accountList)
        {
            if (data.phoneNumber == phoneNumber)
                return data;
        }
        return null;
    }
}

/// <summary>
/// 账户信息数据类
/// </summary>
[System.Serializable]
public class AccountSaveData
{
    public int accountId;               // 账户唯一id
    public string enname;
    public string phoneNumber;          // 账户电话号码
    public string realname;             // 账户实名
    public string nickname;             // 账户名(昵称)
    public string password;             // 账户登陆密码
    public int payword;                 // 账户支付密码
    public string headSprite;           // 头像
    public Sprite GetHeadSprite()
    {
        if(headSprite != null)
            return Resources.Load<Sprite>(headSprite);
        return Resources.Load<Sprite>("Sprites/social_head_default");
    }
}

public static class AccountDefine {
    public const int PasswordMaxLength = 12;                        //登陆密码长度最长
    public const int PasswordMinLength = 5;                         //登陆密码长度最短
    public const int PaywordLength = 6;                             //支付密码长度
    public const int NicknameMaxLength = 5;                         //昵称长度最长
    public const string DefaultHeadSprite = "push_chat_default";    //默认头像
}