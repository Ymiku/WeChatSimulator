using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class AntCreditView : AnimateView
	{
		private AntCreditContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AntCreditContext;
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

        public void OnScrollValueChanged(Vector2 deltaPos) {
            Debug.Log(deltaPos); // rtodo
        }
	}
	public class AntCreditContext : BaseContext
	{
		public AntCreditContext() : base(UIType.AntCredit)
		{
		}
	}
}
