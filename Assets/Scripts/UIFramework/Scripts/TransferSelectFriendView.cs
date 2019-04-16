using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIFrameWork
{
	public class TransferSelectFriendView : AnimateView
	{
		private TransferSelectFriendContext _context;
        public SpellItem spellPrefab;
        public TransferSelectFriendItem selectPrefab;
        public Transform content;
        int spellCount = 0;
        int contactsCount = 0;
        List<SpellItem> spellLst = new List<SpellItem>();
        List<TransferSelectFriendItem> selectLst = new List<TransferSelectFriendItem>();

        public override void Init ()
		{
			base.Init ();
            spellPrefab.gameObject.SetActive(false);
            selectPrefab.gameObject.SetActive(false);
        }
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as TransferSelectFriendContext;
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
        public override void PopCallBack()
        {
            UIManager.Instance.Push(new TransferToAccountContext());
            base.PopCallBack();
        }
        void Refresh()
        {
            spellCount = 0;
            contactsCount = 0;
            for (int i = 0; i < spellLst.Count; i++)
            {
                spellLst[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < selectLst.Count; i++)
            {
                selectLst[i].gameObject.SetActive(false);
            }
            Dictionary<char, List<int>> nameDic = ChatManager.Instance.init2NameDic;
            float y = 0f;
            for (int i = 0; i < 27; i++)
            {
                List<int> friends;
                if (i == 26)
                {
                    friends = nameDic['#'];
                }
                else
                {
                    friends = nameDic[(char)(65 + i)];
                }
                if (friends.Count == 0)
                    continue;
                SpellItem item = GetSpell();
                item.SetData(((char)(65 + i)).ToString());
                item.cachedRectTransform.anchoredPosition = new Vector2(0.0f, y);
                item.gameObject.SetActive(true);
                y -= item.height;
                for (int m = 0; m < friends.Count; m++)
                {
                    TransferSelectFriendItem itemc = GetContacts();
                    itemc.SetData(XMLSaver.saveData.GetAccountData(friends[m]));
                    itemc.cachedRectTransform.anchoredPosition = new Vector2(0.0f, y);
                    itemc.gameObject.SetActive(true);
                    y -= itemc.height;
                }
            }

        }
        SpellItem GetSpell()
        {
            SpellItem item = null;
            if (spellLst.Count > spellCount)
            {
                item = spellLst[spellCount];
            }
            else
            {
                item = Instantiate(spellPrefab);
                item.cachedRectTransform.SetParent(content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                spellLst.Add(item);
            }
            spellCount++;
            return item;
        }
        TransferSelectFriendItem GetContacts()
        {
            TransferSelectFriendItem item = null;
            if (spellLst.Count > spellCount)
            {
                item = selectLst[contactsCount];
            }
            else
            {
                item = Instantiate(selectPrefab);
                item.cachedRectTransform.SetParent(content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                selectLst.Add(item);
            }
            contactsCount++;
            return item;
        }
    }
	public class TransferSelectFriendContext : BaseContext
	{
		public TransferSelectFriendContext() : base(UIType.TransferSelectFriend)
		{
		}
	}
}
