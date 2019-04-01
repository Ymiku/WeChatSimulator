using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class TotalAssetsView : AnimateView
	{
		private TotalAssetsContext _context;
        private Text _totalText;
        private Text _balanceText;
        private Text _yuEBaoText;

		public override void Init ()
		{
			base.Init ();
            _totalText = FindInChild<Text>("Panel/Middle/TotalText/Value");
            _balanceText = FindInChild<Text>("Panel/Middle/BalanceText/Value");
            _yuEBaoText = FindInChild<Text>("Panel/Middle/YuEBaoText/Value");
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TotalAssetsContext;
            _balanceText.text = Player.Instance.assetsData.balance.ToString();
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
	}
	public class TotalAssetsContext : BaseContext
	{
		public TotalAssetsContext() : base(UIType.TotalAssets)
		{
		}
	}
}
