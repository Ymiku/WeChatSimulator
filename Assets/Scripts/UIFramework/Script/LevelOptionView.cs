using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
	
	public class LevelOptionView : AlphaView
	{
		private LevelOptionContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as LevelOptionContext;
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



		public void ResetCallBack()
		{
			if (!GameManager.Instance.CanOperate ())
				return;
			PlaySound (4);;
			UIManager.Instance.StartBlackTrans ();
			UIManager.Instance.StartUILine (UIManager.UILine.LevelMenu);
			UIManager.Instance.funcQueue.Enqueue (delegate(){

			});
		}
		public void NextCallBack()
		{
			
		}
	}
	public class LevelOptionContext :BaseContext
	{
		public  LevelOptionContext() : base(UIType.LevelOption)
		{
		}
	}
}

