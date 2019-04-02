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
        private Button _clearBtn;
        private GameObject _cameraBtnObj;

		public override void Init ()
		{
			base.Init ();
            _nameText = FindInChild<Text>("Mid/name");
            _cardInput = FindInChild<FInputField>("Mid/cardNum/InputField");
            _nextBtn = FindInChild<Button>("Mid/nextBtn");
            _clearBtn = FindInChild<Button>("Mid/cardNum/InputField/Clear");
            _cameraBtnObj = FindChild("Mid/img");
            _nextBtn.onClick.AddListener(OnClickNextBtn);
            _clearBtn.onClick.AddListener(OnClickClear);
            _cardInput.onValueChanged.AddListener(OnValueChanged);
            _clearBtn.gameObject.SetActive(false);
            _clearBtn.interactable = false;
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
            Refresh();
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        private void OnValueChanged(string str)
        {
            _nextBtn.interactable = !string.IsNullOrEmpty(str) && str.Length >= 14;
            _clearBtn.gameObject.SetActive(!string.IsNullOrEmpty(str));
            _cameraBtnObj.SetActive(string.IsNullOrEmpty(str));
        }

        public void OnClickNextBtn()
        {
            string cardId = _cardInput.text.Replace(" ", "");
            if (cardId.Length == BankCardDefine.cardIdLength)
            {
                if (CheckCardLegal(cardId))
                    UIManager.Instance.Push(new AddBankCardInfoContext(cardId));
                else
                    ShowNotice(ContentHelper.Read(ContentHelper.BankCardIllegal));
            }
            else
                ShowNotice(ContentHelper.Read(ContentHelper.BankCardIllegal));
        }

        public void OnClickClear()
        {
            _cardInput.text = "";
        }

        private void Refresh()
        {
            _cardInput.text = "";
            _nameText.text = Player.Instance.accountData.realname;
        }

        private bool CheckCardLegal(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
                return false;
            else if (cardId.Length != BankCardDefine.cardIdLength)
                return false;
            int sum = 0;
            int len = cardId.Length;
            for (int i = 0; i < len; i++)
            {
                int add = (cardId[i] - '0') * (2 - (i + len) % 2);
                add -= add > 9 ? 9 : 0;
                sum += add;
            }
            return sum % 10 == 0;
        }
	}
	public class AddBankCardContext : BaseContext
	{
		public AddBankCardContext() : base(UIType.AddBankCard)
		{
		}
	}
}
