using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class PersonalHomePageView : AnimateView
	{
		private PersonalHomePageContext _context;
        private ImageProxy _backImg;
        private ImageProxy _head;
        private TextProxy _nickName;
        private TextProxy _signatureText;
        private TextProxy _accountText;
        private TextProxy _realNameText;
        private GameObject _systemHeadObj;
        private Button _uploadHeadBtn;
        private Button _uploadBackBtn;
        private Button _changeNickBtn;

		public override void Init ()
		{
			base.Init ();
            _backImg = FindInChild<ImageProxy>("ScrollView/Viewport/Content/bigHead");
            _head = FindInChild<ImageProxy>("ScrollView/Viewport/Content/head/head");
            _nickName = FindInChild<TextProxy>("ScrollView/Viewport/Content/nickName");
            _signatureText = FindInChild<TextProxy>("ScrollView/Viewport/Content/signature");
            _accountText = FindInChild<TextProxy>("ScrollView/Viewport/Content/account/value");
            _realNameText = FindInChild<TextProxy>("ScrollView/Viewport/Content/realName/value");
            _systemHeadObj = FindChild("ScrollView/Viewport/Content/head/system_head");
            _uploadHeadBtn = FindInChild<Button>("ScrollView/Viewport/Content/head");
            _uploadBackBtn = FindInChild<Button>("ScrollView/Viewport/Content/bigHead");
            _changeNickBtn = FindInChild<Button>("ScrollView/Viewport/Content/changeNameBtn");
            _uploadHeadBtn.onClick.AddListener(OnClickUploadHead);
            _uploadBackBtn.onClick.AddListener(OnClickUploadBack);
            _changeNickBtn.onClick.AddListener(OnClickChangeName);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as PersonalHomePageContext;
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
            if (_lastBack != _backImg.sprite)
            {
                _backImg.rectTransform.sizeDelta = Utils.CalSpriteFillSize(_backImg.sprite.bounds.size,new Vector2(1080.0f,757.37f));
                _lastBack = _backImg.sprite;
            }
		}

        private void OnClickUploadHead()
        {
            if (_context.accountId == GameManager.Instance.curUserId)
                UIManager.Instance.Push(new SetHeadContext(true));
        }

        private void OnClickUploadBack()
        {
            if (_context.accountId == GameManager.Instance.curUserId)
                UIManager.Instance.Push(new SetHeadContext(false));
        }

        private void OnClickChangeName()
        {

        }
        Sprite _lastBack = null;
        private void Refresh()
        {
            _lastBack = null;
            HeadSpriteUtils.Instance.SetBack(_backImg,_context.accountId);
            HeadSpriteUtils.Instance.SetHead(_head,_context.accountId);
            AccountSaveData data = XMLSaver.saveData.GetAccountData(_context.accountId);
            if (string.IsNullOrEmpty(data.nickname))
                _nickName.text = ContentHelper.Read(ContentHelper.NotSetNickName);
            else
                _nickName.text = data.nickname;
            _accountText.text = data.phoneNumber;
            _realNameText.text = data.realName;
            bool systemHeadFlag = string.IsNullOrEmpty(data.headSprite);
            if (!systemHeadFlag)
                systemHeadFlag = data.headSprite.IndexOf("Sprites/") == 0;
            _systemHeadObj.SetActive(systemHeadFlag);
            _uploadBackBtn.interactable = _context.accountId == GameManager.Instance.curUserId;
            _uploadHeadBtn.interactable = _context.accountId == GameManager.Instance.curUserId;
        }
	}
	public class PersonalHomePageContext : BaseContext
	{
		public PersonalHomePageContext(int accountId) : base(UIType.PersonalHomePage)
		{
            this.accountId = accountId;
		}
        public int accountId;
	}
}
