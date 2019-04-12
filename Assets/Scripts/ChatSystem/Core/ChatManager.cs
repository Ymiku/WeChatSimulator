using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
public class ChatManager : Singleton<ChatManager> {
	public const int finished = -1;
	public const int need2Init = -2;
	public delegate void RefreshEventHandler(List<ChatInstance> chatLst);
	public event RefreshEventHandler OnRefreshChatLst;
	//name name selectionID
	public string curName{
		get{return GameManager.Instance.curEnName; }
	}
	public int curUserId;
	public ChatInstance curInstance;
    public ChatInstance curExecuteInstance;
    Dictionary<int,ChatInstance> pairId2Instance = new Dictionary<int, ChatInstance>();
	List<ChatInstance> orderedInstance = new List<ChatInstance>();
    public Dictionary<char, List<string>> init2NameDic;
	//
	public void AddFriend(string name)
	{
		//pairId2Instance.Add (GetPairID(curName,name),new ChatInstance());
		XMLSaver.saveData.instanceID.Add (GetPairID(curName,name));
		ChatInstanceData data = new ChatInstanceData ();
		data.lastChatTimeStamp = GameManager.Instance.time;
		XMLSaver.saveData.instanceData.Add (data);
		OnExit ();
		OnEnter (curName);
	}
	public void DeleteFriend(string name)
	{
		pairId2Instance.Remove (GetPairID(curName,name));
		int i = XMLSaver.saveData.instanceID.IndexOf (GetPairID(curName,name));
		XMLSaver.saveData.instanceID.RemoveAt (i);
		XMLSaver.saveData.instanceData.RemoveAt (i);
		OnExit ();
		OnEnter (curName);
	}
	public Node GetLastRunningNode()
	{
        curExecuteInstance = curInstance;
		return curInstance.GetLastRunningNode ();
	}
	public void EnterChat(string name1,string name2 = "")
	{
        Debug.Log(name1);
		if (name2 == "")
			name2 = curName;
		if(curInstance!=null)
		    curInstance.OnExit ();
		curInstance = pairId2Instance [GetPairID(name1,name2)];
		curInstance.OnEnter ();
	}
    public void ExitChat()
    {
        if (curInstance != null)
            curInstance.OnExit();
    }

	public ChatOptionNode TryGetOptionNode()//get every tick
	{
		if (curInstance.curRunningNode is ChatOptionNode) {
			return (curInstance.curRunningNode as ChatOptionNode);
		}
		return null;
	}
	public void RefreshChatLst()//when new node enter
	{
		orderedInstance.Clear ();
		foreach (var item in pairId2Instance.Values) {
			if (orderedInstance.Count == 0) {
				orderedInstance.Add (item);
				continue;
			}
			long t = item.lastChatTimeStamp;
			for (int i = 0; i < orderedInstance.Count; i++) {
				if (t > orderedInstance [i].lastChatTimeStamp) {
					orderedInstance.Insert (i, item);
					continue;
				}
			}
			orderedInstance.Add (item);
		}
		if(OnRefreshChatLst!=null)
			OnRefreshChatLst (orderedInstance);
	}
	//
	public void OnEnter(string name)
	{
		
		curUserId = GameManager.Instance.curUserId;
		pairId2Instance.Clear ();
		List<string> friends = XMLSaver.saveData.GetFriendsLst (name);
        if (init2NameDic == null)
        {
            init2NameDic = new Dictionary<char, List<string>>();
            for (int i = 0; i < 26; i++)
            {
                init2NameDic.Add((char)(i + 65), new List<string>());
            }
            init2NameDic.Add('#',new List<string>());
        }
        else
        {
            foreach (var item in init2NameDic.Values)
            {
                item.Clear();
            }
        }
		for (int i = 0; i < friends.Count; i++) {
			string friendName = friends[i];
			int id = GetPairID (curName,friends[i]);
			ChatInstance instance = new ChatInstance ();
			instance.OnInit (friendName);
			pairId2Instance.Add (id,instance);

            AccountSaveData data = XMLSaver.saveData.GetAccountData(friendName);
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
            init2NameDic[c].Add(friendName);
            //char c = Utils.GetSpellCode(data);
		}
		RefreshChatLst ();
	}
	//
	public void OnExcute()
	{
		foreach (var item in pairId2Instance.Values) {
			item.OnExecute ();
		}
		CleanPool (8);
	}
	public void OnExit()
	{
		foreach (var item in pairId2Instance.Values) {
			item.OnExit ();
		}
		pairId2Instance.Clear ();
	}
	public int GetPairID(string name,string name2)
	{
		int id = XMLSaver.saveData.GetAccountData(name).accountId;
		int id2 = XMLSaver.saveData.GetAccountData(name2).accountId;
		if (id < id2) {
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
	public List<int> GetFriendRequests(int id)
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
}
