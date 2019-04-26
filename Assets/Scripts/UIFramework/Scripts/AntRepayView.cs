using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class AntRepayView : AnimateView
	{
		private AntRepayContext _context;
        public TextProxy _money;
        public TextProxy _canRepay;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AntRepayContext;
            _money.text = _context.money.ToString("0.00");
            _canRepay.text = string.Format(ContentHelper.Read(ContentHelper.AntCanRepay), _context.money.ToString("0.00"));
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
            _money.text = _context.money.ToString("0.00");
            _canRepay.text = string.Format(ContentHelper.Read(ContentHelper.AntCanRepay), _context.money.ToString("0.00"));
        }
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickPay()
        {
            UIManager.Instance.Push(new ConfirmPaymentContext(_context.money));
        }
	}
	public class AntRepayContext : BaseContext
	{
		public AntRepayContext() : base(UIType.AntRepay)
		{
		}

        public AntRepayContext(double money) : base(UIType.AntRepay)
        {
            this.money = money;
        }
        public double money;
    }
}
