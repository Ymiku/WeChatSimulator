using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class CommonMobileStatusBarView : BaseCommonView
	{
		private CommonMobileStatusBarContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as CommonMobileStatusBarContext;
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
	public class CommonMobileStatusBarContext : BaseContext
	{
		public CommonMobileStatusBarContext() : base(UIType.CommonMobileStatusBar)
		{
		}
	}
}
