using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
namespace UIFrameWork
{
	public class EnterAlbumView : AlphaView
	{
		public EnterAlbumItem prefab;
		List<EnterAlbumItem> items = new List<EnterAlbumItem>();
		private EnterAlbumContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as EnterAlbumContext;
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
			int count = Mathf.Max(_context.album.pics.Count, items.Count);
			for (int i = 0; i < count; i++)
			{
				if (_context.album.pics.Count <= i)
				{
					items[i].gameObject.SetActive(false);
					continue;
				}
				if (items.Count <= i)
				{
					items.Add(GameObject.Instantiate<EnterAlbumItem>(prefab));
					items[i].cachedRectTransform.SetParent(prefab.transform.parent);
					items[i].cachedRectTransform.localPosition = Vector2.zero;
					items[i].cachedRectTransform.localScale = Vector3.one;
					items[i].id = i;
				}
				items[i].SetData(_context.album.pics[i]);
				items[i].gameObject.SetActive(true);
			}
		}
	}
	public class EnterAlbumContext : BaseContext
	{
		public AlbumData album;
		public EnterAlbumContext() : base(UIType.EnterAlbum)
		{
		}
	}
}
