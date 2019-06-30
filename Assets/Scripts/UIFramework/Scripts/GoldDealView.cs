using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class GoldDealView : AnimateView
	{
		private GoldDealContext _context;

        public TextProxy myGold;
        public TextProxy yesterdayProfit;
        public TextProxy haveProfit;
        public TextProxy accProfit;
        public TextProxy boShiGold;
        public TextProxy boShiUnit;
        public TextProxy boShiTips;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as GoldDealContext;
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

        public void OnClickMyDingTou()
        {

        }

        public void OnClickBuyRightNow()
        {

        }

        void Refresh()
        {
            myGold.text = FortuneManager.Instance.fortuneData.gold.ToString("0.0000");
            yesterdayProfit.text = FortuneManager.Instance.fortuneData.yesterdayGoldProfit.ToString("0.00");
            accProfit.text = FortuneManager.Instance.fortuneData.totalGoldProfit.ToString("0.00");
            haveProfit.text = FortuneManager.Instance.fortuneData.nowHaveGoldProfit.ToString("0.00");
        }
	}
	public class GoldDealContext : BaseContext
	{
		public GoldDealContext() : base(UIType.GoldDeal)
		{
		}
	}
}
