using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class TransactionsView : AnimateView
	{
		private TransactionsContext _context;
        public PoolableScrollView _scrollView;
        public Text _expendText;
        public Text _incomeText;

		public override void Init ()
		{
			base.Init ();
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
		public override void Excute ()
		{
			base.Excute ();
		}

        private void Refresh()
        {
            _expendText.text = ContentHelper.Read(ContentHelper.ExpendText) + " " + ContentHelper.Read(ContentHelper.RMBSign) +
                " " + AssetsManager.Instance.GetExpendTotalMoney().ToString();
            _incomeText.text = ContentHelper.Read(ContentHelper.IncomeText) + " " + ContentHelper.Read(ContentHelper.RMBSign) + 
                " " + AssetsManager.Instance.GetIncomeTotalMoney().ToString();
            _scrollView.SetDatas(AssetsManager.Instance.assetsData.transactionList);
        }
    }
	public class TransactionsContext : BaseContext
	{
		public TransactionsContext() : base(UIType.Transactions)
		{
		}
	}
}
