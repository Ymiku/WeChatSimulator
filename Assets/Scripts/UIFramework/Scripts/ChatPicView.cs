using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class ChatPicView : AlphaView
	{
        public ImageProxy disPlay;
        public RectTransform contextTrans;
        private ChatPicContext _context;

        public override void Init()
        {
            base.Init();
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as ChatPicContext;
            Refresh();
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
            Refresh();
        }
        bool CanScroll(float scroll)
        {
            if ((scroll > 0 && disPlay.rectTransform.sizeDelta.x < oriSize.x * 2.0f) || (scroll < 0 && disPlay.rectTransform.sizeDelta.x > oriSize.x * 0.5f))
                return true;
            return false;
        }
        float lastLen = -1.0f;
        public override void Excute()
        {
            base.Excute();
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (CanScroll(Input.GetAxis("Mouse ScrollWheel")))
                    disPlay.rectTransform.sizeDelta += disPlay.rectTransform.sizeDelta * Input.GetAxis("Mouse ScrollWheel");
                contextTrans.sizeDelta = new Vector2(Mathf.Max(disPlay.rectTransform.sizeDelta.x, 1080.0f), Mathf.Max(disPlay.rectTransform.sizeDelta.y, 1700.0f));
            }
            if (Input.touchCount > 1)
            {
                float len = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                if (lastLen <= 0.0f)
                    lastLen = len;
                if (CanScroll(len - lastLen))
                    disPlay.rectTransform.sizeDelta += disPlay.rectTransform.sizeDelta * (len - lastLen);
                contextTrans.sizeDelta = new Vector2(Mathf.Max(disPlay.rectTransform.sizeDelta.x, 1080.0f), Mathf.Max(disPlay.rectTransform.sizeDelta.y, 1700.0f));
            }
            else
            {
                lastLen = -1.0f;
            }
        }
        Vector2 oriSize;
        void Refresh()
        {
            disPlay.sprite = _context.chatImage;
            disPlay.rectTransform.sizeDelta = Utils.CalSpriteDisplaySize(disPlay.sprite.bounds.size * 10.0f, new Vector2(1080.0f, 1920.0f));
            oriSize = disPlay.rectTransform.sizeDelta;
            contextTrans.sizeDelta = new Vector2(Mathf.Max(disPlay.rectTransform.sizeDelta.x, 1080.0f), Mathf.Max(disPlay.rectTransform.sizeDelta.y, 1700.0f));
        }
    }
	public class ChatPicContext : BaseContext
	{
        public Sprite chatImage;
        public ChatPicContext() : base(UIType.ChatPic)
		{
		}
	}
}
