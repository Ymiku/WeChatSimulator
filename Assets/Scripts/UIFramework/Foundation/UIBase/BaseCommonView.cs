using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    public class BaseCommonView : BaseView
    {
        public BaseView linkedView;
        public void Show()
        {

        }
        public void Hide()
        {

        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            gameObject.SetActive(true);
        }
        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            gameObject.SetActive(false);
        }
        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }
        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }
    }
}