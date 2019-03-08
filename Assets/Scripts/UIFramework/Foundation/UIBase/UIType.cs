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
	
		public static readonly UIType LogIn = new UIType("View/LogInView");
		public static readonly UIType Register = new UIType("View/RegisterView");
		public static readonly UIType Notice = new UIType("View/NoticeView");
		public static readonly UIType UserCreat = new UIType("View/UserCreatView");
		public static readonly UIType Loading = new UIType("View/LoadingView");
		public static readonly UIType GamePlay = new UIType("View/GamePlayView");

		public static readonly UIType OptionMenu = new UIType("View/OptionMenuView");
		public static readonly UIType LevelComplete = new UIType("View/LevelCompleteView");
		public static readonly UIType LevelOption = new UIType("View/LevelOptionView");
		public static readonly UIType Dialogue = new UIType("View/DialogueView");
		public static readonly UIType Main = new UIType("View/MainView");
		public static readonly UIType Quit = new UIType("View/QuitView");
		public static readonly UIType Achieve = new UIType("View/AchieveView");
		public static readonly UIType NetRoom = new UIType("View/NetRoomView");
    }
}
