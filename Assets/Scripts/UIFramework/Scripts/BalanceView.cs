using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class BalanceView : AnimateView
	{
		private BalanceContext _context;
        private TextProxy _moneyText;

		public override void Init ()
		{
			base.Init ();
            _moneyText = FindInChild<TextProxy>("Panel/Top/Balance/Value");
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BalanceContext;
            _moneyText.text = AssetsManager.Instance.assetsData.balance.ToString();
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
            _moneyText.text = AssetsManager.Instance.assetsData.balance.ToString();
        }
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickRecharge()
        {
            UIManager.Instance.Push(new BalanceRechargeContext());
        }

        public void OnClickCash()
        {
            if (AssetsManager.Instance.bankCardsData.Count == 0)
            {
                ShowNotice(ContentHelper.Read(ContentHelper.PleaseBindBankCard));
                UIManager.Instance.Push(new AddBankCardContext());
                return;
            }
            UIManager.Instance.Push(new BalanceCashContext());
        }
    }
	public class BalanceContext : BaseContext
	{
		public BalanceContext() : base(UIType.Balance)
		{
		}
	}
}
