using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class FortuneView : EnabledView
	{
		private FortuneContext _context;
        public TextProxy _totalText;
        public TextProxy _yesterdayText;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as FortuneContext;
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
            _totalText.text = (AssetsManager.Instance.assetsData.balance + AssetsManager.Instance.assetsData.yuEBao).ToString("0.00");
            _yesterdayText.text = AssetsManager.Instance.assetsData.yuEBaoYesterday.ToString("0.00");
        }

        #region 点击事件部分
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
        public void OnClickYuEBao() {
            UIManager.Instance.Push(new YuEBaoContext());
        }
        public void OnClickTotalAssets() {
            UIManager.Instance.Push(new TotalAssetsContext());
        }
        public void OnClickFixedTerm() {
            UIManager.Instance.Push(new FixedTermContext());
        }
        public void OnClickGold()
        {
            UIManager.Instance.Push(new GoldDealContext());
        }
        #endregion
    }
    public class FortuneContext : BaseContext
	{
		public FortuneContext() : base(UIType.Fortune)
		{
		}
	}
}
