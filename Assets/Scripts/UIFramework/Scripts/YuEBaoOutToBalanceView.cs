using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class YuEBaoOutToBalanceView : EnabledView
	{
		private YuEBaoOutToBalanceContext _context;
        public Button _confirmBtn;
        public FInputField _moneyInput;
        public GameObject _allObj;
        public GameObject _clearObj;

		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutToBalanceContext;
            _moneyInput.text = "";
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
            _moneyInput.text = "";
            _clearObj.SetActive(false);
            _confirmBtn.interactable = false;
        }
		public override void Excute ()
		{
			base.Excute ();
		}

        public void OnValueChanged(string str)
        {
            _allObj.SetActive(string.IsNullOrEmpty(str));
            _clearObj.SetActive(!string.IsNullOrEmpty(str));
            _confirmBtn.interactable = !string.IsNullOrEmpty(str);
            if (!string.IsNullOrEmpty(str) && double.Parse(str) > AssetsManager.Instance.assetsData.balance)
                _moneyInput.text = AssetsManager.Instance.assetsData.balance.ToString();
        }

        public void OnClickAll()
        {
            _moneyInput.text = AssetsManager.Instance.assetsData.balance.ToString();
        }

        public void OnClickClear()
        {
            if (!string.IsNullOrEmpty(_moneyInput.text))
                _moneyInput.text = "";
        }

        public void OnClickConfirm()
        {

        }

        public void OnClickToCard()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new YuEBaoOutToCardContext());
        }
	}
	public class YuEBaoOutToBalanceContext : BaseContext
	{
		public YuEBaoOutToBalanceContext() : base(UIType.YuEBaoOutToBalance)
		{
		}
	}
}
