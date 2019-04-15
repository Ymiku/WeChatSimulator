using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public class UIType {

        public string Path { get; private set; }

        public string Name { get; private set; }

        public UIType(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }
	
		//public static readonly UIType LogIn = new UIType("View/LogInView");
		public static readonly UIType Home = new UIType("View/HomeView");
		public static readonly UIType Fortune = new UIType("View/FortuneView");
		public static readonly UIType Koubei = new UIType("View/KoubeiView");
		public static readonly UIType Friends = new UIType("View/FriendsView");
		public static readonly UIType Me = new UIType("View/MeView");
		public static readonly UIType Chat = new UIType("View/ChatView");
		public static readonly UIType Quit = new UIType("View/QuitView");
		public static readonly UIType Scan = new UIType("View/ScanView");
		public static readonly UIType Pay = new UIType("View/PayView");
		public static readonly UIType Collect = new UIType("View/CollectView");
		public static readonly UIType Pocket = new UIType("View/PocketView");
		public static readonly UIType Transfer = new UIType("View/TransferView");
		public static readonly UIType PhoneTopup = new UIType("View/PhoneTopupView");
		public static readonly UIType MyPackages = new UIType("View/MyPackagesView");
		public static readonly UIType Carhailing = new UIType("View/CarhailingView");
		public static readonly UIType Settings = new UIType("View/SettingsView");
		public static readonly UIType Membership = new UIType("View/MembershipView");
		public static readonly UIType Transactions = new UIType("View/TransactionsView");
		public static readonly UIType Balance = new UIType("View/BalanceView");
		public static readonly UIType TotalAssets = new UIType("View/TotalAssetsView");
		public static readonly UIType TransDetails = new UIType("View/TransDetailsView"); //转账明细界面
		public static readonly UIType YuEBao = new UIType("View/YuEBaoView"); //余额宝界面
		public static readonly UIType AntCredit = new UIType("View/AntCreditView"); //花呗界面
		public static readonly UIType ZhimaCredit = new UIType("View/ZhimaCreditView"); //芝麻信用界面
		public static readonly UIType MyBank = new UIType("View/MyBankView"); //我的银行卡界面
		public static readonly UIType BankServices = new UIType("View/BankServicesView"); //银行卡服务界面
		public static readonly UIType Search = new UIType("View/SearchView"); //搜索栏
		public static readonly UIType FixedTerm = new UIType("View/FixedTermView"); //定期界面
		public static readonly UIType FixedTermHave = new UIType("View/FixedTermHaveView"); //定期持有界面
		public static readonly UIType Login = new UIType("View/LoginView"); //点击头像登录
		public static readonly UIType RegistByPhoneNumber = new UIType("View/RegistByPhoneNumberView"); //注册
		public static readonly UIType LoginByPhoneNumber = new UIType("View/LoginByPhoneNumberView"); //用手机号登录
		public static readonly UIType AddBankCard = new UIType("View/AddBankCardView"); //添加银行卡界面
		public static readonly UIType AddBankCardInfo = new UIType("View/AddBankCardInfoView"); //添加银行卡第二步界面
		public static readonly UIType RegistAgreement = new UIType("View/RegistAgreementView"); //注册协议
		public static readonly UIType TransferToAccount = new UIType("View/TransferToAccountView"); //转到支付宝账户
		public static readonly UIType InputTransferAmount = new UIType("View/InputTransferAmountView"); //输入转账金额界面
		public static readonly UIType ConfirmPayment = new UIType("View/ConfirmPaymentView"); //确认付款
		public static readonly UIType SelectPayWay = new UIType("View/SelectPayWayView"); //选择付款方式
		public static readonly UIType ChangeAccountLogin = new UIType("View/ChangeAccountLoginView"); //切换账号
		public static readonly UIType TransferSucc = new UIType("View/TransferSuccView"); //转账成功
		public static readonly UIType InputAndCheckPayword = new UIType("View/InputAndCheckPaywordView"); //输入支付密码
		public static readonly UIType RecoverPassword = new UIType("View/RecoverPasswordView"); //找回密码
		public static readonly UIType EmailVerification = new UIType("View/EmailVerificationView"); //Email验证
		public static readonly UIType Password = new UIType("View/PasswordView"); //设置密码
		public static readonly UIType SetLoginPassword = new UIType("View/SetLoginPasswordView"); //设置登录密码
		public static readonly UIType SecuritySetting = new UIType("View/SecuritySettingView"); //安全设置
		public static readonly UIType Contacts = new UIType("View/ContactsView"); //好友列表
		public static readonly UIType NewFriends = new UIType("View/NewFriendsView"); //好友申请列表
        public static readonly UIType PersonalInfo = new UIType("View/PersonalInfoView"); //个人信息
        public static readonly UIType PersonalHomePage = new UIType("View/PersonalHomePageView"); //个人主页
        public static readonly UIType SetHead = new UIType("View/SetHeadView"); //设置个人头像
		public static readonly UIType ChangePayword = new UIType("View/ChangePaywordView"); //修改支付密码
		public static readonly UIType ConfirmPayword = new UIType("View/ConfirmPaywordView"); //修改支付密码
		public static readonly UIType YuEBaoIn = new UIType("View/YuEBaoInView"); //转入余额宝
		public static readonly UIType YuEBaoOutToBalance = new UIType("View/YuEBaoOutToBalanceView"); //余额宝转出到余额
		public static readonly UIType YuEBaoOutToCard = new UIType("View/YuEBaoOutToCardView"); //余额宝转出到银行卡
		public static readonly UIType YuEBaoInSucc = new UIType("View/YuEBaoInSuccView"); //余额宝转入成功
		public static readonly UIType YuEBaoOutSucc = new UIType("View/YuEBaoOutSuccView"); //余额宝转出成功
		public static readonly UIType TransferToBankCard = new UIType("View/TransferToBankCardView"); //转账到银行卡
		public static readonly UIType TransferToBankCardConfirm = new UIType("View/TransferToBankCardConfirmView"); //确认银行卡转帐信息
		public static readonly UIType BalanceRecharge = new UIType("View/BalanceRechargeView"); //余额充值
		public static readonly UIType BalanceCash = new UIType("View/BalanceCashView"); //余额提现
		public static readonly UIType BalanceCashSelectCard = new UIType("View/BalanceCashSelectCardView"); //提现选择银行卡
		public static readonly UIType RechargeSucc = new UIType("View/RechargeSuccView"); //充值成功
		public static readonly UIType BalanceCashSucc = new UIType("View/BalanceCashSuccView"); //提现成功
		public static readonly UIType BankCardDetail = new UIType("View/BankCardDetailView"); //银行卡详情
		public static readonly UIType Tweet = new UIType("View/TweetView"); //推特
		public static readonly UIType Blog = new UIType("View/BlogView"); //博客
		public static readonly UIType Album = new UIType("View/AlbumView"); //相册
    }
}
