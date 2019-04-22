using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class MyAntBillView : AnimateView
	{
		private MyAntBillContext _context;
        public TextProxy _thisMonth;
        public TextProxy _nextMonth;
        public TextProxy _remainText;
        public TextProxy _deadlineText;
        public TextProxy _totalText;
        public GameObject _payOffRoot;
        public GameObject _remainRoot;
        public GameObject _thisMonthSelect;
        public GameObject _nextMonthSelect;

        public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as MyAntBillContext;
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

        public void OnClickThisMonth()
        {

        }

        public void OnClickNextMonth()
        {

        }

        private void Refresh()
        {

        }
	}
	public class MyAntBillContext : BaseContext
	{
		public MyAntBillContext() : base(UIType.MyAntBill)
		{
		}
	}
}
