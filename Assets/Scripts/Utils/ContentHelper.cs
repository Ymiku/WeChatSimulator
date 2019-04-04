﻿
public class ContentHelper{

    public static string Read(int id) {
        return StaticDataContent.GetContent(id);
    }

    //content表里id的define
    public const int CanNotTransToSelf = 2;  //不能给自己转账  
    public const int TransAccountNotExist = 3;  //账户不存在或对方设置了隐私保护
    public const int IllegalInput = 4;  //您的输入不合法
    public const int AssetsNotEnough = 6;  //余额不足
    public const int BalanceText = 7;  //账户余额
    public const int YuEBaoText = 8;  //余额宝
    public const int RemainText = 9;  //剩余
    public const int DefaultCardName = 10;  //兴东银行储蓄卡
    public const int BankCardIllegal = 11;  //该卡暂时不能开通快捷支付，请使用其他卡
    public const int SavingCardText = 12;  //储蓄卡
    public const int CardAlreadyBind = 13;  //已添加该卡，不能重复添加
    public const int PaywordError = 14; //支付密码不正确
}
