using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
    public abstract class BaseView : MonoBehaviour
    {
        public bool IsCommonView
        {
            get
            {
                return this is BaseCommonView;
            }
        }
        public bool closeOtherUI = true;
        [SerializeField]
        protected bool _isActive = false;//逻辑层，是否为顶层UI
        [SerializeField]
        protected bool _isViewHide = true;//视图层是否隐藏

        public void ShowNotice(string notice)
        {
            UIManager.Instance.ShowNotice(notice);
        }
        protected CanvasGroup _canvasGroup;
        [HideInInspector]
        public bool hasEnter = false;

        public List<UIType> commonUI2Show = new List<UIType>();
        protected void RegistCommonView(UIType uiType)
        {
            commonUI2Show.Add(uiType);
        }
        public virtual void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            RegistCommonView(UIType.CommonMobileStatusBar);
        }
        public virtual void OnEnter(BaseContext context)
        {
            _isActive = true;
            _canvasGroup.blocksRaycasts = true;
            hasEnter = true;
            ShowUI();
        }

        public virtual void OnExit(BaseContext context)
        {
            _isActive = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnPause(BaseContext context)
        {
            _isActive = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnResume(BaseContext context)
        {
            _isActive = true;
            _canvasGroup.blocksRaycasts = true;
            ShowUI();
        }
        public virtual bool ShowUI()
        {
            if (!_isViewHide)
                return false;
            _isViewHide = false;
            return true;
        }
        public virtual bool HideUI()
        {
            if (_isViewHide)
                return false;
            _isViewHide = true;
            return true;
        }
        public virtual void Excute()
        {

        }
        public virtual void PopCallBack()
        {
            if (!_isActive)
                return;
            if (UIManager.Instance.activeContext.Count > 1)
            {
                UIManager.Instance.Pop();
            }
            else if (UIManager.Instance.curUILine == UIManager.UILine.AccountLogin)
            {
                UIManager.Instance.StartUILine(UIManager.UILine.Main);
            };
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
        //立即结束动画
        public virtual void ForceDisable()
        {
            if (UIManager.Instance.activeView.Contains(this))
                UIManager.Instance.activeView.Remove(this);
            _isViewHide = true;
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
