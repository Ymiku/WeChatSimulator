using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UIFrameWork
{
	public class BalanceCashView : AnimateView
	{
		private BalanceCashContext _context;
        public ImageProxy _icon;
        public TextProxy _bankName;
        public TextProxy _lastCardId;
        public Text _tips;
        public Button _allBtn;
        public Button _okBtn;
        public FInputField _moneyInput;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BalanceCashContext;
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

        public void OnValueChanged(string str)
        {
            bool interactFlag = false;
            double money = 0;
            double.TryParse(_moneyInput.text, out money);
            if (!string.IsNullOrEmpty(_moneyInput.text) && money > 0)
                interactFlag = true;
            _okBtn.interactable = interactFlag;
            double serviceMoney = Utils.GetBankServiceAmount(money);
            if (money == 0)
            {
                _tips.text = string.Format(ContentHelper.Read(ContentHelper.CanUseBalance), AssetsManager.Instance.assetsData.balance.ToString("0.00"));
            }
            else if (money > AssetsManager.Instance.assetsData.balance - serviceMoney)
            {
                _tips.text = ContentHelper.Read(ContentHelper.CashExceed);
            }
            else
            {
                _tips.text = string.Format(ContentHelper.Read(ContentHelper.ServiceText), serviceMoney);
            }
        }

        public void OnClickOk()
        {
            double money = double.Parse(_moneyInput.text);
            double serviceMoney = Utils.GetBankServiceAmount(money);
            if (money + serviceMoney > AssetsManager.Instance.assetsData.balance)
                ShowNotice(ContentHelper.Read(ContentHelper.CashExceed));
            else
                UIManager.Instance.Push(new InputAndCheckPaywordContext(()=> {
                    BankCardSaveData data = AssetsManager.Instance.curUseBankCard;
                    AssetsManager.Instance.assetsData.balance -= (money + serviceMoney);
                    data.money += money;
                    TransactionSaveData actionData = new TransactionSaveData();
                    actionData.timeStr = DateTime.Now.ToString();
                    actionData.streamType = TransactionStreamType.NoChange;
                    actionData.remarkStr = ContentHelper.Read(ContentHelper.OtherText);
                    actionData.money = money;
                    actionData.detailStr = ContentHelper.Read(ContentHelper.YuECash);
                    actionData.iconType = TransactionIconType.BankCard;
                    actionData.cardId = data.cardId;
                    AssetsManager.Instance.AddTransactionData(actionData);
                    UIManager.Instance.Pop();
                    UIManager.Instance.Push(new BalanceCashSuccContext(Utils.FormatPaywayStr(PaywayType.BankCard,data.cardId), money));
                }));
        }

        public void OnClickAll()
        {
            double money = AssetsManager.Instance.assetsData.balance - Utils.GetBankServiceAmount(AssetsManager.Instance.assetsData.balance);
            _moneyInput.text = money.ToString();
        }

        public void OnClickCardItem()
        {
            UIManager.Instance.Push(new BalanceCashSelectCardContext());
        }

        private void Refresh()
        {
            BankCardSaveData data = AssetsManager.Instance.curUseBankCard;
            _moneyInput.text = "";
            _icon.sprite = AssetsManager.Instance.GetBankSprite(data.bankName);
            _bankName.text = data.bankName.Replace(ContentHelper.Read(ContentHelper.SavingCardText),"");
            _lastCardId.text = string.Format(ContentHelper.Read(ContentHelper.CardLastNum), data.cardId.Substring(data.cardId.Length - 4, 4));
            _tips.text = string.Format(ContentHelper.Read(ContentHelper.CanUseBalance), AssetsManager.Instance.assetsData.balance.ToString("0.00"));
            _allBtn.gameObject.SetActive(AssetsManager.Instance.assetsData.balance > 0.2);
        }
	}
	public class BalanceCashContext : BaseContext
	{
		public BalanceCashContext() : base(UIType.BalanceCash)
		{
		}
	}
}
