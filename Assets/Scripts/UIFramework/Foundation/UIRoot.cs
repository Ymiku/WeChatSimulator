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
			UIManager.Instance.StartUILine (UIManager.UILine.MainMenu);
			UIManager.Instance.Push (new LoadingContext());
        }
		void Update()
		{
			UIManager.Instance.Update ();
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				if (!UIManager.Instance.isQuit) {
					//AudioManager.Instance.PlayUISound (4);
					//UIManager.Instance.Push (new QuitContext ());
				}
			}
		}
	}
}
