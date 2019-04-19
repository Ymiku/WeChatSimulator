using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class AlbumPicView : AlphaView
	{
		public ImageProxy disPlay;
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
		public override void Excute ()
		{
			base.Excute ();
		}
		void Refresh()
		{
			disPlay.sprite = _context.pic.pic;
			disPlay.rectTransform.sizeDelta = Utils.CalSpriteDisplaySize (disPlay.sprite,new Vector2(1080.0f,1920.0f));
		}
	}
	public class AlbumPicContext : BaseContext
	{
		public AlbumPic pic;

		public AlbumPicContext() : base(UIType.AlbumPic)
		{
		}
	}
}
