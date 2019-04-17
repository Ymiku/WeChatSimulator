using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class BankCardDetailView : AnimateView
	{
		private BankCardDetailContext _context;
        public Text _titleText;
        public TextProxy _bankText;
        public TextProxy _moneyText;
        public TextProxy _lastNumText;
        public Image _bankIcon;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BankCardDetailContext;
            Refresh();
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
            Refresh();
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        private void Refresh()
        {
            _bankIcon.sprite = AssetsManager.Instance.GetBankSprite(_context.data.bankName);
            _titleText.text = _context.data.bankName;
            _bankText.text = _context.data.bankName.Replace(ContentHelper.Read(ContentHelper.SavingCardText), "");
            _lastNumText.text = _context.data.cardId.Substring(_context.data.cardId.Length - 4, 4);
            _moneyText.text = _context.data.money.ToString("0.00");
        }

        public void OnClickTransfer()
        {
            UIManager.Instance.Push(new TransferToBankCardContext(_context.data));
        }
	}
	public class BankCardDetailContext : BaseContext
	{
		public BankCardDetailContext() : base(UIType.BankCardDetail)
		{
		}

        public BankCardDetailContext(BankCardSaveData data) : base(UIType.BankCardDetail)
        {
            this.data = data;
        }

        public BankCardSaveData data;
    }
}
