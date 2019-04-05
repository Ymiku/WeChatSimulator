using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class PasswordView : AnimateView
	{
		private PasswordContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as PasswordContext;
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
        public void OnClickSetLoginPassword()
        {
            UIManager.Instance.Push(new SetLoginPasswordContext());
        }
	}
	public class PasswordContext : BaseContext
	{
		public PasswordContext() : base(UIType.Password)
		{
		}
	}
}
