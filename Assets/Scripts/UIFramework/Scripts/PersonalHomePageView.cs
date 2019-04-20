using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class PersonalHomePageView : AnimateView
	{
		private PersonalHomePageContext _context;
        private ImageProxy _bigHead;
        private ImageProxy _head;
        private TextProxy _nickName;
        private TextProxy _signatureText;
        private TextProxy _accountText;
        private TextProxy _realNameText;
        private GameObject _systemHeadObj;
        private Button _uploadBtn;
        private Button _changeNickBtn;

		public override void Init ()
		{
			base.Init ();
            _bigHead = FindInChild<ImageProxy>("ScrollView/Viewport/Content/bigHead");
            _head = FindInChild<ImageProxy>("ScrollView/Viewport/Content/head/head");
            _nickName = FindInChild<TextProxy>("ScrollView/Viewport/Content/nickName");
            _signatureText = FindInChild<TextProxy>("ScrollView/Viewport/Content/signature");
            _accountText = FindInChild<TextProxy>("ScrollView/Viewport/Content/account/value");
            _realNameText = FindInChild<TextProxy>("ScrollView/Viewport/Content/realName/value");
            _systemHeadObj = FindChild("ScrollView/Viewport/Content/head/system_head");
            _uploadBtn = FindInChild<Button>("ScrollView/Viewport/Content/head");
            _changeNickBtn = FindInChild<Button>("ScrollView/Viewport/Content/changeNameBtn");
            _uploadBtn.onClick.AddListener(OnClickUpload);
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
		}

        private void OnClickUpload()
        {
            if (_context.accountId == GameManager.Instance.curUserId)
                UIManager.Instance.Push(new SetHeadContext());
        }

        private void OnClickChangeName()
        {

        }

        private void Refresh()
        {
            HeadSpriteUtils.Instance.SetHead(_bigHead);
            HeadSpriteUtils.Instance.SetHead(_head);
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
