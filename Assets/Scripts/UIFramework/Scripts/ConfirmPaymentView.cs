using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class ConfirmPaymentView : AnimateView
	{
		private ConfirmPaymentContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ConfirmPaymentContext;
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
	public class ConfirmPaymentContext : BaseContext
	{
		public ConfirmPaymentContext() : base(UIType.ConfirmPayment)
		{
		}

        public ConfirmPaymentContext(int accountId, float amount) : base(UIType.ConfirmPayment)
        {
            this.accountId = accountId;
            this.amount = amount;
        }

        public int accountId;
        public float amount;
	}
}
