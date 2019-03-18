using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class TransactionsView : AnimateView
	{
		private TransactionsContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TransactionsContext;
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
	public class TransactionsContext : BaseContext
	{
		public TransactionsContext() : base(UIType.Transactions)
		{
		}
	}
}
