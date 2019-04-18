using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
public class ChatManager : Singleton<ChatManager> {
	public const int finished = -1;
	public const int need2Init = -2;
	public delegate void RefreshEventHandler(List<ChatInstance> chatLst);
	public event RefreshEventHandler OnNewMsgOccur;
	//name name selectionID
	public string curName{
		get{return GameManager.Instance.curEnName; }
	}
	public int curUserId
    {
        get { return GameManager.Instance.curUserId; }
    }
    public ChatInstance curInstance;
    public ChatInstance curExecuteInstance;
    Dictionary<int,ChatInstance> pairId2Instance = new Dictionary<int, ChatInstance>();
	List<ChatInstance> orderedInstance = new List<ChatInstance>();
    public Dictionary<char, List<int>> init2NameDic;
	//
	public void AddFriend(string name)
	{
        AddFriend(XMLSaver.saveData.GetAccountData(name).accountId);
	}
    public void AddFriend(int id)
    {
        XMLSaver.saveData.instanceID.Add(GetPairID(curUserId, id));
        ChatInstanceData data = new ChatInstanceData();
        data.lastChatTimeStamp = GameManager.Instance.time;
        XMLSaver.saveData.instanceData.Add(data);
        OnFriendsLstChange();
    }
    public void DeleteFriend(string name)
    {
        DeleteFriend(XMLSaver.saveData.GetAccountData(name).accountId);
    }
    public void DeleteFriend(int id)
	{
		pairId2Instance.Remove (GetPairID(curUserId,id));
		int i = XMLSaver.saveData.instanceID.IndexOf (GetPairID(curUserId, id));
		XMLSaver.saveData.instanceID.RemoveAt (i);
		XMLSaver.saveData.instanceData.RemoveAt (i);
		OnFriendsLstChange ();
	}
	public Node GetLastRunningNode()
	{
        curExecuteInstance = curInstance;
		return curInstance.GetLastRunningNode ();
	}
	public void EnterChat(int friendId)
	{
		if(curInstance!=null)
		    curInstance.OnExit ();
		curInstance = pairId2Instance [GetPairID(curUserId,friendId)];
		curInstance.OnEnter ();
	}
    public void ExitChat()
    {
        if (curInstance != null)
            curInstance.OnExit();
    }

	public ChatOptionNode TryGetOptionNode()//get every tick
	{
		if (curInstance!=null&&curInstance.curRunningNode is ChatOptionNode) {
			return (curInstance.curRunningNode as ChatOptionNode);
		}
		return null;
	}
	public void RefreshMsg()//when new node enter
	{
		orderedInstance.Clear ();
		foreach (var item in pairId2Instance.Values) {
			if (orderedInstance.Count == 0) {
				orderedInstance.Add (item);
				continue;
			}
			long t = item.lastChatTimeStamp;
            orderedInstance.Add(item);
            for (int i = 0; i < orderedInstance.Count; i++) {
				if (t > orderedInstance [i].lastChatTimeStamp) {
                    orderedInstance.RemoveAt(orderedInstance.Count-1);
					orderedInstance.Insert (i, item);
                    break;
				}
			}
			
		}
		if(OnNewMsgOccur!=null)
			OnNewMsgOccur (orderedInstance);
	}
	//
	public void OnFriendsLstChange()
	{
        foreach (var item in pairId2Instance.Values)
        {
            item.OnExit();
        }
        pairId2Instance.Clear();
		List<int> friends = XMLSaver.saveData.GetFriendsLst (curUserId);
        if (init2NameDic == null)
        {
            init2NameDic = new Dictionary<char, List<int>>();
            for (int i = 0; i < 26; i++)
            {
                init2NameDic.Add((char)(i + 65), new List<int>());
            }
            init2NameDic.Add('#',new List<int>());
        }
        else
        {
            foreach (var item in init2NameDic.Values)
            {
                item.Clear();
            }
        }
		for (int i = 0; i < friends.Count; i++) {
			int friendId = friends[i];
			int id = GetPairID (curUserId,friendId);
			ChatInstance instance = new ChatInstance ();
			instance.OnInit (friendId);
			pairId2Instance.Add (id,instance);

            AccountSaveData data = XMLSaver.saveData.GetAccountData(friendId);
            char c;
            if (!string.IsNullOrEmpty(data.nickname))
            {
               c = Utils.GetSpellCode(data.nickname)[0];
            }
            else if (!string.IsNullOrEmpty(data.realName))
            {
                c = Utils.GetSpellCode(data.realName)[0];
            }
            else
            {
                c = '#';
            }
            init2NameDic[c].Add(friendId);
            //char c = Utils.GetSpellCode(data);
		}
		RefreshMsg ();
	}
	//
	public void OnExcute()
	{
		foreach (var item in pairId2Instance.Values) {
			item.OnExecute ();
		}
		CleanPool (8);
	}
    public int GetPairID(int id,int id2)
    {
        if (id < id2)
        {
            return (id << 8) + id2;
        }
        return (id2 << 8) + id;
    }

    int max = 20;
	List<int> poolList = new List<int>();
	Dictionary<int,GraphCanvasType> selectionPool = new Dictionary<int, GraphCanvasType> ();
	public GraphCanvasType LoadSectionByID(int pairId,int id)
	{
		int aid = (pairId << 8) + id;
		int index = poolList.IndexOf (aid);
		if (index == -1) {
			GraphCanvasType c = Resources.Load<GraphCanvasType> ("Sections/" + pairId.ToString () + "/" + id.ToString ());
            if (c == null)
                return null;
			c.sectionID = id;
			for (int i = 0; i < c.nodes.Count; i++) {
				c.nodes [i].nodeId = i;
				c.nodes [i].sectionId = id;
			}
            if(c.GetFirst()!=null)
                c.GenerateOrder(c.GetFirst());
			poolList.Add (aid);
			selectionPool.Add (aid,c);
			return c;
		}
		poolList.RemoveAt (index);
		poolList.Add (aid);
		return selectionPool[aid];
	}
	void CleanPool(int maxCount)
	{
		if (poolList.Count <= maxCount)
			return;
		int need2CleanNum = poolList.Count - maxCount;
		for (int i = 0; i < need2CleanNum; i++) {
			bool isUsing = false;
			foreach (var item in pairId2Instance.Values) {
				if (item.CheckIfUsing (poolList [0])) {
					isUsing = true;
					break;
				}
			}
			if (!isUsing) {
				selectionPool.Remove (poolList[0]);
				poolList.RemoveAt (0);
			}
		}
	}

	public void SendFriendRequest(int from,int to)
	{
		XMLSaver.saveData.friendRequests.Add ((from<<8)+to);
	}
	public List<int> GetFriendRequests()
	{
		List<int> requests = XMLSaver.saveData.friendRequests;
		List<int> outLst = new List<int> ();
		for (int i = 0; i < requests.Count; i++) {
			if ((requests [i] & 255) == curUserId||((requests [i]>>8) & 255) == curUserId) {
				outLst.Add (requests[i]);
			}
		}
		return outLst;
	}
    public void HandleRequest(int request)
    {
        XMLSaver.saveData.friendRequests.Remove(request);
        XMLSaver.saveData.friendRequests.Add(request+(1<<16));
        if ((request & 255) == curUserId)
        {
            AddFriend((request >> 8) & 255);
        }
        else
        {
            AddFriend(request & 255);
        }
    }
    public bool HasRequestToHandle()
    {
        List<int> requests = GetFriendRequests();
        for (int i = 0; i < requests.Count; i++)
        {
            if ((requests[i] >> 16) == 0)
                return true;
        }
        return false;
    }
}
