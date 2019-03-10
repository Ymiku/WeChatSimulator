/*UI类模板
using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class TempletView : ParentView
	{
		private TempletContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TempletContext;
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
	public class TempletContext : BaseContext
	{
		public TempletContext() : base(UIType.Templet)
		{
		}
	}
}
*/