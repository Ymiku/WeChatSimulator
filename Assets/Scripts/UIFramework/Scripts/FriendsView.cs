using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UIFrameWork
{
	public class FriendsView : EnabledView
	{
        public PoolableScrollView scrollView;
		private FriendsContext _context;
        public GameObject requestRedPoint;
		public override void Init ()
		{
			base.Init ();
		}
        public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as FriendsContext;
            ChatManager.Instance.OnNewMsgOccur += RefreshMsg;
            ChatManager.Instance.RefreshMsg();
            requestRedPoint.SetActive(ChatManager.Instance.HasRequestToHandle());
        }

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            ChatManager.Instance.OnNewMsgOccur -= RefreshMsg;
        }

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            ChatManager.Instance.OnNewMsgOccur -= RefreshMsg;
        }

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
            ChatManager.Instance.OnNewMsgOccur += RefreshMsg;
            ChatManager.Instance.RefreshMsg();
            requestRedPoint.SetActive(ChatManager.Instance.HasRequestToHandle());
        }
		public override void Excute ()
		{
			base.Excute ();

		}
        public void RefreshMsg(List<ChatInstance> chatLst)
        {
            if (!gameObject.activeSelf)
                return;
			scrollView.SetDatas (chatLst);
        }
        public void OnClickHome()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new HomeContext());
        }
        public void OnClickFortune()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FortuneContext());
        }
        public void OnClickKoubei()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new KoubeiContext());
        }
        public void OnClickFriends()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FriendsContext());
        }
        public void OnClickMe()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new MeContext());
        }
        public void OnClickContacts()
        {
            UIManager.Instance.Push(new ContactsContext());
        }
        public void OnClickSearch()
        {
            UIManager.Instance.Push(new SearchContext());
        }
		public void OnClickAlbum()
		{
			UIManager.Instance.Push (new AlbumContext());
		}
		public void OnClickTweet()
		{
			UIManager.Instance.Push (new TweetContext());
		}
		public void OnClickBlog()
		{
			UIManager.Instance.Push (new BlogContext());
		}
    }
	public class FriendsContext : BaseContext
	{
		public FriendsContext() : base(UIType.Friends)
		{
		}
	}
}
