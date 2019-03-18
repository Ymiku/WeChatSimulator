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
        public Animator animator;
        public override void OnEnter(BaseContext context)
        {
            _isPause = false;
            if (animator != null)
                animator.SetTrigger("OnEnter");
            else
                gameObject.SetActive(true);

        }
        public override void OnExit(BaseContext context)
        {
            _isPause = true;
            if (animator != null)
                animator.SetTrigger("OnExit");
            else
                gameObject.SetActive(false);
        }

        public override void OnPause(BaseContext context)
        {
            _isPause = true;
            if (animator != null)
                animator.SetTrigger("OnPause");
        }

        public override void OnResume(BaseContext context)
        {
            _isPause = false;
            if (animator != null)
                animator.SetTrigger("OnResume");
        }

	}
}
