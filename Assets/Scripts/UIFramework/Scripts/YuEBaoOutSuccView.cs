using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoOutSuccView : AnimateView
	{
		private YuEBaoOutSuccContext _context;
        private TextProxy _detailText;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutSuccContext;
            _detailText.text = _context.detailStr;
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
	public class YuEBaoOutSuccContext : BaseContext
	{
		public YuEBaoOutSuccContext() : base(UIType.YuEBaoOutSucc)
		{
		}

        public YuEBaoOutSuccContext(string detailStr) : base(UIType.YuEBaoOutSucc)
        {
            this.detailStr = detailStr;
        }

        public string detailStr;
	}
}
