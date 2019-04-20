using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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
            if (_payWay == PaywayType.Balance)
            {
                if (assetsData.balance < amount)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                    _moneyInput.text = "";
                }
                else
                {
                    UIManager.Instance.Push(new InputAndCheckPaywordContext(() =>
                    {
                        assetsData.balance -= amount;
                        assetsData.yuEBao += amount;
                        TransactionSaveData actionData = new TransactionSaveData();
                        actionData.iconType = TransactionIconType.YuEBao;
                        actionData.payway = PaywayType.Balance;
                        actionData.streamType = TransactionStreamType.NoChange;
                        actionData.remarkStr = ContentHelper.Read(ContentHelper.FinanceText);
                        actionData.timeStr = DateTime.Now.ToString();
                        actionData.detailStr = ContentHelper.Read(ContentHelper.YuEBaoText) + "-" +  ContentHelper.Read(ContentHelper.SingleTrunIn);
                        AssetsManager.Instance.AddTransactionData(actionData);
                        UIManager.Instance.Pop();
                        string payStr = Utils.FormatPaywayStr(PaywayType.Balance);
                        UIManager.Instance.Push(new YuEBaoInSuccContext(amount, payStr));
                    }));
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
                        string payStr = Utils.FormatPaywayStr(PaywayType.BankCard, bankData.cardId);
                        UIManager.Instance.Push(new YuEBaoInSuccContext(amount, payStr));
                    }));
                }
            }
        }

        public void OnValueChanged(string str)
        {
            double amount = 0;
            if (!string.IsNullOrEmpty(_moneyInput.text))
                double.TryParse(_moneyInput.text, out amount);
            _confirmBtn.interactable = amount > 0;
        }

        private void Refresh()
        {
            _timeText.text = DateTime.Now.AddDays(1).ToString("MM-dd");
            AssetsSaveData assetsData = AssetsManager.Instance.assetsData;
            BankCardSaveData bankData = AssetsManager.Instance.curUseBankCard;
            _payWay = AssetsManager.Instance.curPayway;
            if (_payWay == PaywayType.YuEBao || _payWay == PaywayType.None || _payWay == PaywayType.Ant)
                _payWay = PaywayType.Balance;
            if (_payWay == PaywayType.Balance && assetsData.balance <= 0 && bankData != null)
                _payWay = PaywayType.BankCard;
            switch (_payWay)
            {
                case PaywayType.Balance:
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
