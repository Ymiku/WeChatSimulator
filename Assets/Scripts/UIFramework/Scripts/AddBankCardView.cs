using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class AddBankCardView : AnimateView
	{
		private AddBankCardContext _context;
        private Text _nameText;
        private FInputField _cardInput;
        private Button _nextBtn;

		public override void Init ()
		{
			base.Init ();
            _nameText = FindInChild<Text>("Mid/name");
            _cardInput = FindInChild<FInputField>("Mid/cardNum/InputField");
            _nextBtn = FindInChild<Button>("Mid/nextBtn");
            _nextBtn.onClick.AddListener(OnClickNextBtn);
            _cardInput.onValueChanged.AddListener(OnValueChanged);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AddBankCardContext;
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

        private void OnValueChanged(string str)
        {
            _nextBtn.interactable = !string.IsNullOrEmpty(str);
        }

        public void OnClickNextBtn()
        {
            string cardId = _cardInput.text;
            UIManager.Instance.Push(new AddBankCardInfoContext(cardId));
        }

        private void Refresh()
        {
            _nameText.text = Player.Instance.accountData.realname;
        }
	}
	public class AddBankCardContext : BaseContext
	{
		public AddBankCardContext() : base(UIType.AddBankCard)
		{
		}
	}
}
