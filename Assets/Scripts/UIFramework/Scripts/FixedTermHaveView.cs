using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class FixedTermHaveView : AnimateView
	{
		private FixedTermHaveContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as FixedTermHaveContext;
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

        public void OnClickMarketBtn()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new FixedTermContext());
        }
    }
	public class FixedTermHaveContext : BaseContext
	{
		public FixedTermHaveContext() : base(UIType.FixedTermHave)
		{
		}
	}
}
