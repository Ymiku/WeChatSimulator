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
    }
}
