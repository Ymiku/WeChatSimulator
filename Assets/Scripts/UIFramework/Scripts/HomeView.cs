using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class HomeView : EnabledView
	{
		private HomeContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as HomeContext;
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
		}
		public override void Excute ()
		{
			base.Excute ();

		}
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
        public void OnClickScan()
        {
            UIManager.Instance.Push(new ScanContext());
        }
        public void OnClickPay()
        {
            UIManager.Instance.Push(new PayContext());
        }
        public void OnClickCollect()
        {
            UIManager.Instance.Push(new CollectContext());
        }
        public void OnClickPocket()
        {
            UIManager.Instance.Push(new PocketContext());
        }
        public void OnClickTransfer()
        {
            UIManager.Instance.Push(new TransferContext());
        }
    }
	public class HomeContext : BaseContext
	{
		public HomeContext() : base(UIType.Home)
		{
		}
	}
}
