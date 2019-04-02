using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class AddBankCardInfoView : AnimateView
	{
		private AddBankCardInfoContext _context;
        private Button _agreeBtn;

		public override void Init ()
		{
			base.Init ();
            _agreeBtn = FindInChild<Button>("AgreeBtn");
            _agreeBtn.onClick.AddListener(OnClickAgree);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AddBankCardInfoContext;
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

        private void OnClickAgree()
        {

        }
	}
	public class AddBankCardInfoContext : BaseContext
	{
		public AddBankCardInfoContext() : base(UIType.AddBankCardInfo)
		{
		}

        public AddBankCardInfoContext(string cardId) : base(UIType.AddBankCard)
        {
            this.cardId = cardId;
        }

        public string cardId;
    }
}
