using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public abstract class EnabledView : BaseView 
	{
		float disappearTime = 4.0f;
		int rxId = -1;
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter (context);
			FrostRX.End(rxId);
			rxId = -1;
			gameObject.SetActive (true);
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
			FrostRX.End(rxId);
			rxId = FrostRX.Start (this).ExecuteAfterTime(()=>{gameObject.SetActive (false);rxId=-1;},disappearTime).GetId();
		}
		public override void OnResume (BaseContext context)
		{
			base.OnResume (context);
			FrostRX.Instance.EndRxById (rxId);
			rxId = -1;
			if(!activeWhenPause)
				gameObject.SetActive (true);
		}
		public override void OnPause (BaseContext context)
		{
			base.OnPause (context);
			if (!activeWhenPause) {
				FrostRX.Instance.EndRxById (rxId);
				rxId = FrostRX.Start (this).ExecuteAfterTime (() => {
					gameObject.SetActive (false);rxId = -1;
				}, disappearTime).GetId ();
			}
		}
        private void OnDestroy()
        {
            FrostRX.End(rxId);
            rxId = -1;
        }
    }
}
