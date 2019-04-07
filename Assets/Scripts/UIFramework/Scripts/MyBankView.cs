using UnityEngine;
using System.Collections;
namespace UIFrameWork
{
    public class MyBankView : AnimateView
    {
        private MyBankContext _context;
        private GameObject _addCardRoot;
        private GameObject _cardScrollRoot;

        public override void Init()
        {
            base.Init();
            _addCardRoot = FindChild("AddCardRoot");
            _cardScrollRoot = FindChild("CardScrollView");

        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as MyBankContext;
            Refresh();
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
            Refresh();
        }
        public override void Excute()
        {
            base.Excute();
        }

        public void OnClickServices()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new BankServicesContext());
        }

        public void OnClickAddBank()
        {
            UIManager.Instance.Push(new AddBankCardContext());
        }

        private void Refresh()
        {
            bool noCardFlag = AssetsManager.Instance.bankCardsData.Count == 0;
            _addCardRoot.SetActive(noCardFlag);
            _cardScrollRoot.SetActive(!noCardFlag);
            if (!noCardFlag)
            {
                
            }
        }
    }
    public class MyBankContext : BaseContext
    {
        public MyBankContext() : base(UIType.MyBank)
        {
        }
    }
}
