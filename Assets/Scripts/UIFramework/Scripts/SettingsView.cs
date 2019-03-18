using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class SettingsView : AlphaView
	{
		private SettingsContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as SettingsContext;
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
	public class SettingsContext : BaseContext
	{
		public SettingsContext() : base(UIType.Settings)
		{
		}
	}
}
