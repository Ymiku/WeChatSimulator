using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public abstract class EnabledView : BaseView 
	{
		public override void OnEnter(BaseContext context)
		{
			gameObject.SetActive (true);
		}

		public override void OnExit(BaseContext context)
		{
			gameObject.SetActive (false);
		}
	}
}
