using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
    public class TransactionsView : AnimateView
    {
        private TransactionsContext _context;
        public PoolableScrollView _scrollView;
        public Text _expendText;
        public Text _incomeText;
        private List<TransactionSaveData> _dataList = new List<TransactionSaveData>();

        public override void Init()
        {
            base.Init();
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as TransactionsContext;
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
            Refresh();
        }
        public override void Excute()
        {
            base.Excute();
        }

        private void Refresh()
        {
            _expendText.text = ContentHelper.Read(ContentHelper.ExpendText) + " " + ContentHelper.Read(ContentHelper.RMBSign) +
                " " + AssetsManager.Instance.GetExpendTotalMoney().ToString();
            _incomeText.text = ContentHelper.Read(ContentHelper.IncomeText) + " " + ContentHelper.Read(ContentHelper.RMBSign) +
                " " + AssetsManager.Instance.GetIncomeTotalMoney().ToString();
            _dataList.Clear();
            for (int i = AssetsManager.Instance.assetsData.transactionList.Count - 1; i >= 0; i--)
                _dataList.Add(AssetsManager.Instance.assetsData.transactionList[i]);
            _scrollView.SetDatas(_dataList);
        }
    }
	public class TransactionsContext : BaseContext
	{
		public TransactionsContext() : base(UIType.Transactions)
		{
		}
	}
}
