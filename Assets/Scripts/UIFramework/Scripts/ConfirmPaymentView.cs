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
            XMLSaver.saveData.SetCurPaywayByMoney(_amount);
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

        public void OnClickUseItem() {
            UIManager.Instance.Push(new SelectPayWayContext());
        }

        public void OnClickOk()
        {
            if (_canPayFlag)
            {
                AssetsSaveData data = XMLSaver.saveData.GetAssetsData(_context.accountId);
                if (data == null)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.TransAccountNotExist));
                    return;
                }
                ResultType result = Utils.TryPay(_amount, XMLSaver.saveData.curPayway);
                if (result == ResultType.Success)
                {
                    data.balance += _amount;
                    // todo 打开转账成功界面
                }
                else
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                }
            }
            else
            {
                UIManager.Instance.Push(new SelectPayWayContext());
            }
        }

        private void Refresh()
        {
            _amountText.text = _context.amount.ToString();
            _signObj.transform.localPosition = new Vector3(-_amountText.preferredWidth / 2,
                _signObj.transform.localPosition.y, _signObj.transform.localPosition.z);
            _canPayFlag = XMLSaver.saveData.curPayway != PaywayType.None;
            _useItem.SetActive(_canPayFlag);
            _okTextObj.SetActive(_canPayFlag);
            _canNotPayObj.SetActive(!_canPayFlag);
            _selectTextObj.SetActive(!_canPayFlag);
            switch (XMLSaver.saveData.curPayway)
            {
                case PaywayType.Banlance:
                    _useItemText.text = ContentHelper.Read(ContentHelper.BalanceText);
                    break;
                case PaywayType.YuEBao:
                    _useItemText.text = ContentHelper.Read(ContentHelper.YuEBaoText);
                    break;
                case PaywayType.BankCard:
                    string cardStr = XMLSaver.saveData.curUseBankCard.cardId.Substring(
                        XMLSaver.saveData.curUseBankCard.cardId.Length - 4, 4);
                    _useItemText.text = XMLSaver.saveData.curUseBankCard.bankName + "(" + cardStr + ")";
                    break;
            }
        }
    }
    public class ConfirmPaymentContext : BaseContext
    {
        public ConfirmPaymentContext() : base(UIType.ConfirmPayment)
        {
        }

        public ConfirmPaymentContext(int accountId, double amount) : base(UIType.ConfirmPayment)
        {
            this.accountId = accountId;
            this.amount = amount;
        }

        public int accountId;
        public double amount;
    }

    public class PaymentHelper
    {
        
    }
}
