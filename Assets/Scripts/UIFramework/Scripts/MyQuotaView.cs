using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class MyQuotaView : AnimateView
	{
		private MyQuotaContext _context;
        public TextProxy _canUseVal;
        public TextProxy _alreadyUseVal;
        public TextProxy _totalVal;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as MyQuotaContext;
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
            _canUseVal.text = AssetsManager.Instance.assetsData.antPay.ToString("0.00");
            _alreadyUseVal.text = (GameDefine.AntLimit - AssetsManager.Instance.assetsData.antPay).ToString("0.00");
            _totalVal.text = GameDefine.AntLimit.ToString("0.00");
        }
	}
	public class MyQuotaContext : BaseContext
	{
		public MyQuotaContext() : base(UIType.MyQuota)
		{
		}
	}
}
