using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class AntCreditView : AnimateView
	{
		private AntCreditContext _context;
        public TextProxy _canUseValue;
        public TextProxy _canUseValue2;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AntCreditContext;
            Refresh();
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
            Refresh();
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        private void Refresh()
        {
            _canUseValue.text = AssetsManager.Instance.assetsData.antPay.ToString("0.00");
            _canUseValue2.text = ContentHelper.Read(ContentHelper.AvailableCredit) + " " + AssetsManager.Instance.assetsData.antPay.ToString("0.00");
        }

        public void OnClickDetail()
        {
            UIManager.Instance.Push(new MyQuotaContext());
        }
	}
	public class AntCreditContext : BaseContext
	{
		public AntCreditContext() : base(UIType.AntCredit)
		{
		}
	}
}
