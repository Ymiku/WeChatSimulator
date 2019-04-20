using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork
{
    public class SetHeadView : AnimateView
    {
        private SetHeadContext _context;
        private Button _uploadBtn;
        private ImageProxy _head;
        private TextProxy _title;

        public override void Init()
        {
            base.Init();
            _head = FindInChild<ImageProxy>("BigHead");
            _title = FindInChild<TextProxy>("Top/Back/text");
            _uploadBtn = FindInChild<Button>("Top/SelectBtn");
            _uploadBtn.onClick.AddListener(OnClickUpload);
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as SetHeadContext;
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
        public override void Excute()
        {
            base.Excute();
        }

        public override void PopCallBack()
        {
            if (_context.isHead)
                HeadSpriteUtils.Instance.SaveHeadTexture();
            else
                HeadSpriteUtils.Instance.SaveBackTexture();
            HeadSpriteUtils.Instance.Clear();
            base.PopCallBack();
        }

        private void OnClickUpload()
        {
            HeadSpriteUtils.Instance.Clear();
            HeadSpriteUtils.Instance.UploadTexture(_head);
        }

        private void Refresh()
        {
            if (_context.isHead)
            {
                HeadSpriteUtils.Instance.SetHead(_head);
                _title.text = ContentHelper.Read(ContentHelper.SetUserHead);
            }
            else
            {
                HeadSpriteUtils.Instance.SetBack(_head);
                _title.text = ContentHelper.Read(ContentHelper.SetUseBack);
            }   
        }
    }
	public class SetHeadContext : BaseContext
	{
		public SetHeadContext(bool isHead) : base(UIType.SetHead)
		{
            this.isHead = isHead;
		}
        public bool isHead;
	}
}
