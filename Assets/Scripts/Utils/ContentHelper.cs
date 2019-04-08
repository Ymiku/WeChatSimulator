
public class ContentHelper {

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
    public const int SelectHeadText = 15; //选择头像
    public const int NotSetNickName = 16; //未设置昵称
    public const int PaywordMustNew = 17; //新密码不能与以前密码相同
    public const int ChangePaywordSucc = 18; //修改支付密码成功
    public const int DifferPayword = 19; //两次输入密码不一致
    public const int PaywordCantLikeOrSerial = 20; //支付密码不能是重复、连续的数字
    public const int SetPaywordText = 21; //设置支付密码
    public const int ChangePaywordText = 22; //修改支付密码
    public const int TotalAssetsText = 23; //总金额{0}元
    public const int GuestDontWorry = 24; //客官别急
    public const int AddCardSucc = 25; //添加银行卡成功
}
