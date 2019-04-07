using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

namespace UIFrameWork
{
	public class ConfirmPaywordView : AnimateView
	{
		private ConfirmPaywordContext _context;
        private Button _deleteBtn;
        private Button _backBtn;
        private Button _nextBtn;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GameObject[] _lengthObj = new GameObject[6];
        private Button[] _numberBtn = new Button[10];
        private int _newPayword;

        public override void Init ()
		{
			base.Init ();
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
            _nextBtn = FindInChild<Button>("Next");
            _deleteBtn.onClick.AddListener(OnClickDelete);
            _nextBtn.onClick.AddListener(OnClickNext);
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ConfirmPaywordContext;
            Clear();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            Clear();
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
        public override void PopCallBack()
        {
            base.PopCallBack();
            UIManager.Instance.Push(new ChangePaywordContext());
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
                _nextBtn.interactable = true;
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
            _nextBtn.interactable = false;
            _newPayword = 0;
            for (int index = 0; index < 10; index++)
            {
                _numberBtn[index].interactable = true;
            }
            _stringBuilder.Remove(0, _stringBuilder.Length);
            for (int i = 0; i < 6; i++)
            {
                _lengthObj[i].SetActive(false);
            }
        }

        private void OnClickNext()
        {
            if (int.Parse(_stringBuilder.ToString()) == _newPayword)
            {
                GameManager.Instance.accountData.payword = _newPayword;
                ShowNotice("修改支付密码成功");
                UIManager.Instance.Pop();
            }
            else
            {
                ShowNotice("两次输入密码不一致");
                Clear();
            }
        }
    }
	public class ConfirmPaywordContext : BaseContext
	{
		public ConfirmPaywordContext() : base(UIType.ConfirmPayword)
		{
		}

        public ConfirmPaywordContext(int payword) : base(UIType.ConfirmPayword)
        {
            this.payword = payword;
        }

        public int payword;
	}
}
