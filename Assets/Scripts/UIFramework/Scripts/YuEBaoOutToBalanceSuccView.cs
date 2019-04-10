using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoOutToBalanceSuccView : AnimateView
	{
		private YuEBaoOutToBalanceSuccContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutToBalanceSuccContext;
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
	public class YuEBaoOutToBalanceSuccContext : BaseContext
	{
		public YuEBaoOutToBalanceSuccContext() : base(UIType.YuEBaoOutToBalanceSucc)
		{
		}
	}
}
