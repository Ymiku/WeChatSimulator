using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public abstract class EnabledView : BaseView 
	{
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter (context);
			gameObject.SetActive (true);
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
			gameObject.SetActive (false);
		}
		public override void OnResume (BaseContext context)
		{
			base.OnResume (context);
			if(!activeWhenPause)
				gameObject.SetActive (true);
		}
		public override void OnPause (BaseContext context)
		{
			base.OnPause (context);
			if(!activeWhenPause)
				gameObject.SetActive (false);
		}
	}
}
