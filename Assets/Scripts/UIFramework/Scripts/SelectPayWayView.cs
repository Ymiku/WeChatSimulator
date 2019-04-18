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
            GetBankCardData(ref dataList);
            SelectPaywayItemData balanceData = GetBalanceData();
            SelectPaywayItemData yuEBaoData = GetYuEBaoData();
            SelectPaywayItemData antData = GetAntData();
            SelectPaywayItemData addCardData = GetAddCardData();
            switch (_context.spendType)
            {
                case SpendType.ToSelfBankCard:
                    dataList.Add(addCardData);
                    break;
                case SpendType.CanUseAnt:
                    if (balanceData.isCanUse)
                        dataList.Add(balanceData);
                    if (yuEBaoData.isCanUse)
                        dataList.Add(yuEBaoData);
                    if (antData.isCanUse)
                        dataList.Add(antData);
                    dataList.Add(addCardData);
                    if (!balanceData.isCanUse)
                        dataList.Add(balanceData);
                    if (!yuEBaoData.isCanUse)
                        dataList.Add(yuEBaoData);
                    if (!antData.isCanUse)
                        dataList.Add(antData);
                    break;
                default:
                    if (balanceData.isCanUse)
                        dataList.Add(balanceData);
                    if (yuEBaoData.isCanUse)
                        dataList.Add(yuEBaoData);
                    dataList.Add(addCardData);
                    if (!balanceData.isCanUse)
                        dataList.Add(balanceData);
                    if (!yuEBaoData.isCanUse)
                        dataList.Add(yuEBaoData);
                    break;
            }
            _scrollView.SetDatas(dataList);
        }


        private void GetBankCardData(ref List<SelectPaywayItemData> waysData)
        {
            List<BankCardSaveData> cardList = AssetsManager.Instance.bankCardsData;
            for (int i = 0; i < cardList.Count; i++)
            {
                SelectPaywayItemData data = new SelectPaywayItemData();
                data.isAddCard = false;
                data.isCanUse = true;
                data.cardId = cardList[i].cardId;
                data.payway = PaywayType.BankCard;
                data.spendType = _context.spendType;
                waysData.Add(data);
            }
        }
        private SelectPaywayItemData GetBalanceData()
        {
            SelectPaywayItemData data = new SelectPaywayItemData();
            bool canUse = AssetsManager.Instance.assetsData.balance >= _context.amount && _context.spendType != SpendType.ToSelfAssets;
            data.isAddCard = false;
            data.isCanUse = canUse;
            data.payway = PaywayType.Balance;
            data.spendType = _context.spendType;
            return data;
        }
        private SelectPaywayItemData GetYuEBaoData()
        {
            SelectPaywayItemData data = new SelectPaywayItemData();
            bool canUse = AssetsManager.Instance.assetsData.yuEBao >= _context.amount &&
                _context.spendType != SpendType.ToSelfYuEBao && _context.spendType != SpendType.ToSelfAssets;
            data.isAddCard = false;
            data.isCanUse = canUse;
            data.payway = PaywayType.YuEBao;
            data.spendType = _context.spendType;
            return data;
        }
        private SelectPaywayItemData GetAntData()
        {
            SelectPaywayItemData data = new SelectPaywayItemData();
            bool canUse = AssetsManager.Instance.assetsData.antPay >= _context.amount && _context.spendType == SpendType.CanUseAnt;
            data.isAddCard = false;
            data.isCanUse = canUse;
            data.payway = PaywayType.Ant;
            data.spendType = _context.spendType;
            return data;
        }
        private SelectPaywayItemData GetAddCardData()
        {
            SelectPaywayItemData data = new SelectPaywayItemData();
            data.isAddCard = true;
            return data;
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
