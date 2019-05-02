using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CinematicEffects;

namespace UIFrameWork
{
    public class UIManager:Singleton<UIManager>
    {
		public delegate void NoticeFunc(string notice);
		event NoticeFunc noticeFunc;
		public enum UILine
		{
			Main,
			AccountLogin
		}
        public Transform alwaysFrontTrans;
		public UIContext activeContext;
		public Dictionary<UILine,UIContext> _UILineDic = new Dictionary<UILine, UIContext>();

        public Dictionary<UIType, GameObject> _UIDict = new Dictionary<UIType,GameObject>();
        public List<UIType> _UIPool = new List<UIType>();
        int maxPoolSize = 10;
        float poolDeltaTime = 4.0f;

        private Transform _canvas;

		public bool isQuit = false;

		public delegate void UIFunc();
		public bool isBlackTrans = false;

        private UIManager()
        {
            _canvas = GameObject.FindObjectOfType<Canvas>().transform;
        }
		public Transform GetCanvas()
		{
			return _canvas;
		}
		public void Push(BaseContext nextContext)
		{
			activeContext.Push (nextContext);
            if (alwaysFrontTrans != null)
                alwaysFrontTrans.SetAsLastSibling();
		}
		public void Pop()
		{
			activeContext.Pop();
			if (alwaysFrontTrans != null)
				alwaysFrontTrans.SetAsLastSibling();
		}
        public UILine curUILine = UILine.Main;
		public void StartUILine(UILine line)
		{
			if (activeContext != null) {
				activeContext.LineExit ();
			}
            curUILine = line;
			if (!_UILineDic.ContainsKey (line)) {
				_UILineDic.Add (line,new UIContext());
			}
			activeContext = _UILineDic[line];
			activeContext.LineStart ();
		}
		public void StartAndResetUILine(UILine line)
		{
			if (activeContext != null) {
				activeContext.LineExit ();
			}
            curUILine = line;
            _UILineDic.AddOrReplace (line,new UIContext());
			activeContext = _UILineDic[line];
		}


        public GameObject GetSingleUI(UIType uiType)
        {
            if (_UIDict.ContainsKey(uiType) == false || _UIDict[uiType] == null)
            {
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(uiType.Path)) as GameObject;
                go.transform.SetParent(_canvas, false);
                go.name = uiType.Name;
                _UIDict.AddOrReplace(uiType, go);
				go.GetComponent<BaseView> ().Init();
                return go;
            }
            return _UIDict[uiType];
        }
        public GameObject TryGetSingleUI(UIType uiType)
        {
            if (_UIDict.ContainsKey(uiType) == false || _UIDict[uiType] == null)
            {
                return null;
            }
            return _UIDict[uiType];
        }
        public void DestroySingleUI(UIType uiType)
        {
            if (!_UIDict.ContainsKey(uiType))
            {
                return;
            }

            if (_UIDict[uiType] == null)
            {
                _UIDict.Remove(uiType);
                return;
            }

            _UIDict[uiType].GetComponent<BaseView>().DestroySelf();
            _UIDict.Remove(uiType);
        }
        float _poolTimeCount = 0.0f;
        public void Execute()
        {
            _poolTimeCount += Time.deltaTime;
            if(_poolTimeCount>=poolDeltaTime)
                TryPool();
        }
        void TryPool()
        {
            _poolTimeCount = 0.0f;
            if (_UIPool.Count <= maxPoolSize||maxPoolSize == 0)
                return;
			int thisPoolCount = 0;
            for (int i = 0; i < _UIPool.Count; i++)
            {
				thisPoolCount++;
				if (GetSingleUI (_UIPool [i]).GetComponent<BaseView> ().DestroySelf ()) {
					_UIPool.RemoveAt (i);
				}
				if (thisPoolCount >= 2)
					break;
            }
        }
		public void AddNoticeListener(NoticeFunc f)
		{
			noticeFunc += f;
		}
		public void ShowNotice(string notice)
		{
			if (noticeFunc != null)
				noticeFunc (notice);
		}
		public UIType GetLastContextType()
		{
            if (activeContext == null)
                return UIType.None;
			return activeContext.GetLastContextType ();
		}
	}
}
