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
        public void Push(BaseContext nextContext,bool needs2Show = true)
        {
            if (_contextStack.Count != 0)
            {
                BaseContext curContext = _contextStack.Peek();
                BaseView curView = UIManager.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
                curView.OnPause(curContext);
                if (!needs2Show && !curView.activeWhenPause)
                    curView.ForceDisable();
                //Pool(curContext.ViewType);
            }
            _contextStack.Push(nextContext);
            _curContext = nextContext;
            BaseView nextView = UIManager.Instance.GetSingleUI(nextContext.ViewType).GetComponent<BaseView>();
			nextView.transform.SetAsLastSibling ();
            nextView.OnEnter(nextContext);
            Pool(nextContext.ViewType);
        }

        public void Pop()
        {
			
            if (_contextStack.Count != 0)
            {
                BaseContext curContext = _contextStack.Peek();
                _contextStack.Pop();

                BaseView curView = UIManager.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
                curView.OnExit(curContext);
                //Pool(curContext.ViewType);
            }

            if (_contextStack.Count != 0)
            {
                BaseContext lastContext = _contextStack.Peek();
                _curContext = lastContext;
                BaseView curView = UIManager.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
				curView.transform.SetAsLastSibling ();
                curView.OnResume(lastContext);
                Pool(lastContext.ViewType);
            }
        }
        void Pool(UIType type)
        {
            List<UIType> UIPool = UIManager.Instance._UIPool;
            if (UIPool.Contains(type))
            {
                UIPool.Remove(type);
            }
            UIPool.Add(type);
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
