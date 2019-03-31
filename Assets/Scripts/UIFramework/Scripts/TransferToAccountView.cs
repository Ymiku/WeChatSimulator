using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using static_data;

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

        public override void Init()
        {
            base.Init();
            _phoneNumText = FindInChild<Text>("Top/InputField/Text");
            _inputField = FindInChild<FInputField>("Top/InputField");
            _nextBtn = FindInChild<Button>("Next");
            _clearBtn = FindInChild<Button>("Top/Clear");
            _friendBtn = FindInChild<Button>("Top/Img");

            _nextBtn.onClick.AddListener(OnClickNext);
            _clearBtn.onClick.AddListener(OnClickClear);
            _friendBtn.onClick.AddListener(OnClickFriend);
            _inputField.onValueChanged.AddListener(OnInputValueChanged);
            _clearBtn.gameObject.SetActive(false);
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
            _inputField.text = "";
            _clearBtn.gameObject.SetActive(false);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }
        public override void Excute()
        {
            base.Excute();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnInputValueChanged(string value)
        {
            _nextBtn.interactable = !string.IsNullOrEmpty(value);
            _clearBtn.gameObject.SetActive(!string.IsNullOrEmpty(value));
        }

        public void OnClickNext()
        {
            string phoneNum = _phoneNumText.text.Replace(" ", "");
            if (Utils.CheckPhoneNumberExist(phoneNum))
            {
                if (Utils.CheckIsSelfNumber(phoneNum))
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.CanNotTransToSelf));
                }
                else {
                    AccountSaveData data = XMLSaver.saveData.GetAccountDataByPhoneNumber(phoneNum);
                    InputTransferAmountContext context = new InputTransferAmountContext(data);
                    UIManager.Instance.Push(context);
                }
            }
            else
            {
                ShowNotice(ContentHelper.Read(ContentHelper.TransAccountNotExist));
            }
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
