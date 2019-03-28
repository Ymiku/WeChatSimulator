using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
    public class TransferToAccountView : AnimateView
    {
        private TransferToAccountContext _context;
        private Text _accountText;

        private List<string> _allNumberList = StaticDataAccount.GetAllPhoneNumbers();

        public override void Init()
        {
            base.Init();
            _accountText = FindInChild<Text>("");  //todo
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as TransferToAccountContext;
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
        }
        public override void Excute()
        {
            base.Excute();
        }

        public void OnClickNext()
        {
            string account = _accountText.text;
            foreach (var item in _allNumberList)
            {
                if (item == account)
                {
                    if (Utils.CheckIsSelfNumber(account))
                    {
                        ShowNotice(ContentHelper.Read(ContentHelper.CanNotTransToSelf));
                        return;
                    }
                    else
                    {
                        // rtodo 打开下一步界面
                        return;
                    }
                }
            }
            ShowNotice(ContentHelper.Read(ContentHelper.TransAccountNotExist));
        }

        public void OnClickFriend()
        {

        }

        public void OnClickClear()
        {

        }

    }
	public class TransferToAccountContext : BaseContext
	{
		public TransferToAccountContext() : base(UIType.TransferToAccount)
		{
		}
	}
}
