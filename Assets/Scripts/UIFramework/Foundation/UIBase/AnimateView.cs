using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Base Animate View
 *
 *	by Xuanyi
 *
 */

namespace UIFrameWork
{
	public abstract class AnimateView : BaseView 
    {
		int rxId = -1;
        public Animator animator;
        public override void OnEnter(BaseContext context)
        {
			base.OnEnter (context);
			gameObject.SetActive(true);
            _isPause = false;
            if (animator != null)
                animator.SetTrigger("OnEnter");
        }
        public override void OnExit(BaseContext context)
        {
			base.OnExit (context);
            _isPause = true;
			if (animator != null) {
				animator.SetTrigger ("OnExit");
				FrostRX.End (rxId);
				rxId = FrostRX.Start (this).ExecuteAfterTime(()=>{gameObject.SetActive(false);rxId=-1;},3.0f).GetId();
			}
            else
                gameObject.SetActive(false);
        }

        public override void OnPause(BaseContext context)
        {
			base.OnPause (context);
            if (animator != null)
                animator.SetTrigger("OnPause");
			if (!activeWhenPause) {
				FrostRX.End (rxId);
				rxId = FrostRX.Start (this).ExecuteAfterTime(()=>{gameObject.SetActive(false);rxId=-1;},3.0f).GetId();
			}
        }

        public override void OnResume(BaseContext context)
        {
			base.OnResume (context);
            if (animator != null)
                animator.SetTrigger("OnResume");
			if (!activeWhenPause)
				gameObject.SetActive (true);
			FrostRX.End (rxId);
			rxId = -1;
        }
		void OnDisable()
		{
			FrostRX.End (rxId);
		}
	}
}
