using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
    public class TransferView : AlphaView
    {
        private TransferContext _context;
        public PoolableScrollView _scrollView;

        public override void Init()
        {
            base.Init();
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as TransferContext;
            _scrollView.SetDatas(AssetsManager.Instance.GetRecentTransList());
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
        public void OnClickTransferToAccount()
        {
            UIManager.Instance.Push(new TransferToAccountContext());
        }
        public void OnClickTransferToCard()
        {
            UIManager.Instance.Push(new TransferToBankCardContext());
        }
    }
	public class TransferContext : BaseContext
	{
		public TransferContext() : base(UIType.Transfer)
		{
		}
	}
}
