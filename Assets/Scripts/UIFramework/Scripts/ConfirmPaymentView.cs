using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
    public class ConfirmPaymentView : AnimateView
    {
        private ConfirmPaymentContext _context;
        private GameObject _useItem;
        private GameObject _canNotPayObj;
        private GameObject _okTextObj;
        private GameObject _selectTextObj;
        private GameObject _signObj;
        private Text _amountText;
        private Text _useItemText;
        private Button _okBtn;
        private Button _useItemBtn;
        private double _amount;
        private bool _canPayFlag;
        private string _paywayStr;

        public override void Init()
        {
            base.Init();
            _useItem = FindChild("Content/UseItem");
            _canNotPayObj = FindChild("Content/CantUse");
            _okTextObj = FindChild("Content/OkBtn/Text");
            _selectTextObj = FindChild("Content/OkBtn/Text1");
            _signObj = FindChild("Content/sign");
            _amountText = FindInChild<Text>("Content/Value");
            _useItemText = FindInChild<Text>("Content/UseItem/wayText");
            _okBtn = FindInChild<Button>("Content/OkBtn");
            _useItemBtn = FindInChild<Button>("Content/UseItem");
            _okBtn.onClick.AddListener(OnClickOk);
            _useItemBtn.onClick.AddListener(OnClickUseItem);
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as ConfirmPaymentContext;
            _amount = _context.amount;
            AssetsManager.Instance.SetCurPaywayByMoney(_amount);
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

        public void OnClickUseItem()
        {
            UIManager.Instance.Push(new SelectPayWayContext(_context.amount, SpendType.Transfer));
        }

        public void OnClickOk()
        {
            if (_canPayFlag)
            {
                AccountSaveData accountData = GameManager.Instance.accountData;
                AssetsSaveData data = XMLSaver.saveData.GetAssetsData(_context.accountId);
                if (data == null)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.TransAccountNotExist));
                    return;
                }
                UIManager.Instance.Push(new InputAndCheckPaywordContext(() =>
                {
                    ResultType result = Utils.TryPay(_amount, AssetsManager.Instance.curPayway);
                    if (result == ResultType.Success)
                    {
                        data.balance += _amount;
                        accountData = XMLSaver.saveData.GetAccountData(_context.accountId);
                        UIManager.Instance.Push(new TransferSuccContext(_amount, _paywayStr, accountData, _context.remarksStr));
                    }
                    else
                    {
                        ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                        UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.Transfer));
                    }
                }));
            }
            else
            {
                UIManager.Instance.Push(new SelectPayWayContext(_context.amount, SpendType.Transfer));
            }
        }

        private void Refresh()
        {
            _amountText.text = _context.amount.ToString();
            _signObj.transform.localPosition = new Vector3(-_amountText.preferredWidth / 2,
                _signObj.transform.localPosition.y, _signObj.transform.localPosition.z);
            _canPayFlag = AssetsManager.Instance.curPayway != PaywayType.None;
            _useItem.SetActive(_canPayFlag);
            _okTextObj.SetActive(_canPayFlag);
            _canNotPayObj.SetActive(!_canPayFlag);
            _selectTextObj.SetActive(!_canPayFlag);
            _paywayStr = Utils.FormatPaywayStr(AssetsManager.Instance.curPayway, AssetsManager.Instance.curUseBankCard.cardId);
            _useItemText.text = _paywayStr;
        }
    }
    public class ConfirmPaymentContext : BaseContext
    {
        public ConfirmPaymentContext() : base(UIType.ConfirmPayment)
        {
        }

        public ConfirmPaymentContext(int accountId, double amount, string remarksStr = "") : base(UIType.ConfirmPayment)
        {
            this.accountId = accountId;
            this.amount = amount;
            this.remarksStr = remarksStr;
        }

        public int accountId;
        public double amount;
        public string remarksStr;  // ×ªÕË±¸×¢
    }

    public class PaymentHelper
    {
        
    }
}
