using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
namespace UIFrameWork
{
	public class ChangePaywordView : EnabledView
	{
		private ChangePaywordContext _context;
        private TextProxy _accountText;
        private Button _deleteBtn;
        private Button _backBtn;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GameObject[] _lengthObj = new GameObject[6];
        private Button[] _numberBtn = new Button[10];
        private int _rxId;

        public override void Init ()
		{
			base.Init ();
            _accountText = FindInChild<TextProxy>("num");
            int i = 0;
            for (; i < 6; i++)
            {
                int index = i + 1;
                GameObject obj = FindChild("payword/" + index);
                _lengthObj[i] = obj;
                obj.SetActive(false);
            }
            i = 0;
            for (; i < 10; i++)
            {
                Button btn = FindInChild<Button>("keypad/Button" + i);
                _numberBtn[i] = btn;
                int number = i;
                btn.onClick.AddListener(() => { OnClickNumber(number); });
            }
            _backBtn = FindInChild<Button>("BackBtn");
            _deleteBtn = FindInChild<Button>("keypad/Delete");
            _deleteBtn.onClick.AddListener(OnClickDelete);
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ChangePaywordContext;
            Clear();
            Refresh();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            Clear();
        }

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        private void OnClickNumber(int i)
        {
            if (_stringBuilder.Length == 5)
            {
                _deleteBtn.interactable = false;
                for (int index = 0; index < 9; index++)
                {
                    _numberBtn[index].interactable = false;
                }
                _stringBuilder.Append(i);
                _lengthObj[_stringBuilder.Length - 1].SetActive(true);

                _rxId = FrostRX.Instance.StartRX().ExecuteAfterTime(() =>
                {
                    if (GameManager.Instance.accountData.payword.ToString() != _stringBuilder.ToString())
                    {
                        Clear();
                        UIManager.Instance.Push(new ConfirmPaywordContext(int.Parse(_stringBuilder.ToString())));
                        UIManager.Instance.Pop();
                    }
                    else
                    {
                        Clear();
                        ShowNotice(ContentHelper.Read(ContentHelper.PaywordMustNew));
                    }
                }, 0.15f).GetId();
            }
            else if (_stringBuilder.Length < 5)
            {
                _stringBuilder.Append(i);
                _lengthObj[_stringBuilder.Length - 1].SetActive(true);
            }
        }

        private void OnClickDelete()
        {
            if (_stringBuilder.Length > 0)
            {
                _lengthObj[_stringBuilder.Length - 1].SetActive(false);
                _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
            }
        }

        private void Clear()
        {
            _backBtn.interactable = true;
            _deleteBtn.interactable = true;
            for (int index = 0; index < 10; index++)
            {
                _numberBtn[index].interactable = true;
            }
            _stringBuilder.Remove(0, _stringBuilder.Length);
            for (int i = 0; i < 6; i++)
            {
                _lengthObj[i].SetActive(false);
            }
            FrostRX.Instance.EndRxById(ref _rxId);
        }

        private void Refresh()
        {
            _accountText.text = Utils.FormatStringForSecrecy(GameManager.Instance.accountData.phoneNumber, FInputType.PhoneNumber);
        }
    }
	public class ChangePaywordContext : BaseContext
	{
		public ChangePaywordContext() : base(UIType.ChangePayword)
		{
		}
	}
}
