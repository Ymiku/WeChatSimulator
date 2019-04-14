using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
namespace UIFrameWork
{
	public class ContactsView : AnimateView
	{
        public SpellItem spellPrefab;
        public ContactsItem contactsPrefab;
        public Transform content;
        public GameObject requestRedPoint;
		private ContactsContext _context;
        int spellCount = 0;
        int contactsCount = 0;
        List<SpellItem> spellLst = new List<SpellItem>();
        List<ContactsItem> contactsLst = new List<ContactsItem>();
		public override void Init ()
		{
			base.Init ();
            spellPrefab.gameObject.SetActive(false);
            contactsPrefab.gameObject.SetActive(false);
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ContactsContext;
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
        void Refresh()
        {
            requestRedPoint.SetActive(ChatManager.Instance.HasRequestToHandle());
            spellCount = 0;
            contactsCount = 0;
            for (int i = 0; i < spellLst.Count; i++)
            {
                spellLst[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < contactsLst.Count; i++)
            {
                contactsLst[i].gameObject.SetActive(false);
            }
            Dictionary<char,List<int>> nameDic = ChatManager.Instance.init2NameDic;
            float y = -158.0f;
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
                item.cachedRectTransform.anchoredPosition = new Vector2(0.0f,y);
                item.gameObject.SetActive(true);
                y -= item.height;
                for (int m = 0; m < friends.Count; m++)
                {
                    ContactsItem itemc = GetContacts();
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
        ContactsItem GetContacts()
        {
            ContactsItem item = null;
            if (spellLst.Count > spellCount)
            {
                item = contactsLst[contactsCount];
            }
            else
            {
                item = Instantiate(contactsPrefab);
                item.cachedRectTransform.SetParent(content);
                item.cachedRectTransform.localScale = Vector3.one;
                item.cachedRectTransform.anchoredPosition = Vector2.zero;
                contactsLst.Add(item);
            }
            contactsCount++;
            return item;
        }
        public void OnClickNewFriends()
        {
            UIManager.Instance.Push(new NewFriendsContext());
        }
	}
	public class ContactsContext : BaseContext
	{
		public ContactsContext() : base(UIType.Contacts)
		{
		}
	}
}
