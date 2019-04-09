using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
public class ChatInstance{
	int curPairID = 0;
	public Node curRunningNode = null;
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
			front = curSection.GetFirst ();
		}
		if (front != null)
			lastSentence = GetLastSentence (front);
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
                finishCount++;
				lastSentence = GetLastSentence (curRunningNode);
                curRunningNode = GetNext(curRunningNode);
				if (curRunningNode == null) {
					saveData.curNodeId = -1;
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
	public Node GetFront(Node curNode)
	{
		Node node = null;
		node = curNode.GetFront ();
		if (node != null)
			return node;
		if (curNode.sectionId == 0)
			return null;
		GraphCanvasType canvas = ChatManager.Instance.LoadSectionByID (curPairID,curNode.sectionId-1);
		return canvas.GetLast ();
	}
	public Node GetNext(Node curNode)
	{
		Node node = null;
		node = curNode.GetNext ();
		if (node != null)
			return node;
		GraphCanvasType canvas = ChatManager.Instance.LoadSectionByID (curPairID,curNode.sectionId+1);
		if (canvas == null)
			return null;
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
		if (selectionId < _activeNodes [0].sectionId || selectionId > _activeNodes [_activeNodes.Count - 1].sectionId)
			return false;
		return true;
	}
}
