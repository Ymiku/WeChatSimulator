using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

namespace UIFrameWork
{
	public class InputAndCheckPaywordView : AnimateView
	{
		private InputAndCheckPaywordContext _context;
        private Button _deleteBtn;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GameObject[] _lengthObj = new GameObject[6];

        public override void Init()
        {
            base.Init();
            int i = 0;
            for (; i < 5; i++)
            {
                GameObject obj = FindChild("Content/payword/" + (i + 1));
                _lengthObj[i] = obj;
                obj.SetActive(false);
            }
            i = 0;
            for (; i < 9; i++)
            {
                Button btn = FindInChild<Button>("Content/keypad/Button" + i);
                int number = i;
                btn.onClick.AddListener(() => { OnClickNumber(number); });
            }
            _deleteBtn = FindInChild<Button>("Content/keypad/Delete");
            _deleteBtn.onClick.AddListener(OnClickDelete);
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as InputAndCheckPaywordContext;
            Clear();
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

        private void OnClickNumber(int i)
        {
            if (_stringBuilder.Length == 6)
            {
                if (GameManager.Instance.accountData.password == _stringBuilder.ToString())
                {
                    Clear();
                    _context.callback();
                    UIManager.Instance.Pop();
                }
                else
                {
                    ShowNotice("ÃÜÂë²»ÕýÈ·");
                    Clear();
                }
            }
            else
            {
                _stringBuilder.Append(i);
                _lengthObj[_stringBuilder.Length - 1].SetActive(true);
            }
        }

        private void OnClickDelete()
        {
            if (_stringBuilder.Length > 0)
            {
                _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
                _lengthObj[_stringBuilder.Length - 1].SetActive(false);
            }
        }

        private void Clear()
        {
            _stringBuilder.Remove(0, _stringBuilder.Length);
            for (int i = 0; i < 5; i++)
            {
                _lengthObj[i].SetActive(false);
            }
        }
	}
	public class InputAndCheckPaywordContext : BaseContext
	{
		public InputAndCheckPaywordContext() : base(UIType.InputAndCheckPayword)
		{
		}

        public InputAndCheckPaywordContext(Callback callback) : base(UIType.InputAndCheckPayword)
        {
            this.callback = callback;
        }

        public delegate void Callback();
        public Callback callback;
    }
}
