using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoOutToCardSuccView : AnimateView
	{
		private YuEBaoOutToCardSuccContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutToCardSuccContext;
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
	public class YuEBaoOutToCardSuccContext : BaseContext
	{
		public YuEBaoOutToCardSuccContext() : base(UIType.YuEBaoOutToCardSucc)
		{
		}
	}
}
