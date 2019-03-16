using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class MyPackagesView : AnimateView
	{
		private MyPackagesContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as MyPackagesContext;
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
	public class MyPackagesContext : BaseContext
	{
		public MyPackagesContext() : base(UIType.MyPackages)
		{
		}
	}
}
