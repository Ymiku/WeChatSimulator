using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoInView : AnimateView
	{
		private YuEBaoInContext _context;
        public TextProxy _bankName;
        public TextProxy _bankLastNum;
        public TextProxy _tips;
        public TextProxy _timeText;
        public Image _icon;
        public Button _confirmBtn;
        public FInputField _moneyInput;

        public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoInContext;
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
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnClickPayway()
        {

        }

        public void OnClickExplain()
        {

        }

        public void OnValueChanged(string str)
        {

        }

        private void Refresh()
        {

        }
	}
	public class YuEBaoInContext : BaseContext
	{
		public YuEBaoInContext() : base(UIType.YuEBaoIn)
		{
		}
	}
}
