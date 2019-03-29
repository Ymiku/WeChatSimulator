using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class InputTransferAmountView : AnimateView
	{
		private InputTransferAmountContext _context;
        private Text _amountText;
        private FInputField _inputField;
        private Button _okBtn;
        private Button _clearBtn;

        private int _accountId;

		public override void Init ()
		{
			base.Init ();
            _amountText = FindInChild<Text>("");  // rtodo
            _inputField = FindInChild<FInputField>("");
            _okBtn = FindInChild<Button>("");
            _clearBtn = FindInChild<Button>("");

            _okBtn.onClick.AddListener(OnClickOk);
            _inputField.onValueChanged.AddListener(OnInputValueChanged);
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

        private void OnEnable()
        {
            EventFactory.numberKeypadEM.AddListener(NumberKeypadEvent.Input, RefreshAmountValue);
            EventFactory.numberKeypadEM.AddListener(NumberKeypadEvent.Clear, DeleteAmountValue);
        }

        private void OnDisable()
        {
            EventFactory.numberKeypadEM.RemoveListener(NumberKeypadEvent.Input, RefreshAmountValue);
            EventFactory.numberKeypadEM.RemoveListener(NumberKeypadEvent.Clear, DeleteAmountValue);
        }

        private void OnInputValueChanged(string value)
        {
            _okBtn.interactable = !string.IsNullOrEmpty(value);
        }

        public void OnClickOk()
        {
            float amount = 0;
            float.TryParse(_amountText.text, out amount);
            if (amount > 0)
                UIManager.Instance.Push(new ConfirmPaymentContext(_accountId, amount));
            else
                ShowNotice(ContentHelper.Read(ContentHelper.IllegalInput));
        }

        public void OnClickClear()
        {
            _inputField.text = "";
        }

        private void RefreshAmountValue(EventArgs args)
        {

        }

        private void DeleteAmountValue()
        {

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
