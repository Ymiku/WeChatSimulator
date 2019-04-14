using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UIFrameWork
{
	public class FriendsView : EnabledView
	{
        public PoolableScrollView scrollView;
		private FriendsContext _context;

		public override void Init ()
		{
			base.Init ();
		}
        public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as FriendsContext;
            ChatManager.Instance.OnRefreshChatLst += OnRefreshLst;
            ChatManager.Instance.RefreshChatLst();
            
        }

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            ChatManager.Instance.OnRefreshChatLst -= OnRefreshLst;
        }

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            ChatManager.Instance.OnRefreshChatLst -= OnRefreshLst;
        }

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
            ChatManager.Instance.OnRefreshChatLst += OnRefreshLst;
            ChatManager.Instance.RefreshChatLst();
            
        }
		public override void Excute ()
		{
			base.Excute ();

		}
        public void OnRefreshLst(List<ChatInstance> chatLst)
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
    }
	public class FriendsContext : BaseContext
	{
		public FriendsContext() : base(UIType.Friends)
		{
		}
	}
}
