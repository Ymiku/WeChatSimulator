using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class SelectPayWayView : AnimateView
	{
		private SelectPayWayContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as SelectPayWayContext;
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
	public class SelectPayWayContext : BaseContext
	{
		public SelectPayWayContext() : base(UIType.SelectPayWay)
		{
		}
	}
}
