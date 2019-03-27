using System.Collections.Generic;
using UnityEngine;

public partial class SaveData {
    private List<AccountSaveData> accountList = new List<AccountSaveData>();

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
            accountList[existAccountIndex] = data;
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
    public AccountSaveData GetAccountDataById(int id) {
        foreach (var data in accountList) {
            if (data.accountId == id)
                return data;
        }
        Debug.LogError(string.Format("account list not exist account {0}", id));
        return null;
    }

    /// <summary>
    /// 通过唯一名字获取账户信息
    /// </summary>
    public AccountSaveData GetAccountDataByName(string name) {
        int id = 0; // Todo 通过name获取id
        return GetAccountDataById(id);
    }
}

/// <summary>
/// 支付宝账户信息数据类
/// </summary>
[System.Serializable]
public class AccountSaveData
{
    #region 字段
    /// <summary>
    /// 账户唯一id
    /// </summary>
    public int accountId
    {
        get { return accountId; }
        private set { accountId = value; }
    }

    /// <summary>
    /// 账户电话号码
    /// </summary>
    public int accountNumber
    {
        get { return accountNumber; }
        private set { accountNumber = value; }
    }

    /// <summary>
    /// 账户实名
    /// </summary>
    public string accountRealname
    {
        get { return accountRealname; }
        private set { accountRealname = value; }
    }

    /// <summary>
    /// 账户名(昵称)
    /// </summary>
    public string accountNickname {
        get { return accountNickname; }
        private set { accountNickname = value; }
    }

    /// <summary>
    /// 账户登陆密码
    /// </summary>
    public string accountPassword {
        get { return accountPassword; }
        private set { accountPassword = value; }
    }

    /// <summary>
    /// 账户支付密码
    /// </summary>
    public int accountPayword {
        get { return accountPayword; }
        private set {
            if (value.ToString().Length == AccountDefine.PaywordLength)
                accountPayword = value;
            else
                Debug.LogError("payword length error , can not set to : " + value);
        }
    }

    /// <summary>
    /// 头像
    /// </summary>
    public string accountHeadSprite {
        get { return accountHeadSprite; }
        private set { accountHeadSprite = value; }
    }
    #endregion

    public override string ToString()
    {
        return string.Format("account id {0}, account number {1}, account real name {2}, account password {3}," +
            " account payword {4}, account nicename {5}, account headsprite {6}",
            accountId, accountNumber, accountRealname, accountPassword, accountPayword, accountNickname, accountHeadSprite);
    }

    /// <summary>
    /// 修改登陆密码 rtodo 密码限制
    /// </summary>
    /// <param name="newPassword"></param>
    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
        {

        }
        else if (newPassword.Length < AccountDefine.PasswordMinLength)
        {

        }
        else if (newPassword.Length > AccountDefine.PasswordMaxLength) {

        }
        else
        {
            accountPassword = newPassword;
        }
    }

    /// <summary>
    /// 修改支付密码
    /// </summary>
    /// <param name="newPayword"></param>
    public void ChangePayword(int newPayword)
    {
        if (newPayword.ToString().Length != AccountDefine.PaywordLength)
        {

        }
        else
        {
            accountPayword = newPayword;
        }
    }

    /// <summary>
    /// 修改昵称 rtodo
    /// </summary>
    /// <param name="newNickName"></param>
    public void ChangeNickname(string newNickName) {
        if (string.IsNullOrEmpty(newNickName))
        {

        }
        else if (newNickName.Length > AccountDefine.NicknameMaxLength)
        {

        }
        else
        {
            accountNickname = newNickName;
        }
    }

    /// <summary>
    /// 更换头像
    /// </summary>
    /// <param name="newHeadSprite"></param>
    public void SetHead(string newHeadSprite) {
        if (!string.IsNullOrEmpty(newHeadSprite)) {
            accountHeadSprite = newHeadSprite;
        }
    }
}

public static class AccountDefine {
    public const int PasswordMaxLength = 12;                        //登陆密码长度最长
    public const int PasswordMinLength = 5;                         //登陆密码长度最短
    public const int PaywordLength = 6;                             //支付密码长度
    public const int NicknameMaxLength = 5;                         //昵称长度最长
    public const string DefaultHeadSprite = "push_chat_default";    //默认头像
}
