using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class BalanceCashSuccView : AnimateView
	{
		private BalanceCashSuccContext _context;
        private TextProxy _paywayText;
        private Text _amountText;
        private GameObject _signObj;

        public override void Init ()
		{
			base.Init ();
            _paywayText = FindInChild<TextProxy>("UseItem/wayText");
            _amountText = FindInChild<Text>("Value");
            _signObj = FindChild("sign");
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BalanceCashSuccContext;
            _paywayText.text = _context.cardStr;
            _amountText.text = _context.money.ToString("0.00");
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
	public class BalanceCashSuccContext : BaseContext
	{
		public BalanceCashSuccContext() : base(UIType.BalanceCashSucc)
		{
		}

        public BalanceCashSuccContext(string cardStr, double money) : base(UIType.BalanceCashSucc)
        {
            this.cardStr = cardStr;
            this.money = money;
        }

        public string cardStr;
        public double money;
    }
}
