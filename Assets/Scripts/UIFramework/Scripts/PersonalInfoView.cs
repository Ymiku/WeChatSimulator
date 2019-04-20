using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
    public class PersonalInfoView : AnimateView
    {
        private PersonalInfoContext _context;
        private Button _personalMainBtn;
        private ImageProxy _headImg;
        private TextProxy _realName;
        private TextProxy _accountText;

        public override void Init()
        {
            base.Init();
            _personalMainBtn = FindInChild<Button>("Content/Item1");
            _headImg = FindInChild<ImageProxy>("Content/Item1/head");
            _realName = FindInChild<TextProxy>("Content/Item2/name");
            _accountText = FindInChild<TextProxy>("Content/Item3/num");
            _personalMainBtn.onClick.AddListener(OnClickPersonalMain);

        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as PersonalInfoContext;
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

        private void OnClickPersonalMain()
        {
            UIManager.Instance.Push(new PersonalHomePageContext(GameManager.Instance.curUserId));
        }

        private void Refresh()
        {
            HeadSpriteUtils.Instance.SetHead(_headImg);
            _accountText.text = GameManager.Instance.accountData.phoneNumber;
            _realName.text = GameManager.Instance.accountData.realName;
        }
    }
	public class PersonalInfoContext : BaseContext
	{
		public PersonalInfoContext() : base(UIType.PersonalInfo)
		{
		}
	}
}
