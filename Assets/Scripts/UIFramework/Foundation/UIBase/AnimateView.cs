using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIFrameWork
{
	public abstract class AnimateView : BaseView 
	{
		public enum AnimateFadeType
		{
			Horizon,
			Vertical
		}
		public AnimateFadeType fadeType = AnimateFadeType.Vertical;
		float _fadeTime = 8.0f;
        RXIndex rxId = new RXIndex();
		float offsetY{
			get{return rootRect.offsetMax.y; }
			set{ 
				rootRect.offsetMax = new Vector2 (rootRect.offsetMax.x, value);
				rootRect.offsetMin = new Vector2 (rootRect.offsetMin.x,value);
			}
		}
		float offsetX{
			get{return rootRect.anchoredPosition.x; }
			set{ 
				rootRect.anchoredPosition = new Vector2 (value,rootRect.anchoredPosition.y);
			}
		}
		RectTransform rootRect;
		void Awake()
		{
			rootRect = GetComponent<RectTransform> ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter (context);	
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
            FrostRX.Instance.EndRxById(rxId);
            switch (fadeType)
            {
                case AnimateFadeType.Horizon:
                    FrostRX.Start(this).Execute(() => { _canvasGroup.alpha = 0.0f; gameObject.SetActive(true); offsetX = 1080.0f; }).
                        ExecuteUntil(() => { offsetX = Mathf.Lerp(offsetX, -0.1f, _fadeTime * Time.deltaTime); _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1.01f, _fadeTime * Time.deltaTime); if (offsetX <= 0.0f) offsetX = 0.0f; },
                            () => { return offsetX == 0.0f && _canvasGroup.alpha >= 1.0f; }).GetId(rxId);
                    break;
                case AnimateFadeType.Vertical:
                    FrostRX.Start(this).Execute(() => { _canvasGroup.alpha = 0.0f; gameObject.SetActive(true); offsetY = -1920.0f; }).
                        ExecuteUntil(() => { offsetY = Mathf.Lerp(offsetY, 0.1f, _fadeTime * Time.deltaTime); _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1.01f, _fadeTime * Time.deltaTime); if (offsetY >= 0.0f) offsetY = 0.0f; },
                            () => { return offsetY == 0.0f && _canvasGroup.alpha >= 1.0f; }).GetId(rxId);
                    break;
            }
            return true;
        }
        public override bool HideUI()
        {
            if (!base.HideUI())
                return false;
            FrostRX.End(rxId);
            switch (fadeType)
            {
                case AnimateFadeType.Horizon:
                    FrostRX.Start(this).
                        ExecuteUntil(() => { offsetX = Mathf.Lerp(offsetX, -1080.1f, _fadeTime * Time.deltaTime); _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, -0.01f, _fadeTime * Time.deltaTime); if (offsetX <= -1080.0f) offsetX = -1080.0f; },
                            () => { return offsetX == -1080.0f && _canvasGroup.alpha <= 0.0f; }).Execute(() => { gameObject.SetActive(false); }).GetId(rxId);
                    break;
                case AnimateFadeType.Vertical:
                    FrostRX.Start(this).
                        ExecuteUntil(() => { offsetY = Mathf.Lerp(offsetY, -1920.1f, _fadeTime * Time.deltaTime); _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, -0.01f, _fadeTime * Time.deltaTime); if (offsetY <= -1920.0f) offsetY = -1920.0f; },
                            () => { return offsetY == -1920.0f && _canvasGroup.alpha <= 0.0f; }).Execute(() => { gameObject.SetActive(false); }).GetId(rxId);
                    break;
            }
            return true;
        }
        private void OnDestroy()
        {
            FrostRX.End(rxId);
        }
        public sealed override void ForceDisable()
        {
            base.ForceDisable();
            FrostRX.End(rxId);
            offsetX = 0;
            offsetY = 0;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
    }
}