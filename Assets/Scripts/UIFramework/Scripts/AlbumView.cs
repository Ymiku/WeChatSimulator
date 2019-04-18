using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
namespace UIFrameWork
{
	public class AlbumView : AlphaView
	{
        public AlbumItem prefab;
        List<AlbumItem> items = new List<AlbumItem>();
		private AlbumContext _context;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as AlbumContext;
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
        void Refresh()
        {
            List<AlbumData> albums;
            ZoneManager.Instance.id2Album.TryGetValue(_context.userId,out albums);
            int count = Mathf.Max(albums.Count, items.Count);
            for (int i = 0; i < count; i++)
            {
                if (albums.Count <= i)
                {
                    items[i].gameObject.SetActive(false);
                    continue;
                }
                if (items.Count <= i)
                {
                    items.Add(GameObject.Instantiate<AlbumItem>(prefab));
                    items[i].cachedRectTransform.SetParent(prefab.transform.parent);
                    items[i].cachedRectTransform.localPosition = Vector2.zero;
                    items[i].cachedRectTransform.localScale = Vector3.one;
                    items[i].id = i;
                }
                items[i].SetData(albums[i]);
                items[i].gameObject.SetActive(true);
            }
        }
	}
	public class AlbumContext : BaseContext
	{
        public int userId = 0;
        public AlbumContext() : base(UIType.Album)
		{
            
		}
	}
}
