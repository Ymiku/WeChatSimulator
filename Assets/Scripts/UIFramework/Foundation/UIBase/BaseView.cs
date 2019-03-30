using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
    public abstract class BaseView : MonoBehaviour
    {
		public bool activeWhenPause = false;
        protected bool _isPause = false;
        public void ShowNotice(string notice)
        {
            UIManager.Instance.ShowNotice(notice);
        }
        protected CanvasGroup _canvasGroup;
        void Awake()
        {
            Init();
        }
        public virtual void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        public virtual void OnEnter(BaseContext context)
        {
            _isPause = false;
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void OnExit(BaseContext context)
        {
            _isPause = true;
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnPause(BaseContext context)
        {
            _isPause = true;
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnResume(BaseContext context)
        {
            _isPause = false;
            _canvasGroup.blocksRaycasts = true;
        }
        public virtual void Excute()
        {

        }
        public virtual void PopCallBack()
        {
            if (_isPause)
                return;
            PlaySound(9);
            UIManager.Instance.Pop();
        }
        void Update()
        {
            Excute();
        }
        public bool DestroySelf(bool force = false)
        {
            if (force || !gameObject.activeSelf)
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }
        public void PlaySound(int i)
        {
            //AudioManager.Instance.PlayUISound (i);
        }
        public GameObject FindChild(string path)
        {
            return Utils.FindChild(gameObject, path);
        }
        public T FindInChild<T>(string path) where T : Component
        {
            return Utils.FindInChild<T>(gameObject, path);
        }
    }
}
