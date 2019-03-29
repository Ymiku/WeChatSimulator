using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class RegistByPhoneNumberView : AlphaView
	{
		private RegistByPhoneNumberContext _context;
        public Text phoneNumber;
		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as RegistByPhoneNumberContext;
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
        public void OnClickRegist()
        {
			string num = phoneNumber.text.Replace (" ","");
            if (num.Length != 11)
            {
                ShowNotice("输入格式错误！");
                return;
            }
			GameManager.Instance.ClearData ();
            AccountSaveData data = XMLSaver.saveData.AddAccountData(0);
			data.phoneNumber = num;
			UIManager.Instance.Push(new LoginContext(){userId = 0});
        }
        public void OnClickAgreement()
        {
			UIManager.Instance.Push(new RegistAgreementContext());
        }
    }
	public class RegistByPhoneNumberContext : BaseContext
	{
		public RegistByPhoneNumberContext() : base(UIType.RegistByPhoneNumber)
		{
		}
	}
}
