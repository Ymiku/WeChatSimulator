using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrameWork
{
	public class TotalAssetsView : AnimateView
	{
		private TotalAssetsContext _context;
        private Text _totalText;
        private Text _balanceText;
        private Text _yuEBaoText;
        private Text _nameText;
        private Text _yesterdayText1;
        private Text _yesterdayText2;
        private Image _headImage;

		public override void Init ()
		{
			base.Init ();
            _totalText = FindInChild<Text>("Panel/Middle/TotalText/Value");
            _balanceText = FindInChild<Text>("Panel/Middle/BalanceText/Value");
            _yuEBaoText = FindInChild<Text>("Panel/Middle/YuEBaoText/Value");
            _nameText = FindInChild<Text>("Panel/Top/NameText");
            _yesterdayText1 = FindInChild<Text>("Panel/Middle/TotalText/Yesterday/Value");
            _yesterdayText2 = FindInChild<Text>("Panel/Middle/YuEBaoText/AddValue");
            _headImage = FindInChild<Image>("Panel/Top/HeadImage/img");
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TotalAssetsContext;
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
		}
		public override void Excute ()
		{
			base.Excute ();
		}

        private void Refresh()
        {
            _nameText.text = GameManager.Instance.accountData.realName;
            _balanceText.text = AssetsManager.Instance.assetsData.balance.ToString("0.00");
            _yuEBaoText.text = AssetsManager.Instance.assetsData.yuEBao.ToString("0.00");
            _totalText.text = (AssetsManager.Instance.assetsData.balance + AssetsManager.Instance.assetsData.yuEBao).ToString("0.00");
            _yesterdayText1.text = "+" + AssetsManager.Instance.assetsData.yuEBaoYesterday.ToString("0.00");
            _yesterdayText2.text = "+" + AssetsManager.Instance.assetsData.yuEBaoYesterday.ToString("0.00");
            HeadSpriteUtils.Instance.SetHead(_headImage);
        }

        public void OnClickBalance()
        {
            UIManager.Instance.Push(new BalanceContext());
        }

        public void OnClickYuEBao()
        {
            UIManager.Instance.Push(new YuEBaoContext());
        }

        public void OnClickFixedTerm()
        {
            UIManager.Instance.Push(new FixedTermContext());
        }

        public void OnClickJiJin()
        {

        }

        public void OnClickGold()
        {

        }

        public void OnClickFortune()
        {
            UIManager.Instance.Push(new FortuneContext());
        }
	}
	public class TotalAssetsContext : BaseContext
	{
		public TotalAssetsContext() : base(UIType.TotalAssets)
		{
		}
	}
}
