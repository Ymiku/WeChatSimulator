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
        private FInputField _amountInputField;
        private FInputField _remarksInputField;
        private Button _okBtn;
        private Button _clearBtn;
        private Button _recordBtn;
        private Image _headImg;

        private AccountSaveData _account;

		public override void Init ()
		{
			base.Init ();
            _amountText = FindInChild<Text>("Middle/AmountInput/Text");
            _nameText = FindInChild<Text>("Middle/Name");
            _accountText = FindInChild<Text>("Middle/Account");
            _amountInputField = FindInChild<FInputField>("Middle/AmountInput");
            _remarksInputField = FindInChild<FInputField>("Middle/RemarksInput");
            _headImg = FindInChild<Image>("Middle/HeadSprite/Mask/Image");
            _okBtn = FindInChild<Button>("Middle/OkBtn");
            _clearBtn = FindInChild<Button>("Middle/AmountInput/Clear");
            _recordBtn = FindInChild<Button>("Top/Record");
            _recordBtn.onClick.AddListener(OnClickRecord);
            _clearBtn.onClick.AddListener(OnClickClear);
            _okBtn.onClick.AddListener(OnClickOk);
            _amountInputField.onValueChanged.AddListener(OnInputValueChanged);
            _clearBtn.gameObject.SetActive(false);
            _okBtn.interactable = false;
		}
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as InputTransferAmountContext;
            _account = _context.account;
            if (string.IsNullOrEmpty(_account.nickname) || _account.nickname == ContentHelper.Read(5))
                _nameText.text = _account.realName + "(" + Utils.FormatStringForSecrecy(_account.realName, FInputType.Name) + ")";
            else
                _nameText.text = _account.nickname + "(" + Utils.FormatStringForSecrecy(_account.realName, FInputType.Name) + ")";

            _accountText.text = Utils.FormatStringForSecrecy(_account.phoneNumber, FInputType.PhoneNumber);
            HeadSpriteUtils.Instance.SetHead(_headImg, _account.accountId);
        }

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            _amountInputField.text = "";
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
            double amount = 0;
            double.TryParse(_amountText.text, out amount);
            if (amount > 0)
                UIManager.Instance.Push(new ConfirmPaymentContext(_account.accountId, amount, _remarksInputField.text));
            else
                ShowNotice(ContentHelper.Read(ContentHelper.IllegalInput));
        }

        public void OnClickClear()
        {
            _amountInputField.text = "";
        }

        public void OnClickRecord()
        {
            UIManager.Instance.Push(new TransactionsContext());
        }
	}
	public class InputTransferAmountContext : BaseContext
	{
		public InputTransferAmountContext() : base(UIType.InputTransferAmount)
		{
		}

        public InputTransferAmountContext(AccountSaveData account) : base(UIType.InputTransferAmount)
        {
            this.account = account;
        }

        public AccountSaveData account;
	}
}
