using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class NewFriendsView : AnimateView
	{
		private NewFriendsContext _context;
        public PoolableScrollView scroll;
		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as NewFriendsContext;
            scroll.SetDatas<int>(ChatManager.Instance.GetFriendRequests());
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
	public class NewFriendsContext : BaseContext
	{
		public NewFriendsContext() : base(UIType.NewFriends)
		{
		}
	}
}
