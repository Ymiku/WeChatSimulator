using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
	public class MyAntBillView : AnimateView
	{
        private enum selectTab
        {
            thisMonth,
            nextMonth,
            noRecord,
        }
        private selectTab _curTab;
		private MyAntBillContext _context;
        public TextProxy _thisMonth;
        public TextProxy _nextMonth;
        public TextProxy _remainText;
        public TextProxy _deadlineText;
        public TextProxy _totalText;
        public GameObject _payOffRoot;
        public GameObject _remainRoot;
        public GameObject _anrecordRoot;
        public AntBillDateItem _datePrefab;
        public AntBillDetailItem _detailPrefab;
        public Transform _content;
        public FToggleGroup _toggleGroup;

        private int _dateCount = 0;
        private int _detailCount = 0;
        private List<AntBillDateItem> _dateList = new List<AntBillDateItem>();
        private List<AntBillDetailItem> _detailList = new List<AntBillDetailItem>();
        private Dictionary<string, List<TransactionSaveData>> _actionsList = new Dictionary<string, List<TransactionSaveData>>();

        public override void Init ()
		{
			base.Init ();
            _toggleGroup.onChangedIndex = OnChangedIndex;
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

        public void OnChangedIndex(int index)
        {
            if (index == (int)selectTab.thisMonth)
            {
                _curTab = selectTab.thisMonth;
                _actionsList = AssetsManager.Instance.GetThisMonthAntActionData();
            }
            else if (index == (int)selectTab.nextMonth)
            {
                _curTab = selectTab.nextMonth;
                _actionsList = AssetsManager.Instance.GetNextMonthAntActionData();
            }
            else
            {
                _curTab = selectTab.noRecord;
            }
            Refresh();
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
