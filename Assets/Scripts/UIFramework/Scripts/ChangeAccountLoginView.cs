using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class ChangeAccountLoginView : AlphaView
	{
        public ChangeAccountItem[] items;
		private ChangeAccountLoginContext _context;

		public override void Init ()
		{
			base.Init ();
		}
        void UpdateView()
        {
            int num = XMLSaver.saveData.canLoginUserIds.Count;
            int i = 0;
            for (; i < num; i++)
            {
                items[i].gameObject.SetActive(true);
                items[i].SetUser(XMLSaver.saveData.canLoginUserIds[i]);
            }
            for (; i < items.Length; i++)
            {
                items[i].gameObject.SetActive(false);
            }
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ChangeAccountLoginContext;
            UpdateView();
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
            UpdateView();
		}
		public override void Excute ()
		{
			base.Excute ();
		}
        public void OnClickNewAccount()
        {
            UIManager.Instance.Push(new LoginByPhoneNumberContext());
        }
	}
	public class ChangeAccountLoginContext : BaseContext
	{
		public ChangeAccountLoginContext() : base(UIType.ChangeAccountLogin)
		{
		}
	}
}
