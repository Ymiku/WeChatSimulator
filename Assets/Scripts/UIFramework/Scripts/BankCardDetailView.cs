using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class BankCardDetailView : AnimateView
	{
		private BankCardDetailContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BankCardDetailContext;
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
	public class BankCardDetailContext : BaseContext
	{
		public BankCardDetailContext() : base(UIType.BankCardDetail)
		{
		}
	}
}
