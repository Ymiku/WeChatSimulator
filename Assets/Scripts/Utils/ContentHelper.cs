
public class ContentHelper
{

    public static string Read(int id)
    {
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
    public const int BalanceMaxTransfer = 26; //最多可转入{0}元
    public const int BankCardMaxTransfer = 27; //该卡本次最多可转入{0}元
    public const int MoneyNotEnough = 28; //金额不足
    public const int PaywayNotSupport = 29; //该付款方式不支持当前交易
    public const int MaxCanToBanlance = 30; //本次最多可转出{0}元
    public const int MaxCanToCard = 31; //可转出到卡{0}元
    public const int ExceedOnceMaxMoney = 32; //超过单笔最大限额
    public const int PleaseSetPayword = 33; //请先设置支付密码
    public const int YuEBaoToBalanceSucc = 34; //成功转出{0}元至支付宝账户余额
    public const int YuEBaoToCardSucc = 35; //成功转出{0}元至{1}
    public const int YuEBaoInSucc = 36; //成功转入{0}元
    public const int CardNotSupport = 37;  //该卡不支持转账
    public const int NameNotMatchCard = 38; //持卡人与卡号不符
    public const int RMBText = 39; //元
    public const int RMBSign = 40; //¥
    public const int TransferToCardDetail = 41; //对方实收{0}元，服务费{1}元
    public const int TransferText = 42; //转账
    public const int RechargeText = 43; //充值
    public const int PleaseBindBankCard = 44; //当前没绑定银行卡,请先绑定银行卡
    public const int CardLastNum = 45; //尾号{0}储蓄卡
    public const int CanUseBalance = 46; //可用余额{0}元
    public const int CashExceed = 47; //金额已超过可提现余额
    public const int ServiceText = 48; //服务费{0}元
    public const int NoCertification = 49; //尚未进行实名认证
    public const int RealName = 50; //真实姓名
    public const int Iam = 51; //我是
    public const int ExpendText = 52; //支出
    public const int IncomeText = 53; //收入
    public const int TodayText = 54; //今天
    public const int YesterdayText = 55; //昨天
    public const int OtherText = 56; //其他
    public const int TransToCardText = 57; //转账到银行卡-
    public const int YuERecharge = 58; //余额充值
    public const int YuECash = 59; //余额提现
    public const int YuEBaoProfitAdd = 60; //余额宝-{0}-收益发放
    public const int FinanceText = 61; //理财
    public const int NextStepText = 62; //下一步
    public const int Sunday = 63; //星期日
    public const int Monday = 64; //星期一
    public const int Tuesday = 65; //星期二
    public const int Wednesday = 66; //星期三
    public const int Thursday = 67; //星期四
    public const int Friday = 68; //星期五
    public const int Saturday = 69; //星期六
    public const int ConfirmOut = 70; //确认转出
    public const int SingleTrunIn = 71; //单次转入
    public const int OutToBalance = 72; //转出到余额
    public const int OutToCard = 73; //转出到银行卡
    public const int HistoryCard = 74;  //历史收款人
    public const int SelfText = 75; //本人
    public const int AntText = 76; //花呗
    public const int CanUseText = 77; //可用
    public const int AvailableCredit = 78; //可用额度
    public const int SetUserHead = 79; //设置个人头像
    public const int SetUseBack = 80; //设置背景
    public const int AntRemarkStr = 81; //借还款
    public const int AntDetailStr = 82; //花呗还款-{0}年{1}月账单
    public const int AntShouldRepay = 83; //{0}月应还
    public const int AntWaitRepay = 84; //{0}月待还
    public const int AntDeadline = 85; //最后还款日{0}月{1}日
    public const int AntMonthCount = 86; //本月账单明细共{0}笔
}