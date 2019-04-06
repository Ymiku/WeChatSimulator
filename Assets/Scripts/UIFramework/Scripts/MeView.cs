using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class MeView : EnabledView
	{
        public ImageProxy headSprite;
        public TextProxy userName;
        public TextProxy phoneNumber;
		private MeContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as MeContext;
            Refresh();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
            Refresh();
		}
		public override void Excute ()
		{
			base.Excute ();

		}

        private void Refresh()
        {
            HeadSpriteUtils.Instance.SetHead(headSprite);
            userName.text = GameManager.Instance.accountData.realname;
            phoneNumber.text = Utils.FormatStringForSecrecy(GameManager.Instance.accountData.phoneNumber, FInputType.PhoneNumber);
        }

        #region 按钮事件部分
        public void OnClickHome()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new HomeContext());
        }
        public void OnClickFortune()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FortuneContext());
        }
        public void OnClickKoubei()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new KoubeiContext());
        }
        public void OnClickFriends()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FriendsContext());
        }
        public void OnClickMe()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new MeContext());
        }
        public void OnClickSettings()
        {
            UIManager.Instance.Push(new SettingsContext());
        }
        public void OnClickMembership() {
            UIManager.Instance.Push(new MembershipContext());
        }
        public void OnClickTransactions() {
            UIManager.Instance.Push(new TransactionsContext());
        }
        public void OnClickTotalAssets() {
            UIManager.Instance.Push(new TotalAssetsContext());
        }
        public void OnClickBalance() {
            UIManager.Instance.Push(new BalanceContext());
        }
        public void OnClickYuEBao() {
            UIManager.Instance.Push(new YuEBaoContext());
        }
        public void OnClickAntCredit() {
            UIManager.Instance.Push(new AntCreditContext());
        }
        public void OnClickZhimaCredit()
        {
            UIManager.Instance.Push(new ZhimaCreditContext());
        }
        public void OnClickBank() {
            UIManager.Instance.Push(new MyBankContext());
        }
        public void OnClickPersonInfo()
        {
            UIManager.Instance.Push(new PersonalInfoContext());
        }
        #endregion
    }
    public class MeContext : BaseContext
	{
		public MeContext() : base(UIType.Me)
		{
		}
	}
}
