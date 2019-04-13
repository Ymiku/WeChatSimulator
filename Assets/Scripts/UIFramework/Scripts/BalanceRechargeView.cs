using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class BalanceRechargeView : AnimateView
	{
		private BalanceRechargeContext _context;
        public FInputField _moneyInput;
        public Button _nextBtn;
        public GameObject _balanceSelectObj;
        public GameObject _yuEBaoSelectObj;
        private RechargeType _rechargeTo;

		public override void Init ()
		{
			base.Init ();
            _rechargeTo = RechargeType.Balance;
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as BalanceRechargeContext;
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
		public override void Excute ()
		{
			base.Excute ();
		}

        private void Refresh()
        {
            _moneyInput.text = "";
            _balanceSelectObj.SetActive(_rechargeTo == RechargeType.Balance);
            _yuEBaoSelectObj.SetActive(_rechargeTo == RechargeType.YuEBao);
        }

        public void OnClickNext()
        {
            if (!string.IsNullOrEmpty(_moneyInput.text))
            {
                double money = double.Parse(_moneyInput.text);
                UIManager.Instance.Push(new ConfirmPaymentContext(_rechargeTo, money));
            }
        }

        public void OnClickBalance()
        {
            if(_rechargeTo == RechargeType.YuEBao)
            {
                _rechargeTo = RechargeType.Balance;
                _balanceSelectObj.SetActive(true);
                _yuEBaoSelectObj.SetActive(false);
            }
        }

        public void OnClickYuEBao()
        {
            if (_rechargeTo == RechargeType.Balance)
            {
                _rechargeTo = RechargeType.YuEBao;
                _balanceSelectObj.SetActive(false);
                _yuEBaoSelectObj.SetActive(true);
            }
        }

        public void OnValueChanged(string str)
        {
            if (!string.IsNullOrEmpty(_moneyInput.text))
                _nextBtn.interactable = true;
            else
                _nextBtn.interactable = false;
        }
	}
	public class BalanceRechargeContext : BaseContext
	{
		public BalanceRechargeContext() : base(UIType.BalanceRecharge)
		{
		}
	}
}
