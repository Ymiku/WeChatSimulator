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
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
		}
		public override void OnResume (BaseContext context)
		{
			base.OnResume (context);
			
		}
		public override void OnPause (BaseContext context)
		{
			base.OnPause (context);
		}
        public override bool ShowUI()
        {
            if (!base.ShowUI())
                return false;
            FrostRX.End(rxId);
            gameObject.SetActive(true);
            return true;
        }
        public override bool HideUI()
        {
            if (!base.HideUI())
                return false;
            FrostRX.End(rxId);
            FrostRX.Start(this).ExecuteAfterTime(() => {
                gameObject.SetActive(false);
            }, disappearTime).GetId(rxId);
            return true;
        }
        public sealed override void ForceDisable()
        {
            base.ForceDisable();
            FrostRX.End(rxId);
            gameObject.SetActive(false);

        }
        private void OnDestroy()
        {
            FrostRX.End(rxId);
        }
    }
}
