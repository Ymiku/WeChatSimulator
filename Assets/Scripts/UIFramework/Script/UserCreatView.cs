using UnityEngine;
using System.Collections;
using GameProtocol;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class UserCreatView : AlphaView
	{
		public Text userName;
		private UserCreatContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as UserCreatContext;
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
		public void CreatUserCallBack()
		{
			if (userName.text.Length == 0) {
				UIManager.Instance.Push (new NoticeContext ("WARNING!", "Please enter your name!"));
				return;
			}
			this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.CREATE_CREQ,userName.text);

		}
	}
	public class UserCreatContext :BaseContext
	{
		public UserCreatContext() : base(UIType.UserCreat)
		{
		}
	}
}
