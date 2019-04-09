using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoOutToCardView : AnimateView
	{
		private YuEBaoOutToCardContext _context;
        public TextProxy _cardText;
        public TextProxy _nomalText;
        public ImageProxy _cardIcon;
        public Button _confirmBtn;
        public GameObject _allObj;
        public GameObject _clearObj;
        public GameObject _fastWaySelectedObj;
        public GameObject _normalWaySelectedobj;
        public FInputField _moneyInput;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutToCardContext;
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

        public void OnClickToBanlance()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new YuEBaoOutToBalanceContext());
        }

        public void OnClickAll()
        {

        }

        public void OnClickFast()
        {

        }

        public void OnClickNormal()
        {

        }

        public void OnClickConfirm()
        {

        }

        public void OnClickClear()
        {

        }

        public void OnClickCard()
        {

        }

        public void OnValueChanged(string str)
        {

        }
	}
	public class YuEBaoOutToCardContext : BaseContext
	{
		public YuEBaoOutToCardContext() : base(UIType.YuEBaoOutToCard)
		{
		}
	}
}
