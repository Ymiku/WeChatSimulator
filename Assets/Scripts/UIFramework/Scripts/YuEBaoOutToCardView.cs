using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UIFrameWork
{
	public class YuEBaoOutToCardView : EnabledView
	{
		private YuEBaoOutToCardContext _context;
        private ToCardTimeType _toCardTimeType;
        public TextProxy _cardText;
        public TextProxy _nomalText;
        public Text _maxMoneyText;
        public ImageProxy _cardIcon;
        public Button _confirmBtn;
        public GameObject _allObj;
        public GameObject _clearObj;
        public GameObject _fastWaySelectedObj;
        public GameObject _normalWaySelectedobj;
        public FInputField _moneyInput;

		public override void Init ()
		{
			base.Init ();
            _clearObj.SetActive(false);
            _toCardTimeType = ToCardTimeType.Fast;
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutToCardContext;
            Refresh();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            Clear();
        }

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            Clear();
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

        public void OnClickToBanlance()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new YuEBaoOutToBalanceContext());
        }

        public void OnClickAll()
        {
            _moneyInput.text = AssetsManager.Instance.assetsData.yuEBao.ToString("0.00");
        }

        public void OnClickFast()
        {
            if (_toCardTimeType == ToCardTimeType.Normal)
            {
                _toCardTimeType = ToCardTimeType.Fast;
                _fastWaySelectedObj.SetActive(true);
                _normalWaySelectedobj.SetActive(false);
            }
        }

        public void OnClickNormal()
        {
           
            if (_toCardTimeType == ToCardTimeType.Fast)
            {
                _toCardTimeType = ToCardTimeType.Normal;
                _fastWaySelectedObj.SetActive(false);
                _normalWaySelectedobj.SetActive(true);
            }
        }

        public void OnClickConfirm()
        {
            // todo µ½ÕËÊ±¼ä
            AssetsSaveData data = AssetsManager.Instance.assetsData;
            BankCardSaveData bankCard = AssetsManager.Instance.curUseBankCard;
            double amount = 0;
            double.TryParse(_moneyInput.text, out amount);
            if (amount > data.yuEBao)
            {
                ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
            }
            else
            {
                UIManager.Instance.Push(new InputAndCheckPaywordContext(() =>
                {
                    data.yuEBao -= amount;
                    bankCard.money += amount;
                    TransactionSaveData actionData = new TransactionSaveData();
                    actionData.iconType = TransactionIconType.BankCard;
                    actionData.bankName = bankCard.bankName;
                    actionData.moneyType = TransactionMoneyType.NoChange;
                    actionData.remarkStr = ContentHelper.Read(ContentHelper.FinanceText);
                    actionData.timeStr = DateTime.Now.ToString();
                    actionData.detailStr = ContentHelper.Read(ContentHelper.YuEBaoText) + "-" + ContentHelper.Read(ContentHelper.OutToCard);
                    AssetsManager.Instance.AddTransactionData(actionData);
                    UIManager.Instance.Pop();
                    string str = string.Format(ContentHelper.Read(ContentHelper.YuEBaoToCardSucc), amount.ToString("0.00"),
                        Utils.FormatPaywayStr(PaywayType.BankCard, bankCard.cardId));
                    UIManager.Instance.Push(new YuEBaoOutSuccContext(str));
                }));
            }
        }

        public void OnClickClear()
        {
            _moneyInput.text = "";
        }

        public void OnClickCard()
        {
            UIManager.Instance.Push(new SelectPayWayContext(0, SpendType.ToSelfBankCard));
        }

        public void OnValueChanged(string str)
        {
            bool canUse = !string.IsNullOrEmpty(_moneyInput.text);
            if (canUse)
                canUse = double.Parse(_moneyInput.text) > 0;
            _allObj.SetActive(string.IsNullOrEmpty(_moneyInput.text));
            _clearObj.SetActive(!string.IsNullOrEmpty(_moneyInput.text));
            _confirmBtn.interactable = canUse;
        }

        private void Refresh()
        {
            BankCardSaveData data = AssetsManager.Instance.curUseBankCard;
            _cardIcon.sprite = AssetsManager.Instance.GetBankSprite(data.bankName);
            _cardText.text = Utils.FormatPaywayStr(PaywayType.BankCard, data.cardId);
            _maxMoneyText.text = string.Format(ContentHelper.Read(ContentHelper.MaxCanToCard), AssetsManager.Instance.assetsData.yuEBao);
            _fastWaySelectedObj.SetActive(_toCardTimeType == ToCardTimeType.Fast);
            _normalWaySelectedobj.SetActive(_toCardTimeType == ToCardTimeType.Normal);
        }

        private void Clear()
        {
            _moneyInput.text = "";
            _clearObj.SetActive(false);
            _confirmBtn.interactable = false;
        }
	}
	public class YuEBaoOutToCardContext : BaseContext
	{
		public YuEBaoOutToCardContext() : base(UIType.YuEBaoOutToCard)
		{
		}
	}
    public enum ToCardTimeType
    {
        Fast,
        Normal,
    }
}
