using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
    public class YuEBaoView : EnabledView
    {
        private YuEBaoContext _context;
        private TextProxy _totalText;
        private TextProxy _yesterdayText;
        private TextProxy _accText;
        private TextProxy _wanYuanText;
        private GameObject _closeEyesObj;
        private Button _eyesBtn;
        private bool _eyesState;

        public override void Init()
        {
            base.Init();
            GameObject contentObj = FindChild("ScrollView/Viewport/Content");
            _totalText = Utils.FindInChild<TextProxy>(contentObj, "UpPart/TotalText");
            _yesterdayText = Utils.FindInChild<TextProxy>(contentObj, "UpPart/YesterdayText");
            _accText = Utils.FindInChild<TextProxy>(contentObj, "UpPart/Acc/Value");
            _wanYuanText = Utils.FindInChild<TextProxy>(contentObj, "UpPart/WanYuan/Value");
            _closeEyesObj = Utils.FindChild(contentObj, "UpPart/OpenEyes/CloseEyes");
            _eyesBtn = Utils.FindInChild<Button>(contentObj, "UpPart/OpenEyes");
            _eyesBtn.onClick.AddListener(ClickEyes);
            _eyesState = true;
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as YuEBaoContext;
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

        private void Refresh()
        {
            RefreshTop();
        }

        private void RefreshTop()
        {
            _closeEyesObj.SetActive(!_eyesState);
            string moneyStr = "";
            AssetsSaveData data = AssetsManager.Instance.assetsData;
            if (_eyesState)
            {
                moneyStr = data.yuEBao.ToString();
                _accText.text = data.yuEBaoProfit.ToString();
            }
            else
            {
                moneyStr = Utils.FormatStringForSecrecy("", FInputType.Money);
                _accText.text = Utils.FormatStringForSecrecy("", FInputType.Money);
            }
            _wanYuanText.text = GameDefine.TenThousandProfit.ToString();
            _totalText.text = string.Format(ContentHelper.Read(ContentHelper.TotalAssetsText), moneyStr);
            _yesterdayText.text = data.yuEBaoYesterday > 0 ? data.yuEBaoYesterday.ToString() : ContentHelper.Read(ContentHelper.GuestDontWorry);
            _eyesBtn.transform.localPosition = new Vector3(50 + _totalText.preferredWidth / 2,
                _eyesBtn.transform.localPosition.y, _eyesBtn.transform.localPosition.z);
        }

        public void ClickEyes()
        {
            _eyesState = !_eyesState;
            RefreshTop();
        }

        public void ClickIn()
        {
            UIManager.Instance.Push(new YuEBaoInContext());
        }

        public void ClickTurnOut()
        {
            UIManager.Instance.Push(new YuEBaoOutToBalanceContext());
        }
	}
	public class YuEBaoContext : BaseContext
	{
		public YuEBaoContext() : base(UIType.YuEBao)
		{
		}
	}
}
