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

		if(false)
		{
			//npc读表，tbd
			return data;
		}
        data.accountRealname = StaticDataContent.GetContent(5);
		//data.accountId = accountId;
		//data.accountNickname = "";
		//data.accountHeadSprite = AccountDefine.DefaultHeadSprite;

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
            accountList.Add(data);
        }
		return data;
    }

    /// <summary>
    /// 通过唯一id获取账户信息
    /// </summary>
    public AccountSaveData GetAccountData(int id) {
        foreach (var data in accountList) {
            if (data.accountId == id)
                return data;
        }
        AccountSaveData _data = new AccountSaveData();
        _data.accountId = id;
        return _data;
    }

    /// <summary>
    /// 通过唯一名字获取账户信息
    /// </summary>
    public AccountSaveData GetAccountData(string name) {
        int id = 0; // Todo 通过name获取id
        return GetAccountData(id);
    }
}

/// <summary>
/// 账户信息数据类
/// </summary>
[System.Serializable]
public class AccountSaveData
{
    public int accountId;               // 账户唯一id
    public string phoneNumber;          // 账户电话号码
    public string accountRealname;      // 账户实名
    public string accountNickname;      // 账户名(昵称)
    public string accountPassword;      // 账户登陆密码
    public int accountPayword;          // 账户支付密码
    public string accountHeadSprite;    // 头像
    public Sprite GetHeadSprite()
    {
        if(accountHeadSprite != null)
            return Resources.Load<Sprite>(accountHeadSprite);
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