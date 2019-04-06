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
        private void OnEnable()
        {
            ChatManager.Instance.OnRefresh += OnRefreshLst;
        }
        private void OnDisable()
        {
            ChatManager.Instance.OnRefresh -= OnRefreshLst;
        }
        public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as FriendsContext;
            ChatManager.Instance.Refresh();
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
            ChatManager.Instance.Refresh();
        }
		public override void Excute ()
		{
			base.Excute ();

		}
        public void OnRefreshLst(List<ChatInstance> chatLst)
        {
            if (!gameObject.activeSelf)
                return;
            scrollView.Init(chatLst);
            for (int i = 0; i < scrollView._activeItems.Count; i++)
            {
                (scrollView._activeItems[i] as ChatItem).OnRefresh();
            }
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
        public void OnEnterChatView()
        {
            UIManager.Instance.Push(new ChatContext());
        }
    }
	public class FriendsContext : BaseContext
	{
		public FriendsContext() : base(UIType.Friends)
		{
		}
	}
}
