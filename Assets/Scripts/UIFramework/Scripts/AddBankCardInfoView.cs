using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class AddBankCardInfoView : AnimateView
	{
		private AddBankCardInfoContext _context;
        private Text _phoneNumText;
        private Text _bankText;
        private GameObject _cardTextObj;
        private Button _agreeBtn;

        public override void Init ()
		{
			base.Init ();
            _phoneNumText = FindInChild<Text>("PhoneNum/PhoneText");
            _bankText = FindInChild<Text>("CardType/BankText");
            _cardTextObj = FindChild("CardType/CardText");
            _agreeBtn = FindInChild<Button>("AgreeBtn");
            _agreeBtn.onClick.AddListener(OnClickAgree);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AddBankCardInfoContext;
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
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        private void OnClickAgree()
        {
            XMLSaver.saveData.AddBankCardData(GameManager.Instance.curUserId, _context.cardId);
            Player.Instance.UpdateCardsList();
            ShowNotice("绑卡成功");
            // todo 绑卡成功
        }

        private void Refresh()
        {
            _phoneNumText.text = Utils.FormatStringForSecrecy(Player.Instance.accountData.phoneNumber, FInputType.PhoneNumber);
            _bankText.text = StaticDataBankCard.GetCardNameById(_context.cardId).Replace(
                ContentHelper.Read(ContentHelper.SavingCardText), "");
            _cardTextObj.transform.localPosition = new Vector3(_bankText.transform.localPosition.x + _bankText.preferredWidth,
                _cardTextObj.transform.localPosition.y, _cardTextObj.transform.localPosition.z);
        }
	}
	public class AddBankCardInfoContext : BaseContext
	{
		public AddBankCardInfoContext() : base(UIType.AddBankCardInfo)
		{
		}

        public AddBankCardInfoContext(string cardId) : base(UIType.AddBankCardInfo)
        {
            this.cardId = cardId;
        }

        public string cardId;
    }
}
