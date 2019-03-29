using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
    public class TransferToAccountView : AnimateView
    {
        private TransferToAccountContext _context;
        private Text _phoneNumText;
        private FInputField _inputField;
        private Button _nextBtn;
        private Button _clearBtn;
        private Button _friendBtn;

        private List<string> _allNumberList = StaticDataAccount.GetAllPhoneNumbers();

        public override void Init()
        {
            base.Init();
            _phoneNumText = FindInChild<Text>("");  //todo
            _inputField = FindInChild<FInputField>("");
            _nextBtn = FindInChild<Button>("");
            _clearBtn = FindInChild<Button>("");
            _friendBtn = FindInChild<Button>("");

            _nextBtn.onClick.AddListener(OnClickNext);
            _clearBtn.onClick.AddListener(OnClickClear);
            _friendBtn.onClick.AddListener(OnClickFriend);
            _inputField.onValueChanged.AddListener(OnInputValueChanged);
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as TransferToAccountContext;
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
        public override void Excute()
        {
            base.Excute();
        }

        private void OnInputValueChanged(string value)
        {
            _nextBtn.interactable = !string.IsNullOrEmpty(value);
        }

        public void OnClickNext()
        {
            string phoneNum = _phoneNumText.text;
            foreach (var item in _allNumberList)
            {
                if (item == phoneNum)
                {
                    if (Utils.CheckIsSelfNumber(phoneNum))
                    {
                        ShowNotice(ContentHelper.Read(ContentHelper.CanNotTransToSelf));
                        return;
                    }
                    else
                    {
                        int accountId = StaticDataAccount.GetAccountIdByNumber(phoneNum);
                        InputTransferAmountContext context = new InputTransferAmountContext(accountId);
                        UIManager.Instance.Push(context);
                        return;
                    }
                }
            }
            ShowNotice(ContentHelper.Read(ContentHelper.TransAccountNotExist));
        }

        public void OnClickFriend()
        {

        }

        public void OnClickClear()
        {
            _inputField.text = "";
        }

    }
	public class TransferToAccountContext : BaseContext
	{
		public TransferToAccountContext() : base(UIType.TransferToAccount)
		{
		}
	}
}
