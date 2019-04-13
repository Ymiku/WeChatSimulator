using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class RechargeSuccView : AnimateView
	{
		private RechargeSuccContext _context;
        private TextProxy _paywayText;
        private TextProxy _amountText;
        private GameObject _signObj;

		public override void Init ()
		{
			base.Init ();
            _paywayText = FindInChild<TextProxy>("UseItem/text");
            _amountText = FindInChild<TextProxy>("Value");
            _signObj = FindChild("sign");
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as RechargeSuccContext;
            _paywayText.text = _context.paywayStr;
            _amountText.text = _context.money.ToString();
            _signObj.transform.localPosition = new Vector3(-_amountText.preferredWidth / 2 + 26.5f,
                _signObj.transform.localPosition.y, _signObj.transform.localPosition.z);
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
	public class RechargeSuccContext : BaseContext
	{
		public RechargeSuccContext() : base(UIType.RechargeSucc)
		{
		}

        public RechargeSuccContext(string paywayStr, double money) : base(UIType.RechargeSucc)
        {
            this.paywayStr = paywayStr;
            this.money = money;
        }

        public string paywayStr;
        public double money;
    }
}
