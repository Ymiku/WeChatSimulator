using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public abstract class EnabledView : BaseView 
	{
		float disappearTime = 2.0f;
		RXIndex rxId = new RXIndex();
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter (context);
			FrostRX.End(rxId);
			gameObject.SetActive (true);
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
			FrostRX.End(rxId);
			FrostRX.Start (this).ExecuteAfterTime(()=>{ gameObject.SetActive(false); },disappearTime).GetId(rxId);
		}
		public override void OnResume (BaseContext context)
		{
			base.OnResume (context);
			FrostRX.End(rxId);
			if(!activeWhenPause)
				gameObject.SetActive (true);
		}
		public override void OnPause (BaseContext context)
		{
			base.OnPause (context);
			if (!activeWhenPause) {
				FrostRX.End (rxId);
				FrostRX.Start (this).ExecuteAfterTime (() => {
					gameObject.SetActive (false);
				}, disappearTime).GetId (rxId);
			}
		}
        public sealed override void ForceDisable()
        {
            base.ForceDisable();
            if (activeWhenPause)
                return;
            FrostRX.End(rxId);
            gameObject.SetActive(false);

        }
        private void OnDestroy()
        {
            FrostRX.End(rxId);
        }
    }
}
