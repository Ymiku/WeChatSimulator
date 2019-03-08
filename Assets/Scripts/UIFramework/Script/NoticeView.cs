/*
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class NoticeView : AlphaView
	{
		public Text title;
		public Text notice;
		public Button button0;
		public Button button1;
		public Button button2;
		private NoticeContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as NoticeContext;
			title.text = _context.title;
			notice.text = _context.notice;
			if (_context.isOne) {
				button0.gameObject.SetActive (true);
				button1.gameObject.SetActive (false);
				button2.gameObject.SetActive (false);
			} else {
				button0.gameObject.SetActive (false);
				button1.gameObject.SetActive (true);
				button2.gameObject.SetActive (true);
			}
			PlaySound (5);
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

		}
		public void YesCallBack()
		{
			PlaySound (4);
			_context.yesCallBack ();
		}
		public void NoCallBack()
		{
			_context.noCallBack ();
		}
	}
	public class NoticeContext :BaseContext
	{
		public bool isOne = true;
		public string notice;
		public string title;
		public CallBack yesCallBack = null;
		public CallBack noCallBack = null;
		public NoticeContext(string title,string notice) : base(UIType.Notice)
		{
			this.title = title;
			this.notice = notice;
			yesCallBack = () => UIManager.Instance.Pop ();
		}
		public NoticeContext(string title,string notice,CallBack yes,CallBack no) : base(UIType.Notice)
		{
			this.title = title;
			this.notice = notice;
			isOne = false;
			yesCallBack = yes;
			noCallBack = no;
		}
	}

}
*/