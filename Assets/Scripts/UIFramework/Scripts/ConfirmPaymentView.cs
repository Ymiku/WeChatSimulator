using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
    public class ConfirmPaymentView : AnimateView
    {
        private ConfirmPaymentContext _context;
        private PaywayType _payway;
        private SpendType _spendType;
        private GameObject _orderItem;
        private GameObject _serviceItem;
        private GameObject _useItem;
        private GameObject _canNotPayObj;
        private GameObject _okTextObj;
        private GameObject _selectTextObj;
        private GameObject _signObj;
        private Text _amountText;
        private Text _useItemText;
        private Text _orderText;
        private Text _serviceText;
        private Text _infoText;
        private Button _okBtn;
        private Button _useItemBtn;
        private double _amount;
        private bool _canPayFlag;
        private string _paywayStr;

        public override void Init()
        {
            base.Init();
            _useItem = FindChild("Content/Group/UseItem");
            _orderItem = FindChild("Content/Group/OrderAmount");
            _serviceItem = FindChild("Content/Group/ServiceCharge");
            _canNotPayObj = FindChild("Content/Group/CantUse");
            _okTextObj = FindChild("Content/OkBtn/Text");
            _selectTextObj = FindChild("Content/OkBtn/Text1");
            _signObj = FindChild("Content/sign");
            _amountText = FindInChild<Text>("Content/Value");
            _useItemText = FindInChild<Text>("Content/Group/UseItem/wayText");
            _orderText = FindInChild<Text>("Content/Group/OrderAmount/value");
            _serviceText = FindInChild<Text>("Content/Group/ServiceCharge/value");
            _infoText = FindInChild<Text>("Content/Group/Info/value");
            _okBtn = FindInChild<Button>("Content/OkBtn");
            _useItemBtn = FindInChild<Button>("Content/Group/UseItem");
            _okBtn.onClick.AddListener(OnClickOk);
            _useItemBtn.onClick.AddListener(OnClickUseItem);
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as ConfirmPaymentContext;
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
            if (_context.spendType == SpendType.TransferToBalance)
                UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.TransferToBalance));
            else if (_context.spendType == SpendType.TransferToBankCard)
                UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.TransferToBankCard));
        }

        public void OnClickOk()
        {
            if (_spendType == SpendType.TransferToBalance)
                PayBalance();
            else if (_spendType == SpendType.TransferToBankCard)
                PayBankCard();
            else if (_spendType == SpendType.ToSelfAssets)
                Recharge();
        }

        private void PayBalance()
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
                        string receiverStr = "(" + Utils.FormatStringForSecrecy(accountData.realName, FInputType.Name) + ")";
                        if (string.IsNullOrEmpty(accountData.nickname) || accountData.nickname == ContentHelper.Read(5))
                            receiverStr = accountData.realName + receiverStr;
                        else
                            receiverStr = accountData.nickname + receiverStr;
                        UIManager.Instance.Push(new TransferSuccContext(_amount, _paywayStr, receiverStr, _context.remarksStr));
                    }
                    else
                    {
                        ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                        UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.TransferToBalance));
                    }
                }));
            }
            else
            {
                UIManager.Instance.Push(new SelectPayWayContext(_context.amount, SpendType.TransferToBalance));
            }
        }

        private void PayBankCard()
        {
            if (_canPayFlag)
            {
                BankCardSaveData data = XMLSaver.saveData.GetBankCardData(_context.cardId);
                if (data == null)
                {
                    ShowNotice(ContentHelper.Read(ContentHelper.CardNotSupport));
                    return;
                }
                UIManager.Instance.Push(new InputAndCheckPaywordContext(() =>
                {
                    ResultType result = Utils.TryPay(_amount, AssetsManager.Instance.curPayway);
                    if (result == ResultType.Success)
                    {
                        data.money += _amount;
                        string receiverStr = "(" + Utils.FormatPaywayStr(PaywayType.BankCard, _context.cardId) + ")";
                        receiverStr = data.realName + receiverStr;
                        UIManager.Instance.Push(new TransferSuccContext(_amount, _paywayStr, receiverStr, _context.remarksStr));
                    }
                    else
                    {
                        ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                        UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.TransferToBalance));
                    }
                }));
            }
            else
            {
                UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.TransferToBankCard));
            }
        }

        private void Recharge()
        {
            if (_canPayFlag)
            {
                UIManager.Instance.Push(new InputAndCheckPaywordContext(() =>
                {
                    ResultType result = Utils.TryPay(_amount, _payway, AssetsManager.Instance.curUseBankCard.cardId);
                    if (result == ResultType.Success)
                    {
                        if(_context.rechargeType == RechargeType.Balance)
                            AssetsManager.Instance.assetsData.balance += _amount;
                        else
                            AssetsManager.Instance.assetsData.yuEBao += _amount;
                        string payStr = Utils.FormatPaywayStr(PaywayType.BankCard, _context.cardId);
                        UIManager.Instance.Push(new RechargeSuccContext(payStr, _amount));
                    }
                    else
                    {
                        ShowNotice(ContentHelper.Read(ContentHelper.AssetsNotEnough));
                        UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.TransferToBalance));
                    }
                }));
            }
            else
            {
                UIManager.Instance.Push(new SelectPayWayContext(_amount, SpendType.ToSelfAssets));
            }
        }

        private void Refresh()
        {
            _spendType = _context.spendType;
            _amount = _spendType == SpendType.TransferToBankCard ? _context.realAmount + _context.serviceAmount : _context.amount;
            if(_spendType == SpendType.ToSelfAssets)
                _payway = AssetsManager.Instance.curUseBankCard != null ? PaywayType.BankCard : PaywayType.None; 
            else
                _payway = AssetsManager.Instance.SetCurPaywayByMoney(_amount);
            _serviceItem.SetActive(_spendType == SpendType.TransferToBankCard);
            _orderItem.SetActive(_spendType == SpendType.TransferToBankCard);
            if (_spendType == SpendType.TransferToBankCard)
            {
                _orderText.text = _context.realAmount.ToString();
                _serviceText.text = _context.serviceAmount.ToString();
            }
            _amountText.text = _amount.ToString();
            _infoText.text = _spendType == SpendType.ToSelfAssets ? ContentHelper.Read(ContentHelper.RechargeText) : ContentHelper.Read(ContentHelper.TransferText);
            _signObj.transform.localPosition = new Vector3(-_amountText.preferredWidth / 2, _signObj.transform.localPosition.y, _signObj.transform.localPosition.z);
            _canPayFlag = _payway != PaywayType.None;
            _useItem.SetActive(_canPayFlag);
            _okTextObj.SetActive(_canPayFlag);
            _canNotPayObj.SetActive(!_canPayFlag);
            _selectTextObj.SetActive(!_canPayFlag);
            _paywayStr = Utils.FormatPaywayStr(AssetsManager.Instance.curPayway, AssetsManager.Instance.curUseCardId);
            _useItemText.text = _paywayStr;
        }
    }
    public class ConfirmPaymentContext : BaseContext
    {
        public ConfirmPaymentContext() : base(UIType.ConfirmPayment)
        {
        }

        /// <summary>
        /// 转到支付宝
        /// </summary>
        public ConfirmPaymentContext(int accountId, double amount, string remarksStr = "") : base(UIType.ConfirmPayment)
        {
            this.accountId = accountId;
            this.amount = amount;
            this.remarksStr = remarksStr;
            spendType = SpendType.TransferToBalance;
        }

        /// <summary>
        /// 转到银行卡
        /// </summary>
        public ConfirmPaymentContext(string cardId, double realAmount, double serviceAmount, string remarksStr = "") : base(UIType.ConfirmPayment)
        {
            this.cardId = cardId;
            this.realAmount = realAmount;
            this.serviceAmount = serviceAmount;
            this.remarksStr = remarksStr;
            spendType = SpendType.TransferToBankCard;
        }

        /// <summary>
        /// 充值使用
        /// </summary>
        public ConfirmPaymentContext(RechargeType rechargeType, double amount) : base(UIType.ConfirmPayment)
        {
            this.rechargeType = rechargeType;
            this.amount = amount;
            spendType = SpendType.ToSelfAssets;
        }

        //转账类型
        public SpendType spendType;

        //通用
        public string remarksStr;

        //转到支付宝账户使用
        public int accountId;
        public double amount;

        //转到银行使用
        public string cardId;
        public double realAmount;
        public double serviceAmount;

        //充值使用
        public RechargeType rechargeType;
    }
}
