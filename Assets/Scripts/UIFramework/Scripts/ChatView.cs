using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class ChatView : AlphaView
	{
        public PoolableFScrollView scrollView;
        public TextProxy friendName;
		private ChatContext _context;

		public override void Init ()
		{
			base.Init ();
            scrollView.Init();
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ChatContext;
            ChatManager.Instance.EnterChat(_context.friendId);
            scrollView.OnEnter();
            friendName.text = ChatManager.Instance.curInstance.friendData.GetAnyName();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            scrollView.OnExit();
            ChatManager.Instance.ExitChat();
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
	public class ChatContext : BaseContext
	{
        public int friendId;
		public ChatContext() : base(UIType.Chat)
		{
		}
	}
}
