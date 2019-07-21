using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UIFrameWork
{
    public class UIContext
    {
        private Stack<BaseContext> _contextStack = new Stack<BaseContext>();
        BaseContext _curContext;
		public void LineStart()
		{
			if (_contextStack.Count == 0)
				return;
			BaseView temp;
			foreach (BaseContext item in _contextStack) {
				temp = UIManager.Instance.GetSingleUI (item.ViewType).GetComponent<BaseView> ();
				temp.OnEnter (item);
                temp.OnPause(item);
                temp.ForceDisable();
			}

			BaseContext lastContext = _contextStack.Peek();
			BaseView curView = UIManager.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
			curView.OnResume(lastContext);
		}
		public void LineExit()
		{
            BaseView view = null;
			foreach (BaseContext item in _contextStack) {
                view = UIManager.Instance.GetSingleUI(item.ViewType).GetComponent<BaseView>();
                view.OnExit(item);
                if (item != _curContext)
                    view.ForceDisable();
            }
		}
        public void Push(BaseContext nextContext)
        {
            UIType curType = nextContext.ViewType;
            UIType nextType = UIType.None;
            
            if (_contextStack.Count != 0)
            {
                BaseContext curContext = _contextStack.Peek();
                nextType = curContext.ViewType;
                BaseView curView = UIManager.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
                curView.OnPause(curContext);
            }
            BaseView nextView = UIManager.Instance.GetSingleUI(nextContext.ViewType).GetComponent<BaseView>();
            if (nextView.closeOtherUI)
                UIManager.Instance.CloseAllUI();
            _contextStack.Push(nextContext);
            _curContext = nextContext;
			nextView.transform.SetAsLastSibling ();
            nextView.OnEnter(nextContext);
			DisableInstant ();
            if(!UIManager.Instance.activeView.Contains(nextView))
                UIManager.Instance.activeView.Add(nextView);
            UIManager.Instance.commonUIManager.Refresh(curType,nextType);
        }

        public void Pop()
        {
            UIType curType = UIType.None;
            UIType nextType = UIType.None;
            if (_contextStack.Count != 0)
            {
                BaseContext curContext = _contextStack.Peek();
                _contextStack.Pop();
                BaseView curView = UIManager.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
                curView.OnExit(curContext);
            }

            if (_contextStack.Count != 0)
            {
                BaseContext lastContext = _contextStack.Peek();
                curType = lastContext.ViewType;
                _curContext = lastContext;
                BaseView curView = UIManager.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
				curView.transform.SetAsLastSibling ();
                if(!curView.hasEnter)
                {
                    curView.OnEnter(lastContext);
                    curView.OnPause(lastContext);
                }
                curView.OnResume(lastContext);
                if (curView.closeOtherUI)
                    UIManager.Instance.CloseAllUI();
                if (!UIManager.Instance.activeView.Contains(curView))
                    UIManager.Instance.activeView.Add(curView);
            }
			DisableInstant ();
            UIManager.Instance.commonUIManager.Refresh(curType,nextType);
        }
        //同一时间最多显示两个UI
		void DisableInstant()
		{
			List<UIType> UIPool = UIManager.Instance._UIPool;
			if (UIPool.Count <= 2)
				return;
			if (UIPool [UIPool.Count - 3] != UIPool [UIPool.Count - 2] && UIPool [UIPool.Count - 3] != UIPool [UIPool.Count - 1]) {
                GameObject endObj = UIManager.Instance.TryGetSingleUI(UIPool[UIPool.Count - 3]);
                if (endObj == null)
                    return;
                BaseView endView = UIManager.Instance.TryGetSingleUI(UIPool[UIPool.Count-3]).GetComponent<BaseView>();
                if(endView!=null)
				    endView.ForceDisable ();
			}
		}
		public UIType GetLastContextType()
		{
            
			if (_contextStack.Count <= 1)
				return UIType.None;
            BaseContext curContext = _contextStack.Pop();
            UIType lastType = _contextStack.Peek().ViewType;
            _contextStack.Push(curContext);
            return lastType;
		}
        public BaseContext PeekOrNull()
        {
            if (_contextStack.Count != 0)
            {
                return _contextStack.Peek();
            }
            return null;
        }
        public int Count
        {
            get { return _contextStack.Count; }
        }
    }
}
