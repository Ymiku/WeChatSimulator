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
                data.realName = staticData.real_name;
                data.nickname = staticData.nick_name;
                data.phoneNumber = staticData.phone_number;
				string path = "HeadSprites/"+data.enname;
				if(Resources.Load<Sprite>(path)!=null)
					data.headSprite = path;
                path = "BackSprites/" + data.enname;
                if (Resources.Load<Sprite>(path) != null)
                    data.backSprite = path;
            }
            data.accountId = accountId;
            if (accountId == 0)
            {
                accountList.Insert(0,data);
            }
            else
            {
                accountList.Add(data);
            }
        }
		return data;
    }

    /// <summary>
    /// 通过唯一id获取账户信息
    /// </summary>
	AccountSaveData cacheData = new AccountSaveData(){accountId = -1};
	public AccountSaveData GetAccountData(int id) {
		if (GameManager.Instance.accountData!=null&&GameManager.Instance.accountData.accountId == id) {
			//return GameManager.Instance.accountData;
		}
		if (cacheData.accountId == id)
			return cacheData;
        for (int i = 0; i < accountList.Count; i++)
        {
			if (accountList [i].accountId == id) {
				cacheData = accountList [i];
				return accountList [i];
			}
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
    public string realName;             // 账户实名
    public string nickname;             // 账户名(昵称)
    public string password;             // 账户登陆密码
    public string payword;              // 账户支付密码
    public string headSprite;           // 头像
    public string backSprite;
    public string GetAnyName()
    {
        if (!string.IsNullOrEmpty(nickname))
            return nickname;
        if (!string.IsNullOrEmpty(realName))
            return realName;
        return ContentHelper.Read(ContentHelper.NotSetNickName);
    }
}