using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
	public abstract class BaseView : MonoBehaviour
    {
		protected bool _isPause = false;
		void Awake()
		{
			Init();
		}
		public virtual void Init()
		{

		}
        public virtual void OnEnter(BaseContext context)
        {
			_isPause = false;
        }

        public virtual void OnExit(BaseContext context)
        {
			_isPause = true;
        }

        public virtual void OnPause(BaseContext context)
        {
			_isPause = true;
        }

        public virtual void OnResume(BaseContext context)
        {
			_isPause = false;
        }
		public virtual void Excute()
		{

		}
		public virtual void PopCallBack()
		{
			if (_isPause)
				return;
			PlaySound (9);
			UIManager.Instance.Pop ();
		}
		void Update()
		{
			Excute ();
		}
        public void DestroySelf(bool force = false)
        {
            if(force||!enabled)
                Destroy(gameObject);
        }
		public void PlaySound(int i)
		{
			//AudioManager.Instance.PlayUISound (i);
		}
	}
}
