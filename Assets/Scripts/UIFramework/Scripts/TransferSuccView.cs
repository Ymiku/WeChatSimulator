using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class TransferSuccView : AnimateView
	{
		private TransferSuccContext _context;
        private Text _amountText;
        private Text _payWayText;
        private Text _receiverText;
        private Button _okBtn;
        private Button _receiverBtn;
        private GameObject _signObj;

        public override void Init ()
		{
			base.Init ();
            _amountText = FindInChild<Text>("Value");
            _payWayText = FindInChild<Text>("UseItem/wayText");
            _receiverText = FindInChild<Text>("ReceiveItem/nameText");
            _okBtn = FindInChild<Button>("OkText");
            _receiverBtn = FindInChild<Button>("ReceiveItem");
            _signObj = FindChild("sign");
            _okBtn.onClick.AddListener(OnClickOkBtn);
            _receiverBtn.onClick.AddListener(OnClickReceiver);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TransferSuccContext;
            _payWayText.text = _context.paywayStr;
            _amountText.text = _context.amount.ToString("0.00");
            _signObj.transform.localPosition = new Vector3(-_amountText.preferredWidth/2 + 26.5f,
                _signObj.transform.localPosition.y, _signObj.transform.localPosition.z);
            _receiverText.text = _context.receiverStr;
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

        public void OnClickOkBtn()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FriendsContext());
        }

        public void OnClickReceiver()
        {

        }
	}
	public class TransferSuccContext : BaseContext
	{
		public TransferSuccContext() : base(UIType.TransferSucc)
		{
		}

        public TransferSuccContext(double amount, string paywayStr, string receiverStr, string remarksStr = "") :
            base(UIType.TransferSucc)
        {
            this.amount = amount;
            this.paywayStr = paywayStr;
            this.remarksStr = remarksStr;
            this.receiverStr = receiverStr;
        }

        public double amount;
        public string paywayStr;
        public string remarksStr;  //×ªÕË±¸×¢
        public string receiverStr;
	}
}
