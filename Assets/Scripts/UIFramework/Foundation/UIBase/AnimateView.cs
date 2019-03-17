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
        [SerializeField]
        protected Animator _animator;

        public override void OnEnter(BaseContext context)
        {
            if (_animator != null)
                _animator.SetTrigger("OnEnter");
        }

        public override void OnExit(BaseContext context)
        {
            if (_animator != null)
                _animator.SetTrigger("OnExit");
        }

        public override void OnPause(BaseContext context)
        {
            if (_animator != null)
                _animator.SetTrigger("OnPause");
        }

        public override void OnResume(BaseContext context)
        {
            if (_animator != null)
                _animator.SetTrigger("OnResume");
        }

	}
}
