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
			#if UNITY_EDITOR
			Init();
			#endif
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
			if (!GameManager.Instance.CanOperate ())
				return;
			if (_isPause)
				return;
			PlaySound (9);
			UIManager.Instance.Pop ();
		}
		public virtual void PopAndTransCallBack()
		{
			if (!GameManager.Instance.CanOperate ())
				return;
			if (_isPause)
				return;
			PlaySound (9);
			UIManager.Instance.StartBlackTrans ();
			UIManager.Instance.Pop ();
		}
		void Update()
		{
			Excute ();
		}
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
		public void PlaySound(int i)
		{
			AudioManager.Instance.PlayUISound (i);
		}
	}
}
