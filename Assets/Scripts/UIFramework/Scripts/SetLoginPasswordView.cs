using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class SetLoginPasswordView : AnimateView
	{
        public InputField input;
		private SetLoginPasswordContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as SetLoginPasswordContext;
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
        public void OnClickChangePassword()
        {
            GameManager.Instance.accountData.password = input.text;
            ShowNotice("ÐÞ¸ÄÃÜÂë³É¹¦£¡");
            UIManager.Instance.Pop();
        }
	}
	public class SetLoginPasswordContext : BaseContext
	{
		public SetLoginPasswordContext() : base(UIType.SetLoginPassword)
		{
		}
	}
}
