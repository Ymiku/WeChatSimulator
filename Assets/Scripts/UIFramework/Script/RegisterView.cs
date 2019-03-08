using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.dto;

namespace UIFrameWork
{

	public class RegisterView : AlphaView
	{
		public Text accout;
		public Text password;
		public Text password2;
		private RegisterContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as RegisterContext;
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


		public void RegisterCallBack()
		{
			PlaySound (8);
			if (accout.text.Length == 0) {
				UIManager.Instance.Push (new NoticeContext ("WARNING!", "Please enter your account!"));
				return;
			}
			if (password.text.Length <= 4) {
				UIManager.Instance.Push (new NoticeContext ("WARNING!", "Password is too short!"));
				return;
			}
			if (password.text != password2.text) {
				UIManager.Instance.Push (new NoticeContext("WARNING!","Different password!"));
				return;
			}
			AccountInfoDTO dto = new AccountInfoDTO ();
			dto.account = accout.text;
			dto.password = GameSecurity.MD5ToString(dto.account+password.text);
			this.WriteMessage (Protocol.TYPE_LOGIN, 0, LoginProtocol.REG_CREQ, dto);
			UIManager.Instance.Loading ();
		}
	}
	public class RegisterContext :BaseContext
	{
		public RegisterContext() : base(UIType.Register)
		{
		}
	}
}

