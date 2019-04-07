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
        private Button _backBtn;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GameObject[] _lengthObj = new GameObject[6];
        private Button[] _numberBtn = new Button[10];
        private int _rxId = -1;

        public override void Init()
        {
            base.Init();
            int i = 0;
            for (; i < 6; i++)
            {
                int index = i + 1;
                GameObject obj = FindChild("Content/payword/" + index);
                _lengthObj[i] = obj;
                obj.SetActive(false);
            }
            i = 0;
            for (; i < 10; i++)
            {
                Button btn = FindInChild<Button>("Content/keypad/Button" + i);
                _numberBtn[i] = btn;
                int number = i;
                btn.onClick.AddListener(() => { OnClickNumber(number); });
            }
            _deleteBtn = FindInChild<Button>("Content/keypad/Delete");
            _deleteBtn.onClick.AddListener(OnClickDelete);
            _backBtn = FindInChild<Button>("Content/Top/Close");
            _backBtn.onClick.AddListener(PopCallBack);
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
            Clear();
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }
        public override void Excute()
        {
            base.Excute();
        }

        private void OnClickNumber(int i)
        {
            if (_stringBuilder.Length == 5)
            {
                _backBtn.interactable = false;
                _deleteBtn.interactable = false;
                for (int index = 0; index < 9; index++)
                {
                    _numberBtn[index].interactable = false;
                }
                _stringBuilder.Append(i);
                _lengthObj[_stringBuilder.Length - 1].SetActive(true);
                _rxId = FrostRX.Instance.StartRX().ExecuteAfterTime(() =>
                {
                    if (GameManager.Instance.accountData.payword == _stringBuilder.ToString())
                    {
                        Clear();
                        UIManager.Instance.Pop();
                        _context.callback();
                    }
                    else
                    {
                        Clear();
                        ShowNotice(ContentHelper.Read(ContentHelper.PaywordError));
                    }
                }, 0.5f).GetId();
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
            FrostRX.End(ref _rxId);
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
