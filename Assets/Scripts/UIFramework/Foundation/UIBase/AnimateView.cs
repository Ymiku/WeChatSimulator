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
		int rxId = -1;
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
			gameObject.SetActive(true);
			_isPause = false;
			FrostRX.Instance.EndRxById (rxId);
			switch (fadeType) {
			case AnimateFadeType.Horizon:
				rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=true;_canvasGroup.alpha=0.0f;gameObject.SetActive(true);offsetX=1080.0f;}).
					ExecuteUntil(()=>{offsetX = Mathf.Lerp(offsetX,-0.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,1.01f,_fadeTime*Time.deltaTime);if(offsetX<=0.0f)offsetX=0.0f;},
						()=>{return offsetX==0.0f&&_canvasGroup.alpha>=1.0f;}).Execute(()=>{rxId=-1;}).GetId();
				break;
			case AnimateFadeType.Vertical:
				rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=true;_canvasGroup.alpha=0.0f;gameObject.SetActive(true);offsetY=-1920.0f;}).
					ExecuteUntil(()=>{offsetY = Mathf.Lerp(offsetY,0.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,1.01f,_fadeTime*Time.deltaTime);if(offsetY>=0.0f)offsetY=0.0f;},
						()=>{return offsetY==0.0f&&_canvasGroup.alpha>=1.0f;}).Execute(()=>{rxId=-1;}).GetId();
				break;
			}
		}
		public override void OnExit(BaseContext context)
		{
			base.OnExit (context);
			_isPause = true;

			FrostRX.Instance.EndRxById (rxId);
			switch (fadeType) {
			case AnimateFadeType.Horizon:
				rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=false;}).
					ExecuteUntil(()=>{offsetX = Mathf.Lerp(offsetX,1080.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,-0.01f,_fadeTime*Time.deltaTime);if(offsetX>=1080.0f)offsetX=1080.0f;},
						()=>{return offsetX==1080.0f&&_canvasGroup.alpha<=0.0f;}).Execute(()=>{rxId=-1;gameObject.SetActive(false);}).GetId();
				break;
			case AnimateFadeType.Vertical:
				rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=false;}).
					ExecuteUntil(()=>{offsetY = Mathf.Lerp(offsetY,-1920.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,-0.01f,_fadeTime*Time.deltaTime);if(offsetY<=-1920.0f)offsetY=-1920.0f;},
						()=>{return offsetY==-1920.0f&&_canvasGroup.alpha<=0.0f;}).Execute(()=>{rxId=-1;gameObject.SetActive(false);}).GetId();
				break;
			}
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause (context);
			if (!activeWhenPause) {
				FrostRX.End (rxId);
				switch (fadeType) {
				case AnimateFadeType.Horizon:
					rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=false;}).
						ExecuteUntil(()=>{offsetX = Mathf.Lerp(offsetX,-1080.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,-0.01f,_fadeTime*Time.deltaTime);if(offsetX<=-1080.0f)offsetX=-1080.0f;},
							()=>{return offsetX==-1080.0f&&_canvasGroup.alpha<=0.0f;}).Execute(()=>{rxId=-1;gameObject.SetActive(false);}).GetId();
					break;
				case AnimateFadeType.Vertical:
					rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=false;}).
						ExecuteUntil(()=>{offsetY = Mathf.Lerp(offsetY,-1920.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,-0.01f,_fadeTime*Time.deltaTime);if(offsetY<=-1920.0f)offsetY=-1920.0f;},
							()=>{return offsetY==-1920.0f&&_canvasGroup.alpha<=0.0f;}).Execute(()=>{rxId=-1;gameObject.SetActive(false);}).GetId();
					break;
				}
			}
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume (context);
			if (!activeWhenPause) {
				FrostRX.End(rxId);
				switch (fadeType) {
				case AnimateFadeType.Horizon:
					rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=true;gameObject.SetActive(true);}).
						ExecuteUntil(()=>{offsetX = Mathf.Lerp(offsetX,0.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,1.01f,_fadeTime*Time.deltaTime);if(offsetX>=0.0f)offsetX=0.0f;},
							()=>{return offsetX==0.0f&&_canvasGroup.alpha>=1.0f;}).Execute(()=>{rxId=-1;}).GetId();
					break;
				case AnimateFadeType.Vertical:
					rxId = FrostRX.Start(this).Execute(()=>{_canvasGroup.blocksRaycasts=true;gameObject.SetActive(true);}).
						ExecuteUntil(()=>{offsetY = Mathf.Lerp(offsetY,0.1f,_fadeTime*Time.deltaTime);_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,1.01f,_fadeTime*Time.deltaTime);if(offsetY>=0.0f)offsetY=0.0f;},
							()=>{return offsetY==0.0f&&_canvasGroup.alpha>=1.0f;}).Execute(()=>{rxId=-1;}).GetId();
					break;
				}
			}
		}
	}
}