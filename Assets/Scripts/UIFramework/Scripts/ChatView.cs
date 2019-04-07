using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class ChatView : AlphaView
	{
        public PoolableFScrollView scrollView;
		private ChatContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ChatContext;
            ChatManager.Instance.EnterChat(_context.friendName);
            scrollView.Init();
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
	public class ChatContext : BaseContext
	{
        public string friendName;
		public ChatContext() : base(UIType.Chat)
		{
		}
	}
}
