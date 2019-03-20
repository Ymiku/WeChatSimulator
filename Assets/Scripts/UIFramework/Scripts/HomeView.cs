using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
	public class HomeView : EnabledView
	{
		private HomeContext _context;
        public RectTransform contextTrans;
        public CanvasGroup hideCanvas;
        bool showHide = false;
		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as HomeContext;
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
		}
		public override void Excute ()
		{
			base.Excute ();
            if(Input.touchCount==0&&!Input.GetMouseButton(0)&&contextTrans.anchoredPosition.y>=0.0f&& contextTrans.anchoredPosition.y <= 210.0f)
            {
                float y = 0.0f;
                if (contextTrans.anchoredPosition.y >= 100.0f)
                {
                    y = Mathf.Lerp(contextTrans.anchoredPosition.y,210.0f,20.0f*Time.deltaTime);
                    contextTrans.anchoredPosition = new Vector2(contextTrans.anchoredPosition.x, y);
                }
                else
                {
                    y = Mathf.Lerp(contextTrans.anchoredPosition.y, 0.0f, 20.0f * Time.deltaTime);
                    contextTrans.anchoredPosition = new Vector2(contextTrans.anchoredPosition.x, y);
                }
            }
            if(contextTrans.anchoredPosition.y>=20.0f)
            {
                hideCanvas.blocksRaycasts = true;
                showHide = true;
            }
            if (contextTrans.anchoredPosition.y <= 5.0f)
            {
                hideCanvas.blocksRaycasts = false;
                showHide = false;
            }
            if (showHide)
            {
                hideCanvas.alpha = Mathf.Lerp(hideCanvas.alpha,1.0f,8.0f*Time.deltaTime);
            }
            else
            {
                hideCanvas.alpha = Mathf.Lerp(hideCanvas.alpha, 0.0f, 8.0f * Time.deltaTime);
            }
        }
        public void OnClickHome()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new HomeContext());
        }
        public void OnClickFortune()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FortuneContext());
        }
        public void OnClickKoubei()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new KoubeiContext());
        }
        public void OnClickFriends()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new FriendsContext());
        }
        public void OnClickMe()
        {
            UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
            UIManager.Instance.Push(new MeContext());
        }
        public void OnClickScan()
        {
            UIManager.Instance.Push(new ScanContext());
        }
        public void OnClickPay()
        {
            UIManager.Instance.Push(new PayContext());
        }
        public void OnClickCollect()
        {
            UIManager.Instance.Push(new CollectContext());
        }
        public void OnClickPocket()
        {
            UIManager.Instance.Push(new PocketContext());
        }
        public void OnClickTransfer()
        {
            UIManager.Instance.Push(new TransferContext());
        }
        public void OnClickPhoneTopup()
        {
            UIManager.Instance.Push(new PhoneTopupContext());
        }
        public void OnClickMyPackages()
        {
            UIManager.Instance.Push(new MyPackagesContext());
        }
    }
	public class HomeContext : BaseContext
	{
		public HomeContext() : base(UIType.Home)
		{
		}
	}
}
