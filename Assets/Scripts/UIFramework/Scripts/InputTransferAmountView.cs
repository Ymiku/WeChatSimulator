using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static_data;

namespace UIFrameWork
{
	public class InputTransferAmountView : AnimateView
	{
		private InputTransferAmountContext _context;
        private Text _amountText;
        private Text _nameText;
        private Text _accountText;
        private FInputField _inputField;
        private Button _okBtn;
        private Button _clearBtn;

        private ACCOUNT _account;

		public override void Init ()
		{
			base.Init ();
            activeWhenPause = true;
            _amountText = FindInChild<Text>("Middle/AmountInput/Text");
            _nameText = FindInChild<Text>("Middle/Name");
            _accountText = FindInChild<Text>("Middle/Account");
            _inputField = FindInChild<FInputField>("Middle/AmountInput");
            _okBtn = FindInChild<Button>("Middle/OkBtn");
            _clearBtn = FindInChild<Button>("Middle/AmountInput/Clear");

            _okBtn.onClick.AddListener(OnClickOk);
            _inputField.onValueChanged.AddListener(OnInputValueChanged);
            _clearBtn.gameObject.SetActive(false);
            _okBtn.interactable = false;
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as InputTransferAmountContext;
            _account = _context.account;
            _nameText.text = _account.name + "(" + _account.name + ")";
            _accountText.text = Utils.FormatStringForSecrecy(_account.phone_number.Substring(0, 11), FInputType.PhoneNumber);
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            _inputField.text = "";
            _clearBtn.gameObject.SetActive(false);
            _okBtn.interactable = false;
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

        private void OnInputValueChanged(string value)
        {
            _okBtn.interactable = !string.IsNullOrEmpty(value);
            _clearBtn.gameObject.SetActive(!string.IsNullOrEmpty(value));
        }

        public void OnClickOk()
        {
            float amount = 0;
            float.TryParse(_amountText.text, out amount);
            if (amount > 0)
                UIManager.Instance.Push(new ConfirmPaymentContext(_account.id, amount));
            else
                ShowNotice(ContentHelper.Read(ContentHelper.IllegalInput));
        }

        public void OnClickClear()
        {
            _inputField.text = "";
        }
	}
	public class InputTransferAmountContext : BaseContext
	{
		public InputTransferAmountContext() : base(UIType.InputTransferAmount)
		{
		}

        public InputTransferAmountContext(ACCOUNT account) : base(UIType.InputTransferAmount)
        {
            this.account = account;
        }

        public ACCOUNT account;
	}
}
