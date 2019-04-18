using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
	public class BankCardRecentView : AnimateView
    { 
        public RecentCardItem _cardPrefab;
        public RecentCardTitleItem _titlePerfab;
        public Transform _content;
        private int _titleCount = 0;
        private int _cardsCount = 0;
        private List<RecentCardItem> _cardsList = new List<RecentCardItem>();
        private List<RecentCardTitleItem> _titleList = new List<RecentCardTitleItem>();
        private BankCardRecentContext _context;

        public override void Init ()
		{
			base.Init ();
            _cardPrefab.gameObject.SetActive(false);
            _titlePerfab.gameObject.SetActive(false);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BankCardRecentContext;
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
            _titleCount = 0;
            _cardsCount = 0;
            int i = 0;
            for (; i < _cardsList.Count; i++)
            {
                _cardsList[i].gameObject.SetActive(false);
            }
            i = 0;
            for (; i < _titleList.Count; i++)
            {
                _titleList[i].gameObject.SetActive(false);
            }
            float y = 0f;
            List<BankCardSaveData> cardList = AssetsManager.Instance.GetRecentTransCardList();
            if (cardList.Count > 0)
            {
                RecentCardTitleItem titleItem = GetTitle();
                titleItem.SetData(ContentHelper.Read(ContentHelper.HistoryCard));
                titleItem.cachedRectTransform.anchoredPosition = new Vector2(0.0f, y);
                titleItem.gameObject.SetActive(true);
                y -= titleItem.height;
                for (i = 0; i < cardList.Count; i++)
                {
                    RecentCardItem cardItem = GetCard();
                    cardItem.SetData(cardList[i]);
                    cardItem.cachedRectTransform.anchoredPosition = new Vector2(0.0f, y);
                    cardItem.gameObject.SetActive(true);
                    y -= cardItem.height;
                }
            }
            cardList = AssetsManager.Instance.bankCardsData;
            if (cardList.Count > 0)
            {
                RecentCardTitleItem titleItem = GetTitle();
                titleItem.SetData(ContentHelper.Read(ContentHelper.SelfText));
                titleItem.cachedRectTransform.anchoredPosition = new Vector2(0.0f, y);
                titleItem.gameObject.SetActive(true);
                y -= titleItem.height;
                for (i = 0; i < cardList.Count; i++)
                {
                    RecentCardItem cardItem = GetCard();
                    cardItem.SetData(cardList[i]);
                    cardItem.cachedRectTransform.anchoredPosition = new Vector2(0.0f, y);
                    cardItem.gameObject.SetActive(true);
                    y -= cardItem.height;
                }
            }
        }

        private RecentCardTitleItem GetTitle()
        {
            RecentCardTitleItem item = null;
            if (_titleList.Count > _titleCount)
            {
                item = _titleList[_titleCount];
            }
            else
            {
                item = Instantiate(_titlePerfab);
                item.cachedRectTransform.SetParent(_content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                _titleList.Add(item);
            }
            _titleCount++;
            return item;
        }

        private RecentCardItem GetCard()
        {
            RecentCardItem item = null;
            if (_cardsList.Count > _cardsCount)
            {
                item = _cardsList[_cardsCount];
            }
            else
            {
                item = Instantiate(_cardPrefab);
                item.cachedRectTransform.SetParent(_content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                _cardsList.Add(item);
            }
            _cardsCount++;
            return item;
        }
	}
	public class BankCardRecentContext : BaseContext
	{
		public BankCardRecentContext() : base(UIType.BankCardRecent)
		{
		}
	}
}
