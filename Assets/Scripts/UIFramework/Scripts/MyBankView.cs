using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class MyBankView : AnimateView
	{
		private MyBankContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as MyBankContext;
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

        public void OnClickServices() {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new BankServicesContext());
        }
	}
	public class MyBankContext : BaseContext
	{
		public MyBankContext() : base(UIType.MyBank)
		{
		}
	}
}
