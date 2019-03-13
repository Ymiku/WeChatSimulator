using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public class UIRoot : MonoBehaviour {

        public void Start()
        {
            UIManager.Create();
            Localization.Create();
			UIManager.Instance.StartUILine (UIManager.UILine.Main);
			UIManager.Instance.Push (new HomeContext());
        }
		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
                if (!UIManager.Instance.isQuit&&UIManager.Instance.activeContext.Count>1)
                {
                    UIManager.Instance.Pop();
                }
                else if(UIManager.Instance.isQuit)
                {
                    Application.Quit();
                }
				else{
                    //AudioManager.Instance.PlayUISound (4);
                    UIManager.Instance.Push (new QuitContext ());
                }
			}
		}
	}
}
