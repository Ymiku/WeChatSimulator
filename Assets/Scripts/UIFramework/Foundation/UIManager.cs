using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CinematicEffects;

namespace UIFrameWork
{
    public class UIManager:Singleton<UIManager>
    {
		public enum UILine
		{
			MainMenu,
			LevelMenu
		}
		public UIContext activeContext;
		public Dictionary<UILine,UIContext> _UILineDic = new Dictionary<UILine, UIContext>();

        public Dictionary<UIType, GameObject> _UIDict = new Dictionary<UIType,GameObject>();
        public List<UIType> _UIPool = new List<UIType>();
        int maxPoolSize = 8;
        float poolDeltaTime = 4.0f;

        private Transform _canvas;

		public bool isQuit = false;

		public delegate void UIFunc();
		public bool isBlackTrans = false;

        private UIManager()
        {
            _canvas = GameObject.Find("Canvas").transform;
        }
		public Transform GetCanvas()
		{
			return _canvas;
		}

		public void Push(BaseContext nextContext)
		{
			activeContext.Push (nextContext);
		}
		public void Pop()
		{
			activeContext.Pop();
		}
		public void StartUILine(UILine line)
		{
			if (activeContext != null) {
				activeContext.LineExit ();
			}
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
        void Execute()
        {
            TryPool();
        }
        void TryPool()
        {
            if (_UIPool.Count <= maxPoolSize||maxPoolSize == 0)
                return;
            for (int i = 0; i < _UIPool.Count; i++)
            {
                if (GetSingleUI(_UIPool[i]).GetComponent<BaseView>().DestroySelf())
                    _UIPool.RemoveAt(i);
                i--;
                if (_UIPool.Count <= maxPoolSize)
                    break;
            }
        }
	}
}
