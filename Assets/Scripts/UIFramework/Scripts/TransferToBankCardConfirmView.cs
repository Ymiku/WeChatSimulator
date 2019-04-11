using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class TransferToBankCardConfirmView : AnimateView
	{
		private TransferToBankCardConfirmContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TransferToBankCardConfirmContext;
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
	public class TransferToBankCardConfirmContext : BaseContext
	{
		public TransferToBankCardConfirmContext() : base(UIType.TransferToBankCardConfirm)
		{
		}

        public TransferToBankCardConfirmContext(double amount, string name, string cardId) : base(UIType.TransferToBankCardConfirm)
        {
            this.amount = amount;
            this.name = name;
            this.cardId = cardId;
        }

        public double amount;
        public string name;
        public string cardId;
	}
}
