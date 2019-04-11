using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class TransferToBankCardView : AnimateView
	{
		private TransferToBankCardContext _context;
        public FInputField _nameField;
        public FInputField _cardField;
        public FInputField _moneyField;
        public Button _nextBtn;
        public TextProxy _nextText;

		public override void Init ()
		{
			base.Init ();
            _nameField.onValueChanged.AddListener(OnNameValueChanged);
            _cardField.onValueChanged.AddListener(OnCardValueChanged);
            _moneyField.onValueChanged.AddListener(OnMoneyValueChanged);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TransferToBankCardContext;
            RefreshBtnStatus();
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
            RefreshBtnStatus();
        }
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickNextBtn()
        {
            string cardId = _cardField.text.Replace(" ", "");
            BankCardSaveData cardData = XMLSaver.saveData.GetBankCardData(cardId);
            if (cardData != null)
            {
                if (cardData.realName != _nameField.text)
                    ShowNotice(ContentHelper.Read(ContentHelper.NameNotMatchCard));
                else
                {
                    UIManager.Instance.Push(new TransferToBankCardConfirmContext(double.Parse(_moneyField.text),
                        _nameField.text, cardId));
                }
            }
            else
                ShowNotice(ContentHelper.Read(ContentHelper.CardNotSupport));
        }

        public void OnNameValueChanged(string str)
        {
            RefreshBtnStatus();
        }

        public void OnCardValueChanged(string str)
        {
            RefreshBtnStatus();
        }

        public void OnMoneyValueChanged(string str)
        {
            RefreshBtnStatus();
        }

        private void RefreshBtnStatus()
        {
            bool cardForbid = true;
            if (!string.IsNullOrEmpty(_cardField.text))
            {
                string cardId = _cardField.text.Replace(" ", "");
                cardForbid = cardId.Length > BankCardDefine.cardIdMinLength && cardId.Length < BankCardDefine.cardIdMaxLength;
            }
            bool moneyForbid = true;
            if (!string.IsNullOrEmpty(_moneyField.text))
            {
                double money = double.Parse(_moneyField.text);
                moneyForbid = money <= 0;
            }
            if (!string.IsNullOrEmpty(_nameField.text) && !cardForbid && !moneyForbid)
            {
                _nextBtn.interactable = true;
                _nextText.text = string.Format(GameDefine.NormalTextColor, _nextText.text);
            }
            else
            {
                _nextBtn.interactable = false;
                _nextText.text = string.Format(GameDefine.ForbidTextColor, _nextText.text);
            }
        }

        private void Clear()
        {
            _nameField.text = "";
            _cardField.text = "";
            _moneyField.text = "";
        }
    }
	public class TransferToBankCardContext : BaseContext
	{
		public TransferToBankCardContext() : base(UIType.TransferToBankCard)
		{
		}
	}
}
