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
			FrostRX.End(ref rxId);
			gameObject.SetActive (true);
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
			FrostRX.End(ref rxId);
			rxId = FrostRX.Start (this).ExecuteAfterTime(()=>{gameObject.SetActive (false);rxId=-1;},disappearTime).GetId();
		}
		public override void OnResume (BaseContext context)
		{
			base.OnResume (context);
			FrostRX.Instance.EndRxById (ref rxId);
			if(!activeWhenPause)
				gameObject.SetActive (true);
		}
		public override void OnPause (BaseContext context)
		{
			base.OnPause (context);
			if (!activeWhenPause) {
				FrostRX.Instance.EndRxById (ref rxId);
				rxId = FrostRX.Start (this).ExecuteAfterTime (() => {
					gameObject.SetActive (false);rxId = -1;
				}, disappearTime).GetId ();
			}
		}
        public sealed override void ForceDisable()
        {
            base.ForceDisable();
            if (activeWhenPause)
                return;
            FrostRX.End(ref rxId);
            gameObject.SetActive(false);

        }
        private void OnDestroy()
        {
            FrostRX.End(ref rxId);
        }
    }
}
