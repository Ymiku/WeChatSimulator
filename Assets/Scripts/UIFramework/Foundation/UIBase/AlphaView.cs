using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIFrameWork
{
	public abstract class AlphaView : BaseView 
	{
		public bool alwaysSee = true;
		public float alphaSpeed = 20f;
		private bool _show = false;
		private bool _isEnter = false;
		private CanvasGroup _canvasGroup;
		public override void Init ()
		{
			base.Init ();
			_canvasGroup = GetComponent<CanvasGroup> ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter (context);
			_canvasGroup.alpha = 0f;
			_show = true;
			_isEnter = true;
			_canvasGroup.blocksRaycasts = true;
			gameObject.SetActive (true);
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
			_show = false;
			_isEnter = false;
			_canvasGroup.blocksRaycasts = false;
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause (context);
			_canvasGroup.blocksRaycasts = false;
			if (!alwaysSee) {
				_show = false;
			}
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume (context);
			_canvasGroup.blocksRaycasts = true;
			gameObject.SetActive (true);
			if (!alwaysSee) {
				_show = true;
			}
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
	}
}
