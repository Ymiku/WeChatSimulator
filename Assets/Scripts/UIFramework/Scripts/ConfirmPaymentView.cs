using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class ConfirmPaymentView : AnimateView
	{
		private ConfirmPaymentContext _context;
        private GameObject _useItem;
        private GameObject _canNotUseObj;
        private GameObject _okTextObj;
        private GameObject _selectTextObj;

		public override void Init ()
		{
			base.Init ();
            _useItem = FindChild("Content/UseItem");
            _canNotUseObj = FindChild("Content/CantUse");
            _okTextObj = FindChild("Content/OkBtn/Text");
            _selectTextObj = FindChild("Content/OkBtn/Text1");
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
