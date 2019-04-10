using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIFrameWork
{
	public class SelectPayWayView : AnimateView
	{
		private SelectPayWayContext _context;
        private PoolableScrollView _scrollView;

		public override void Init ()
		{
			base.Init ();
            _scrollView = FindInChild<PoolableScrollView>("Content/ScrollRect");
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as SelectPayWayContext;
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
            List<SelectPaywayItemData> dataList = new List<SelectPaywayItemData>();
            List<BankCardSaveData> cardList = AssetsManager.Instance.bankCardsData;
            for (int i = 0; i < cardList.Count; i++)
            {
                SelectPaywayItemData data = new SelectPaywayItemData();
                data.isAddCard = false;
                data.isEnough = true;
                data.cardId = cardList[i].cardId;
                data.payway = PaywayType.BankCard;
                data.spendType = _context.spendType;
                dataList.Add(data);
            }
            bool balanceEnough = AssetsManager.Instance.assetsData.balance >= _context.amount;
            SelectPaywayItemData balanceData = new SelectPaywayItemData();
            balanceData.isAddCard = false;
            balanceData.isEnough = balanceEnough;
            balanceData.payway = PaywayType.Banlance;
            balanceData.spendType = _context.spendType;
            bool yuEBaoEnough = AssetsManager.Instance.assetsData.yuEBao >= _context.amount && _context.spendType != SpendType.ToSelfYuEBao;
            SelectPaywayItemData yuEBaoData = new SelectPaywayItemData();
            yuEBaoData.isAddCard = false;
            yuEBaoData.isEnough = yuEBaoEnough;
            yuEBaoData.payway = PaywayType.YuEBao;
            yuEBaoData.spendType = _context.spendType;
            SelectPaywayItemData addCardData = new SelectPaywayItemData();
            addCardData.isAddCard = true;
            if (_context.spendType != SpendType.ToSelfBankCard)
            {
                if (balanceEnough)
                    dataList.Add(balanceData);
                if (yuEBaoEnough)
                    dataList.Add(yuEBaoData);
                dataList.Add(addCardData);
                if (!balanceEnough)
                    dataList.Add(balanceData);
                if (!yuEBaoEnough)
                    dataList.Add(yuEBaoData);
            }
            else
                dataList.Add(addCardData);
            _scrollView.SetDatas(dataList);
        }
	}
	public class SelectPayWayContext : BaseContext
	{
		public SelectPayWayContext() : base(UIType.SelectPayWay)
		{
		}

        public SelectPayWayContext(double amount, SpendType spendType) : base(UIType.SelectPayWay)
        {
            this.amount = amount;
            this.spendType = spendType;
        }

        public double amount;
        public SpendType spendType;
	}
}
