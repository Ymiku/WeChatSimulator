using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class LoginView : AlphaView
	{
        public ImageProxy headSprite;
        public TextProxy phoneNumber;
		private LoginContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		void UpdateView()
		{
			AccountSaveData data = XMLSaver.saveData.GetAccountData (GameManager.Instance.curUserId);
			phoneNumber.text = Utils.FormatStringForSecrecy (data.phoneNumber,FInputType.PhoneNumber);
			headSprite.sprite = data.GetHeadSprite();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as LoginContext;
			UpdateView ();
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
			UpdateView ();
		}
		public override void Excute ()
		{
			base.Excute ();
		}
		public void OnClickLogin()
		{
			UIManager.Instance.StartUILine(UIManager.UILine.Main);
			UIManager.Instance.Push(new HomeContext());
		}
		public void OnClickChangeAccountLogin()
		{
			if (XMLSaver.saveData.canLoginUserIds.Count <= 1) {
				UIManager.Instance.Push (new LoginByPhoneNumberContext ());
				return;
			}
			UIManager.Instance.Push(new ChangeAccountLoginContext());
		}
	}
	public class LoginContext : BaseContext
	{
		public LoginContext() : base(UIType.Login)
		{
			
		}
	}
}
