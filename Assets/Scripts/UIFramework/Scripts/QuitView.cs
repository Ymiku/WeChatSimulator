using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class QuitView : AlphaView
	{
		private QuitContext _context;
        public float count = 2.0f;
        float _count = 0.0f;
		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as QuitContext;
            UIManager.Instance.isQuit = true;
            _count = count;
        }

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            UIManager.Instance.isQuit = false;
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
            _count -= Time.deltaTime;
            if (_count <= 0.0f)
                PopCallBack();
		}
	}
	public class QuitContext : BaseContext
	{
		public QuitContext() : base(UIType.Quit)
		{
		}
	}
}
