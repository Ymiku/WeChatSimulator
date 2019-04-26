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
        private Vector2 _normalSize = new Vector2(1080, 836);

        public override void Init ()
		{
			base.Init ();
            _datePrefab.gameObject.SetActive(false);
            _detailPrefab.gameObject.SetActive(false);
            _toggleGroup.onChangedIndex = OnChangedIndex;
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as MyAntBillContext;
            _curTab = selectTab.thisMonth;
            Refresh();
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
            _curTab = selectTab.thisMonth;
            Refresh();
        }
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickRepay()
        {
            int year = DateTime.Now.Year;
            if (_curTab == selectTab.nextMonth && DateTime.Now.Month == 12)
                year += 1;
            int month = DateTime.Now.Month;
            if (_curTab == selectTab.nextMonth)
                month = month == 12 ? 1 : month + 1;
            UIManager.Instance.Push(new AntRepayContext(GetBillCount(), year, month));
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
            _remainRoot.SetActive(_curTab != selectTab.noRecord && amount > 0);
            _payOffRoot.SetActive(_curTab != selectTab.noRecord && amount == 0);
            _totalText.gameObject.SetActive(_curTab != selectTab.noRecord);
            _remainText.text = amount.ToString("0.00");
            _totalText.text = string.Format(ContentHelper.Read(ContentHelper.AntMonthCount), GetBillCount());
            int month = DateTime.Now.Month == 12 ? 1 : DateTime.Now.Month + 1;
            _deadlineText.text = string.Format(ContentHelper.Read(ContentHelper.AntDeadline), month, 9);
            _content.anchoredPosition = Vector3.zero;
            _content.sizeDelta = _normalSize;
            ClearGrid();
            ShowGrid();
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

        AntBillDateItem GetBillDate()
        {
            AntBillDateItem item = null;
            if (_dateList.Count > _dateCount)
            {
                item = _dateList[_dateCount];
            }
            else
            {
                item = Instantiate(_datePrefab);
                item.cachedRectTransform.SetParent(_content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                _dateList.Add(item);
            }
            _dateCount++;
            return item;
        }
        AntBillDetailItem GetBillDetail()
        {
            AntBillDetailItem item = null;
            if (_detailList.Count > _detailCount)
            {
                item = _detailList[_detailCount];
            }
            else
            {
                item = Instantiate(_detailPrefab);
                item.cachedRectTransform.SetParent(_content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                _detailList.Add(item);
            }
            _dateCount++;
            return item;
        }

        private void ClearGrid()
        {
            _detailCount = 0;
            _dateCount = 0;
            for (int i = 0; i < _detailList.Count; i++)
            {
                _detailList[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < _detailList.Count; i++)
            {
                _detailList[i].gameObject.SetActive(false);
            }
        }
        private void ShowGrid()
        {
            Vector2 tempVec = new Vector2(0, 0);
            if (_curTab == selectTab.noRecord)
                return;
            int dateCount = 0;
            float y = -836;
            foreach (string date in _actionsDict.Keys)
            {
                AntBillDateItem dateItem = GetBillDate();
                dateItem.SetData(date);
                y = y - dateCount * 145;
                tempVec.y = y;
                dateItem.cachedRectTransform.anchoredPosition = tempVec;
                for (int i = 0; i< _actionsDict[date].Count; i ++)
                {
                    AntBillDetailItem detailItem = GetBillDetail();
                    detailItem.SetData(_actionsDict[date][i]);
                    y = y - i * 210;
                    tempVec.y = y;
                    detailItem.cachedRectTransform.anchoredPosition = tempVec;
                    tempVec.x = 993.2f;
                    tempVec.y = 145 + (i + 1) * 210;
                    dateItem.cachedRectTransform.sizeDelta = tempVec;
                }
                y += 40;
            }
            _content.sizeDelta = new Vector2(1080, -y);
        }
    }
	public class MyAntBillContext : BaseContext
	{
		public MyAntBillContext() : base(UIType.MyAntBill)
		{
		}
	}
}
