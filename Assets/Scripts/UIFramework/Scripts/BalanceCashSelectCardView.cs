using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class BalanceCashSelectCardView : AnimateView
	{
		private BalanceCashSelectCardContext _context;
        private PoolableScrollView _scrollView;

		public override void Init ()
		{
			base.Init ();
            _scrollView = FindInChild<PoolableScrollView>("CardScrollView");
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BalanceCashSelectCardContext;
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
            _scrollView.SetDatas(AssetsManager.Instance.bankCardsData);
        }

        public void OnClickAdd()
        {
            UIManager.Instance.Push(new AddBankCardContext());
        }
	}
	public class BalanceCashSelectCardContext : BaseContext
	{
		public BalanceCashSelectCardContext() : base(UIType.BalanceCashSelectCard)
		{
		}
	}
}
