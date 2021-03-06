using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class RegistAgreementView : AnimateView
	{
		private RegistAgreementContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as RegistAgreementContext;
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
	public class RegistAgreementContext : BaseContext
	{
		public RegistAgreementContext() : base(UIType.RegistAgreement)
		{
		}
	}
}
