using static_data;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDataBlog
{
	public static BLOG_ARRAY Info;

	public static void Init()
	{
		Info = StaticDataLoader.ReadOneDataConfig<BLOG_ARRAY>("blog");
		TransToBlogData ();
	}

	public static void TransToBlogData()
	{
		for (int i = 0; i < Info.items.Count; i++)
		{
			BLOG a = Info.items [i];
			BlogData blog = new BlogData ();
			blog.id = a.blog_id;
			blog.userId = a.user_id;
			blog.path = a.blog_path;
			blog.order = a.order;
			List<BlogData> blogs;
			if (ZoneManager.Instance.id2Blog.TryGetValue (a.user_id,out blogs)) {

			} else {
				blogs = new List<BlogData> ();
				ZoneManager.Instance.id2Blog.Add (a.user_id,blogs);
			}
			blogs.Add(blog);
		}
	}
}
