using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.dto;

namespace UIFrameWork
{

	public class LogInView : AlphaView
	{
		public Text accout;
		public Text password;
		private LogInContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as LogInContext;
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

		public void LoginInCallBack()
		{
			PlaySound (8);
			if (accout.text.Length == 0 || password.text.Length <= 4) {
				UIManager.Instance.Push (new NoticeContext ("WARNING!", "Account or password format is incorrect"));
				return;
			}
			AccountInfoDTO dto = new AccountInfoDTO ();
			dto.account = accout.text;
			dto.password = GameSecurity.MD5ToString(dto.account+password.text);
			this.WriteMessage (Protocol.TYPE_LOGIN, 0, LoginProtocol.LOGIN_CREQ, dto);
			UIManager.Instance.Loading ();
		}
		public void RegisterCallBack()
		{
			PlaySound (8);
			UIManager.Instance.Push (new RegisterContext());
		}
		public void DemoCallBack()
		{
			PlaySound (8);
			UIManager.Instance.Push (new GamePlayContext());
		}
	}
	public class LogInContext :BaseContext
	{
		public  LogInContext() : base(UIType.LogIn)
		{
		}
	}
}

