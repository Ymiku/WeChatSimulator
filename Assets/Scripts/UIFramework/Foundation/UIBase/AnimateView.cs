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
        bool _isEnd;
        public override void OnEnter(BaseContext context)
        {
            if (_animator != null)
                _animator.SetTrigger("OnEnter");
            else
                gameObject.SetActive(true);

        }
        public override void OnExit(BaseContext context)
        {
            _isEnd = true;
            if (_animator != null)
                _animator.SetTrigger("OnExit");
            else
                gameObject.SetActive(false);
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
