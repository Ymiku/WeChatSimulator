using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIFrameWork
{
    public class CommonUIManager
    {
        Dictionary<UIType, BaseContext> type2Context = new Dictionary<UIType, BaseContext>()
        {
            { UIType.CommonHomeBar,new CommonHomeBarContext() }, //主界面工具条
            { UIType.CommonMobileStatusBar,new CommonMobileStatusBarContext() }, //状态栏
        };
        public void Refresh(UIType curType,UIType nextType)
        {
            BaseView curView = UIManager.Instance.TryGetSingleUI(curType).GetComponent<BaseView>();
            BaseView nextView = null;
            GameObject next = UIManager.Instance.TryGetSingleUI(nextType);
            if(next!=null)
                nextView = next.GetComponent<BaseView>();
            foreach (var k in type2Context.Keys)
            {
                if(!curView.commonUI2Show.Contains(k)&&(nextView==null||!nextView.commonUI2Show.Contains(k)))
                {
                    GameObject go = UIManager.Instance.TryGetSingleUI(k);
                    if (go != null) {
                        go.GetComponent<BaseCommonView>().OnExit(type2Context[k]);
                        go.GetComponent<BaseCommonView>().linkedView = null;
                    }
                }
            }
            
            if(nextView!=null)
            {
                for (int i = 0; i < nextView.commonUI2Show.Count; i++)
                {
                    UIType commonType = nextView.commonUI2Show[i];
                    if (curView.commonUI2Show.Contains(commonType))
                        continue;
                    AddCommonUI(commonType,nextView);
                }
            }
            for (int i = 0; i < curView.commonUI2Show.Count; i++)
            {
                AddCommonUI(curView.commonUI2Show[i],curView);
            }
        }
        void AddCommonUI(UIType commonType,BaseView parent)
        {
            BaseCommonView commonView = UIManager.Instance.GetSingleUI(commonType).GetComponent<BaseCommonView>();
            commonView.transform.SetSiblingIndex(parent.transform.GetSiblingIndex()+1);
            commonView.linkedView = parent;
            commonView.OnEnter(type2Context[commonType]);
        }
    }
}
