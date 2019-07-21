using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIFrameWork
{
	public abstract class AlphaView : BaseView 
	{
		public float alphaSpeed = 20f;
		private bool _show = false;
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter (context);
			_canvasGroup.alpha = 0f;
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause (context);
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume (context);
		}
        public override bool ShowUI()
        {
            if (!base.ShowUI())
                return false;
            _show = true;
            gameObject.SetActive(true);
            return true;
        }
        public override bool HideUI()
        {
            if (!base.HideUI())
                return false;
            _show = false;
            return true;
        }
        public override void Excute ()
		{
			base.Excute ();
			if (_show && _canvasGroup.alpha < 1f) {
				_canvasGroup.alpha = Mathf.Lerp (_canvasGroup.alpha, 1f, alphaSpeed * Time.deltaTime);
			}
			if (!_show) {
				_canvasGroup.alpha = Mathf.Lerp (_canvasGroup.alpha, 0f, alphaSpeed * Time.deltaTime);
				if (_canvasGroup.alpha <= 0.01f) {
					gameObject.SetActive (false);
				}
			}
		}
        public sealed override void ForceDisable()
        {
            base.ForceDisable();
            _show = false;
            _canvasGroup.alpha = 0.0f;
            gameObject.SetActive(false);
        }
    }
}
