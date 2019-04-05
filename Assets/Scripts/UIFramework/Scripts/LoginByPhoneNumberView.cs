using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class LoginByPhoneNumberView : AlphaView
	{
        public GameObject eye;
        public FInputField phone;
        public InputField password;
        public Button loginButton;
		private LoginByPhoneNumberContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as LoginByPhoneNumberContext;
            eye.SetActive(true);
            password.contentType = InputField.ContentType.Password;
            loginButton.interactable = (phone.text.Length != 0 && password.text.Length != 0);
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
        public void OnClickEye()
        {
            if (eye.activeSelf)
            {
                eye.SetActive(false);
                password.contentType = InputField.ContentType.Standard;
            }
            else
            {
                eye.SetActive(true);
                password.contentType = InputField.ContentType.Password;
            }
            password.ActivateInputField();
        }
        public void OnClickLogin()
        {
            AccountSaveData data = XMLSaver.saveData.GetAccountDataByPhoneNumber(phone.GetText());
            if (data == null)
            {
                ShowNotice("’À∫≈≤ª¥Ê‘⁄ªÚ√‹¬Î¥ÌŒÛ");
                return;
            }
            if (data.password != password.text)
            {
                ShowNotice("’À∫≈≤ª¥Ê‘⁄ªÚ√‹¬Î¥ÌŒÛ");
                return;
            }
            GameManager.Instance.SetUser(data.accountId);
            UIManager.Instance.Push(new LoginContext());
        }
        public void OnValueChange()
        {
            loginButton.interactable = (phone.text.Length != 0 && password.text.Length != 0);
        }
	}
	public class LoginByPhoneNumberContext : BaseContext
	{
		public LoginByPhoneNumberContext() : base(UIType.LoginByPhoneNumber)
		{
		}
	}
}
