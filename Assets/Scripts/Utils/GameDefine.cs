using System;
using UnityEngine;

public class GameDefine
{
    public const string DefaultHeadSprite = "HeadSprites/social_head_default";      //默认头像
    public const string DefaultBankSprite = "BankCardSprites/bank_default";         //默认银行卡icon
    public const double BankCardMaxTransfer = 100000.0;                             //银行卡单次最多
    public const string ForbidTextColor = "<color=#BEBFBE>{0}</color>";             //禁用灰
    public const string NormalTextColor = "<color=#F5F8F8>{0}</color>";             //正常白
    public const float ServicePower = 0.001f;                                       //服务费
    public const float TenThousandProfit = 0.6532f;                                 //余额宝万份收益
    public const int CardIdMaxLength = 19;                                          //银行卡卡号长度         
    public const int CardIdMinLength = 16;                                          //银行卡卡号长度最低
    public const double AntLimit = 100000.0;                                        //蚂蚁花呗额度 

    public static readonly string[] Weekdays = {
        ContentHelper.Read(ContentHelper.Sunday),
        ContentHelper.Read(ContentHelper.Monday),
        ContentHelper.Read(ContentHelper.Tuesday),
        ContentHelper.Read(ContentHelper.Wednesday),
        ContentHelper.Read(ContentHelper.Thursday),
        ContentHelper.Read(ContentHelper.Friday),
        ContentHelper.Read(ContentHelper.Saturday)};
}
