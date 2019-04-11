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
            _nextBtn.interactable = false;
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

            if (CheckCardLegal(cardId))
                if (CheckCardHasBind(cardId))
                    ShowNotice(ContentHelper.Read(ContentHelper.CardAlreadyBind));
                else
                {
                    UIManager.Instance.Pop();
                    UIManager.Instance.Push(new AddBankCardInfoContext(cardId));
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
            _nameText.text = GameManager.Instance.accountData.realname;
        }

        private bool CheckCardLegal(string cardId)
        {
            if (AssetsManager.Instance.CheckCardExist(cardId))
                return false;
            if (string.IsNullOrEmpty(cardId))
                return false;
            else if (cardId.Length > BankCardDefine.cardIdMaxLength || cardId.Length < BankCardDefine.cardIdMinLength)
                return false;
            //模10算法 检查合法性
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

        private bool CheckCardHasBind(string cardId)
        {
            for (int i = 0; i < AssetsManager.Instance.bankCardsData.Count; i++) {
                if (AssetsManager.Instance.bankCardsData[i].cardId == cardId)
                    return true;
            }
            return false;
        }
	}
	public class AddBankCardContext : BaseContext
	{
		public AddBankCardContext() : base(UIType.AddBankCard)
		{
		}
	}
}
