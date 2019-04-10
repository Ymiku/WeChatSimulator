using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
public class ChatInstance{
	int curPairID = 0;
	public Node curRunningNode = null;
	public Node nextReadNode = null;
	public GraphCanvasType curSection;
	public long lastChatTimeStamp;
	float totalRectHeight = 0.0f;
	List<Node> _activeNodes = new List<Node> ();
	string selfName;
	public string friendName;
	public string lastSentence = "";
	public ChatInstanceData saveData;
	public int redNum = 0;
	public void OnInit(string friendName)
	{
        ChatManager.Instance.curExecuteInstance = this;
        selfName = ChatManager.Instance.curName;
        int pairId = ChatManager.Instance.GetPairID(selfName,friendName);
		this.friendName = friendName;
		curPairID = pairId;
		saveData = XMLSaver.saveData.GetInstanceData (curPairID);
		lastChatTimeStamp = saveData.lastChatTimeStamp;
		curSection = ChatManager.Instance.LoadSectionByID(curPairID,saveData.curSectionId);
        Node front = null;
        if (saveData.curNodeId >= 0)
        {
            curRunningNode = curSection.nodes[saveData.curNodeId];
            front = curRunningNode.GetFront();
        }
		else if(saveData.curNodeId == ChatManager.finished)
        {
            front = curSection.GetLast();
        }
		else if(saveData.curNodeId == ChatManager.need2Init)
		{
			nextReadNode = curRunningNode = curSection.GetFirst ();
			saveData.curNodeId = curRunningNode.nodeId;
			saveData.readNodeId = nextReadNode.nodeId;
		}
		if (saveData.readNodeId >= 0) {
			nextReadNode = ChatManager.Instance.LoadSectionByID (curPairID, saveData.readSectionId).nodes [saveData.readNodeId];
		} else {
			nextReadNode = ChatManager.Instance.LoadSectionByID (curPairID, saveData.curSectionId).GetLast ();
		}
		if (front != null) {
			lastSentence = GetLastSentence (front);
		}
		totalRectHeight = saveData.totalRectHeight;
	}
	public void OnEnter()
	{
        ChatManager.Instance.curExecuteInstance = this;
        _activeNodes.Clear ();
		redNum = 0;
		//set panel
	}
	public void OnExecute()
	{
        ChatManager.Instance.curExecuteInstance = this;
		if (curRunningNode == null) {
			return;
		}
		bool hasFinished = true;
        int finishCount = 0;
		while (hasFinished&&curRunningNode!=null) {
			hasFinished = false;
			hasFinished = curRunningNode.Execute();
			if (hasFinished) {
				DeactiveNode (curRunningNode);
                finishCount++;
				lastSentence = GetLastSentence (curRunningNode);
				Node temp = curRunningNode;
                curRunningNode = GetNext(curRunningNode);
				if (curRunningNode == null) {
					saveData.curNodeId = -1;
				}
				else
				{
					curRunningNode.TrySetReverseOption (temp);
					ActiveNode (curRunningNode);
				}
			}
		}
        if (finishCount != 0)
        {
            if (curRunningNode != null)
            {
                curSection = ChatManager.Instance.LoadSectionByID(curPairID, curRunningNode.sectionId);
                saveData.curNodeId = curRunningNode.nodeId;
            }
            else
            {
                saveData.curNodeId = -1;
            }
            saveData.curSectionId = curSection.sectionID;
            redNum+=finishCount;
            ChatManager.Instance.RefreshChatLst();
        }
	}
	public void OnExit()
	{
		_activeNodes.Clear ();
	}
	public Node GetLastRunningNode()
	{
		return curRunningNode.GetFront ();
	}
	public string GetLastSentence(Node node)
	{
		if (node == null)
			return "";
		if (node is SetParamNode)
			return GetLastSentence (node.GetFront());
		return node.GetLastSentence();
	}
	public Node GetFront(Node curNode,bool showableOnly = false)
	{
		Node node = null;
		node = curNode.GetFront (showableOnly);
		if (node != null)
			return node;
		if (curNode.sectionId == 0)
			return null;
		GraphCanvasType canvas = ChatManager.Instance.LoadSectionByID (curPairID,curNode.sectionId-1);
		if (showableOnly && canvas.GetLast () is SetParamNode)
			return GetFront (canvas.GetLast (),showableOnly);
		return canvas.GetLast ();
	}
	public Node GetNext(Node curNode,bool showableOnly = false)
	{
		Node node = null;
		node = curNode.GetNext (showableOnly);
		if (node != null)
			return node;
		GraphCanvasType canvas = ChatManager.Instance.LoadSectionByID (curPairID,curNode.sectionId+1);
		if (canvas == null)
			return null;
		if (showableOnly && canvas.GetFirst () is SetParamNode)
			return GetNext (canvas.GetFirst (),showableOnly);
		return canvas.GetFirst ();

	}
    public void ActiveNode(Node node)
    {
        _activeNodes.Add(node);
    }
    public void DeactiveNode(Node node)
    {
        _activeNodes.Remove(node);
    }
	public bool CheckIfUsing(int selectionId)
	{
		if (curSection.sectionID == selectionId)
			return true;
		for (int i = 0; i < _activeNodes.Count; i++) {
			if (selectionId == _activeNodes [i].sectionId)
				return true;
		}
		return false;
	}
	public Node ReadNext()
	{
		nextReadNode = GetNext (nextReadNode,true);
		if (nextReadNode == null) {
			saveData.readSectionId = saveData.curSectionId;
			saveData.readNodeId = -1;
			return null;
		}
		saveData.readSectionId = nextReadNode.sectionId;
		saveData.readNodeId = nextReadNode.nodeId;
		return nextReadNode;
	}
}
