using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class TransferToBankCardConfirmView : AnimateView
	{
		private TransferToBankCardConfirmContext _context;
        public TextProxy _nameText;
        public TextProxy _cardText;
        public TextProxy _moneyText;
        public TextProxy _detailText;
        public ImageProxy _bankIcon;
        public FInputField _remarksField;

        private double _serviceAmount;
        private double _totalMoney;

        public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TransferToBankCardConfirmContext;
            Refresh();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            _remarksField.text = "";
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            _remarksField.text = "";
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickConfirm()
        {
            UIManager.Instance.Push(new ConfirmPaymentContext(_context.cardId, _context.amount, _serviceAmount, _remarksField.text));
        }

        private void Refresh()
        {
            BankCardSaveData data = XMLSaver.saveData.GetBankCardData(_context.cardId);
            _nameText.text = _context.name;
            _cardText.text = Utils.FormatPaywayStr(PaywayType.BankCard, _context.cardId);
            _serviceAmount = Utils.GetBankServiceAmount(_context.amount);
            _totalMoney = _context.amount + _serviceAmount;
            _moneyText.text = _totalMoney.ToString() + ContentHelper.Read(ContentHelper.RMBText);
            _detailText.text = string.Format(ContentHelper.Read(ContentHelper.TransferToCardDetail), _context.amount, _serviceAmount);
            _bankIcon.sprite = AssetsManager.Instance.GetBankSprite(data.bankName);
            _bankIcon.rectTransform.anchoredPosition3D = new Vector3(-_cardText.preferredWidth / 2 - 20,
                _cardText.transform.localPosition.y, _cardText.transform.localPosition.z);
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
