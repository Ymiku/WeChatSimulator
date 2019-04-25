using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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
        public GameObject _noRecordRoot;
        public AntBillDateItem _datePrefab;
        public AntBillDetailItem _detailPrefab;
        public RectTransform _content;
        public FToggleGroup _toggleGroup;

        private int _dateCount = 0;
        private int _detailCount = 0;
        private List<AntBillDateItem> _dateList = new List<AntBillDateItem>();
        private List<AntBillDetailItem> _detailList = new List<AntBillDetailItem>();
        private Dictionary<string, List<TransactionSaveData>> _actionsDict = new Dictionary<string, List<TransactionSaveData>>();
        private Vector2 _normalSize = new Vector2(1080, 704);

        public override void Init ()
		{
			base.Init ();
            _curTab = selectTab.thisMonth;
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
                _actionsDict = AssetsManager.Instance.GetThisMonthAntBill();
            }
            else if (index == (int)selectTab.nextMonth)
            {
                _curTab = selectTab.nextMonth;
                _actionsDict = AssetsManager.Instance.GetNextMonthAntBill();
            }
            else
            {
                _curTab = selectTab.noRecord;
            }
            Refresh();
        }

        private void Refresh()
        {
            double amount = GetBillCount();
            _noRecordRoot.SetActive(_curTab == selectTab.noRecord);
            _remainRoot.SetActive(amount > 0);
            _payOffRoot.SetActive(amount == 0);
            _totalText.gameObject.SetActive(_curTab != selectTab.noRecord);
            _remainText.text = amount.ToString("0.00");
            _totalText.text = string.Format(ContentHelper.Read(ContentHelper.AntMonthCount), GetBillCount());
            int month = DateTime.Now.Month == 12 ? 1 : DateTime.Now.Month + 1;
            _deadlineText.text = string.Format(ContentHelper.Read(ContentHelper.AntDeadline), month, 9);
            _content.anchoredPosition = Vector3.zero;
            _content.sizeDelta = _normalSize;
            if (_curTab != selectTab.noRecord)
            {
               
            }
        }

        int GetBillCount()
        {
            int result = 0;
            if (_actionsDict != null)
                foreach (var list in _actionsDict.Values)
                    for (int i = 0; i < list.Count; i++)
                        result++;
            return result;
        }
        double GetAmount()
        {
            double result = 0;
            if(_actionsDict != null)
                foreach (var list in _actionsDict.Values)
                    for (int i = 0; i < list.Count; i++)
                        result += list[i].money;
            return result;
        }
    }
	public class MyAntBillContext : BaseContext
	{
		public MyAntBillContext() : base(UIType.MyAntBill)
		{
		}
	}
}
