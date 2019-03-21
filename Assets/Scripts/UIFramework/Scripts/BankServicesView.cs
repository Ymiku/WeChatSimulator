using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class BankServicesView : AnimateView
	{
		private BankServicesContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BankServicesContext;
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

        public void OnClickMyBank() {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new MyBankContext());
        }
	}
	public class BankServicesContext : BaseContext
	{
		public BankServicesContext() : base(UIType.BankServices)
		{
		}
	}
}
