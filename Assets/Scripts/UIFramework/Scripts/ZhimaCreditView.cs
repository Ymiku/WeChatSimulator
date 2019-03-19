using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class ZhimaCreditView : AnimateView
	{
		private ZhimaCreditContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ZhimaCreditContext;
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
	public class ZhimaCreditContext : BaseContext
	{
		public ZhimaCreditContext() : base(UIType.ZhimaCredit)
		{
		}
	}
}
