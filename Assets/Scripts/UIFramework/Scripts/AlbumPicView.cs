using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class AlbumPicView : AlphaView
	{
		public ImageProxy disPlay;
		public RectTransform contextTrans;
		public ImageProxy left;
		public ImageProxy right;
		private AlbumPicContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AlbumPicContext;
			Refresh ();
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
			Refresh ();
		}
        bool CanScroll(float scroll)
        {
            if ((scroll > 0 && disPlay.rectTransform.sizeDelta.x < oriSize.x * 2.0f) || (scroll < 0 && disPlay.rectTransform.sizeDelta.x > oriSize.x * 0.5f))
                return true;
            return false;
        }
		float lastLen = -1.0f;
		public override void Excute ()
		{
			base.Excute ();
			if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
                if(CanScroll(Input.GetAxis("Mouse ScrollWheel")))
				    disPlay.rectTransform.sizeDelta += disPlay.rectTransform.sizeDelta * Input.GetAxis ("Mouse ScrollWheel");
				contextTrans.sizeDelta = new Vector2 (Mathf.Max(disPlay.rectTransform.sizeDelta.x,1080.0f),Mathf.Max(disPlay.rectTransform.sizeDelta.y,1700.0f));
			}
			if (Input.touchCount > 1) {
				float len = (Input.GetTouch (0).position - Input.GetTouch (1).position).magnitude;
				if (lastLen <= 0.0f)
					lastLen = len;
                if (CanScroll(len-lastLen))
                    disPlay.rectTransform.sizeDelta += disPlay.rectTransform.sizeDelta * (len-lastLen);
				contextTrans.sizeDelta = new Vector2 (Mathf.Max(disPlay.rectTransform.sizeDelta.x,1080.0f),Mathf.Max(disPlay.rectTransform.sizeDelta.y,1700.0f));
			} else {
				lastLen = -1.0f;
			}

			if (_context.album.pics.Count <= 1)
				return;
			if ((Input.touchCount>=1||Input.GetMouseButton(0))&&contextTrans.anchoredPosition.x >= ((contextTrans.sizeDelta.x-1080.0f)*0.5f+200.0f)) {
				left.color = new Color (1.0f,1.0f,1.0f,Mathf.Lerp(left.color.a,1.0f,4.0f*Time.deltaTime));
			} else {
				left.color = new Color (1.0f,1.0f,1.0f,Mathf.Lerp(left.color.a,0.0f,4.0f*Time.deltaTime));
			}
			if ((Input.touchCount>=1||Input.GetMouseButton(0))&&contextTrans.anchoredPosition.x <= -((contextTrans.sizeDelta.x-1080.0f)*0.5f+200.0f)) {
				right.color = new Color (1.0f,1.0f,1.0f,Mathf.Lerp(right.color.a,1.0f,4.0f*Time.deltaTime));
			} else {
				right.color = new Color (1.0f,1.0f,1.0f,Mathf.Lerp(right.color.a,0.0f,4.0f*Time.deltaTime));
			}
			if (Input.GetMouseButtonUp (0) || (Input.touchCount == 1 && Input.touches [0].phase == TouchPhase.Ended)) {
				if (contextTrans.anchoredPosition.x >= ((contextTrans.sizeDelta.x - 1080.0f) * 0.5f + 200.0f)) {
					Debug.Log (_context.index);
					if (_context.index == _context.album.pics.Count - 1)
						_context.index = 0;
					else
						_context.index++;
					disPlay.sprite = _context.album.pics[_context.index].pic;
					disPlay.rectTransform.sizeDelta = Utils.CalSpriteDisplaySize (disPlay.sprite.bounds.size*10.0f,new Vector2(1080.0f,1920.0f));
					contextTrans.sizeDelta = new Vector2 (Mathf.Max(disPlay.rectTransform.sizeDelta.x,1080.0f),Mathf.Max(disPlay.rectTransform.sizeDelta.y,1700.0f));
					contextTrans.anchoredPosition = new Vector2 (-1080.0f,0.0f);
					return;
				}
				if (contextTrans.anchoredPosition.x <= -((contextTrans.sizeDelta.x - 1080.0f) * 0.5f + 200.0f)) {
					
					if (_context.index == 0)
						_context.index = _context.album.pics.Count - 1;
					else
						_context.index--;
					disPlay.sprite = _context.album.pics[_context.index].pic;
					disPlay.rectTransform.sizeDelta = Utils.CalSpriteDisplaySize (disPlay.sprite.bounds.size*10.0f,new Vector2(1080.0f,1920.0f));
					contextTrans.sizeDelta = new Vector2 (Mathf.Max(disPlay.rectTransform.sizeDelta.x,1080.0f),Mathf.Max(disPlay.rectTransform.sizeDelta.y,1700.0f));
					contextTrans.anchoredPosition = new Vector2 (1080.0f,0.0f);
				}
			}
		}
        Vector2 oriSize;
		void Refresh()
		{
			disPlay.sprite = _context.album.pics[_context.index].pic;
			disPlay.rectTransform.sizeDelta = Utils.CalSpriteDisplaySize (disPlay.sprite.bounds.size*10.0f,new Vector2(1080.0f,1920.0f));
            oriSize = disPlay.rectTransform.sizeDelta;
			contextTrans.sizeDelta = new Vector2 (Mathf.Max(disPlay.rectTransform.sizeDelta.x,1080.0f),Mathf.Max(disPlay.rectTransform.sizeDelta.y,1700.0f));
			left.color = new Color (1.0f,1.0f,1.0f,0.0f);
			right.color = new Color (1.0f,1.0f,1.0f,0.0f);
		}
	}
	public class AlbumPicContext : BaseContext
	{
		public int index;
		public AlbumData album;

		public AlbumPicContext() : base(UIType.AlbumPic)
		{
		}
	}
}
