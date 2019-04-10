using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoInView : AnimateView
	{
		private YuEBaoInContext _context;
        private PaywayType _payWay;
        public TextProxy _bankName;
        public TextProxy _tips;
        public TextProxy _timeText;
        public Image _icon;
        public Button _confirmBtn;
        public FInputField _moneyInput;

        public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoInContext;
            _moneyInput.text = "";
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
            _moneyInput.text = "";
            Refresh();
        }
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickPayway()
        {
            double amount = 0;
            double.TryParse(_moneyInput.text, out amount);
            UIManager.Instance.Push(new SelectPayWayContext(amount, SpendType.ToSelfYuEBao));
        }

        public void OnClickExplain()
        {
            
        }

        public void OnClickConfirm()
        {
            if (string.IsNullOrEmpty(_moneyInput.text))
                return;
            double amount = double.Parse(_moneyInput.text);
            AssetsSaveData assetsData = AssetsManager.Instance.assetsData;
            if (_payWay == PaywayType.Banlance)
            {
                if (assetsData.balance < amount)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                    _moneyInput.text = "";
                }
                else
                {
                    assetsData.balance -= amount;
                    assetsData.yuEBao += amount;
                    UIManager.Instance.Pop();
                }
            }
            else if(_payWay == PaywayType.BankCard)
            {
                BankCardSaveData bankData = AssetsManager.Instance.curUseBankCard;
                if(GameDefine.BankCardMaxTransfer < amount)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.ExceedOnceMaxMoney));
                }
                else if (bankData.money < amount)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                    _moneyInput.text = "";
                }
                else
                {
                    UIManager.Instance.Push(new InputAndCheckPaywordContext(() =>
                    {
                        bankData.money -= amount;
                        assetsData.yuEBao += amount;
                        UIManager.Instance.Pop();
                    }));
                }
            }
        }

        public void OnValueChanged(string str)
        {
            _confirmBtn.interactable = !string.IsNullOrEmpty(_moneyInput.text);
        }

        private void Refresh()
        {
            _payWay = AssetsManager.Instance.curPayway;
            AssetsSaveData assetsData = AssetsManager.Instance.assetsData;
            BankCardSaveData bankData = AssetsManager.Instance.curUseBankCard;
            if (_payWay == PaywayType.YuEBao || _payWay == PaywayType.None)
                _payWay = PaywayType.Banlance;
            if (_payWay == PaywayType.Banlance && assetsData.balance <= 0 && bankData != null)
                _payWay = PaywayType.BankCard;
            switch (_payWay)
            {
                case PaywayType.Banlance:
                    _icon.sprite = Utils.GetBalanceSprite();
                    _bankName.text = ContentHelper.Read(ContentHelper.BalanceText);
                    _tips.text = assetsData.balance > 0 ? string.Format(ContentHelper.Read(ContentHelper.BalanceMaxTransfer), assetsData.balance):
                        ContentHelper.Read(ContentHelper.AssetsNotEnough);
                    break;
                case PaywayType.BankCard:
                    _icon.sprite = AssetsManager.Instance.GetBankSprite(bankData.bankName);
                    _bankName.text = Utils.FormatPaywayStr(PaywayType.BankCard, bankData.cardId);
                    _tips.text = string.Format(ContentHelper.Read(ContentHelper.BankCardMaxTransfer), GameDefine.BankCardMaxTransfer);
                    break;
            }
        }
	}
	public class YuEBaoInContext : BaseContext
	{
		public YuEBaoInContext() : base(UIType.YuEBaoIn)
		{
		}
	}
}
