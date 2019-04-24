using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
namespace UIFrameWork
{
	public class TweetView : AlphaView
	{
		public ProceduralImage head;
		private TweetContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TweetContext;
			Refresh ();
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
			Refresh ();
		}
		public override void Excute ()
		{
			base.Excute ();
		}
        void Refresh()
        {
			AccountSaveData data = XMLSaver.saveData.GetAccountData (_context.userId);
			HeadSpriteUtils.Instance.SetHead (head,_context.userId);
			List<int> friends = XMLSaver.saveData.GetFriendsLst (_context.userId);
        }
	}
	public class TweetContext : BaseContext
	{
        public int userId;
		public TweetContext() : base(UIType.Tweet)
		{
		}
	}
}
