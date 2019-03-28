using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class InputTransferAmountView : AnimateView
	{
		private InputTransferAmountContext _context;
        private Text _amountText;

        private int _accountId;

		public override void Init ()
		{
			base.Init ();
            _amountText = FindInChild<Text>("");  // rtodo
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as InputTransferAmountContext;
            _accountId = _context.accountId;
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

        public void OnClickOk()
        {
            float amount = 0;
            float.TryParse(_amountText.text, out amount);
            if (amount > 0)
            {
                UIManager.Instance.Push(new ConfirmPaymentContext(_accountId, amount));
            }
        }
	}
	public class InputTransferAmountContext : BaseContext
	{
		public InputTransferAmountContext() : base(UIType.InputTransferAmount)
		{
		}

        public InputTransferAmountContext(int accountId) : base(UIType.InputTransferAmount)
        {
            this.accountId = accountId;
        }

        public int accountId;
	}
}
