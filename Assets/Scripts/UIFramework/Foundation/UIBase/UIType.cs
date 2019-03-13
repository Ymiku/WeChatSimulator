using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public class UIType {

        public string Path { get; private set; }

        public string Name { get; private set; }

        public UIType(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }
	
		//public static readonly UIType LogIn = new UIType("View/LogInView");
		public static readonly UIType Home = new UIType("View/HomeView");
		public static readonly UIType Fortune = new UIType("View/FortuneView");
		public static readonly UIType Koubei = new UIType("View/KoubeiView");
		public static readonly UIType Friends = new UIType("View/FriendsView");
		public static readonly UIType Me = new UIType("View/MeView");
    }
}
