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

        public override void Init()
        {
            base.Init();
            _head = FindInChild<ImageProxy>("BigHead");
            _uploadBtn = FindInChild<Button>("Top/SelectBtn");
            _uploadBtn.onClick.AddListener(OnClickUpload);
        }
        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            _context = context as SetHeadContext;
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
            HeadSpriteUtils.Instance.SaveTexture();
            base.PopCallBack();
        }

        private void OnClickUpload()
        {
            HeadSpriteUtils.Instance.UploadTexture(_head);
        }

        private void Refresh()
        {
            HeadSpriteUtils.Instance.SetHead(_head);
        }
    }
	public class SetHeadContext : BaseContext
	{
		public SetHeadContext() : base(UIType.SetHead)
		{
		}
	}
}
