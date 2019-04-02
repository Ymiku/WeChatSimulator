using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class AddBankCardInfoView : AnimateView
	{
		private AddBankCardInfoContext _context;

		public override void Init ()
		{
			base.Init ();
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
