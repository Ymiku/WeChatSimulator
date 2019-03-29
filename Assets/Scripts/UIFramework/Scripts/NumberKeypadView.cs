using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UIFrameWork
{
    public class NumberKeypadView : AnimateView
    {
        private NumberKeypadContext _context;
        private List<Button> _numberBtnList = new List<Button>();
        private Button _pointBtn;
        private Button _deleteBtn;
        private Button _okBtn;
        private Button _hideBtn;

        public override void Init()
        {
            base.Init();
            for (int i = 0; i < 9; i++)
            {
                Button btn = FindInChild<Button>("number" + i);
                int index = i;
                btn.onClick.AddListener(() => { OnClickNumber(index); });
                _numberBtnList.Add(btn);
            }
            _pointBtn = FindInChild<Button>("");
            _pointBtn.onClick.AddListener(OnClickPoint);
            _deleteBtn = FindInChild<Button>("");
            _deleteBtn.onClick.AddListener(OnClickDelete);
            _okBtn = FindInChild<Button>("");
            _okBtn.onClick.AddListener(PopCallBack);
            _hideBtn = FindInChild<Button>("");
            _hideBtn.onClick.AddListener(PopCallBack);
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as NumberKeypadContext;
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

        private void OnEnable() {
            EventFactory.numberKeypadEM.AddListener(NumberKeypadEvent.Clear, DisableOkBtn);
        }

        private void OnDisable()
        {
            EventFactory.numberKeypadEM.AddListener(NumberKeypadEvent.Clear, DisableOkBtn);
        }

        public void OnClickNumber(int number)
        {
            if (_okBtn.interactable == false)
            {
                _okBtn.interactable = true;
            }
            EventFactory.numberKeypadEM.TriggerEvent(NumberKeypadEvent.Input, new EventArgs(number.ToString()));
        }

        public void OnClickPoint()
        {
            if (_okBtn.interactable == false)
            {
                _okBtn.interactable = true;
            }
            EventFactory.numberKeypadEM.TriggerEvent(NumberKeypadEvent.Input, new EventArgs("."));
        }

        public void OnClickDelete()
        {
            EventFactory.numberKeypadEM.TriggerEvent(NumberKeypadEvent.Delete);
        }

        private void DisableOkBtn()
        {
            _okBtn.interactable = false;
        }
    }
	public class NumberKeypadContext : BaseContext
	{
		public NumberKeypadContext() : base(UIType.NumberKeypad)
		{
		}
	}
}
