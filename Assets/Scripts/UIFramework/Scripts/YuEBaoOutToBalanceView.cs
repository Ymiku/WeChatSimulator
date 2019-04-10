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
        public Text _maxMoneyText;

		public override void Init ()
		{
			base.Init ();
            _clearObj.SetActive(false);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as YuEBaoOutToBalanceContext;
            Refresh();
        }

        public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
            Clear();
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            Clear();
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

        public void OnValueChanged(string str)
        {
            bool canUse = !string.IsNullOrEmpty(_moneyInput.text);
            if (canUse)
                canUse = double.Parse(_moneyInput.text) > 0;
            _allObj.SetActive(string.IsNullOrEmpty(_moneyInput.text));
            _clearObj.SetActive(!string.IsNullOrEmpty(_moneyInput.text));
            _confirmBtn.interactable = canUse;
        }

        public void OnClickAll()
        {
            _moneyInput.text = AssetsManager.Instance.assetsData.yuEBao.ToString();
        }

        public void OnClickClear()
        {
            if (!string.IsNullOrEmpty(_moneyInput.text))
                _moneyInput.text = "";
        }

        public void OnClickConfirm()
        {
            AssetsSaveData data = AssetsManager.Instance.assetsData;
            double amount = 0;
            double.TryParse(_moneyInput.text, out amount);
            if (amount > data.yuEBao)
            {
                ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
            }
            else
            {
                data.yuEBao -= amount;
                data.balance += amount;
                UIManager.Instance.Pop();
            }
        }

        public void OnClickToCard()
        {
            UIManager.Instance.Pop();
            UIManager.Instance.Push(new YuEBaoOutToCardContext());
        }

        private void Refresh()
        {
            _moneyInput.text = "";
            _maxMoneyText.text = string.Format(ContentHelper.Read(ContentHelper.MaxCanToBanlance),
                AssetsManager.Instance.assetsData.yuEBao);
        }

        private void Clear()
        {
            _moneyInput.text = "";
            _clearObj.SetActive(false);
            _confirmBtn.interactable = false;
        }
	}
	public class YuEBaoOutToBalanceContext : BaseContext
	{
		public YuEBaoOutToBalanceContext() : base(UIType.YuEBaoOutToBalance)
		{
		}
	}
}
