using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoInSuccView : AnimateView
	{
		private YuEBaoInSuccContext _context;
        public TextProxy _detailText;
        public TextProxy _startTimeText;
        public TextProxy _receiveTimeText;
        public TextProxy _payText;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoInSuccContext;
            _payText.text = _context.payStr;
            _detailText.text = string.Format(ContentHelper.Read(ContentHelper.YuEBaoInSucc), _context.amount);
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
	}
	public class YuEBaoInSuccContext : BaseContext
	{
		public YuEBaoInSuccContext() : base(UIType.YuEBaoInSucc)
		{
		}

        public YuEBaoInSuccContext(double amount, string payStr) : base(UIType.YuEBaoInSucc)
        {
            this.amount = amount;
            this.payStr = payStr;
        }

        public double amount;
        public string payStr;
    }
}
