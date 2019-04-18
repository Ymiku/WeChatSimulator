using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
	public class BankCardRecentView : AnimateView
    { 
        public RecentCardItem _cardPrefab;
        public RecentCardTitleItem _titleItem;
        public Transform _content;
        private int _titleCount = 0;
        private int _itemCount = 0;
        private List<RecentCardItem> _cardsList = new List<RecentCardItem>();
        private List<RecentCardTitleItem> _titleList = new List<RecentCardTitleItem>();
        private BankCardRecentContext _context;

        public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BankCardRecentContext;
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
	}
	public class BankCardRecentContext : BaseContext
	{
		public BankCardRecentContext() : base(UIType.BankCardRecent)
		{
		}
	}
}
