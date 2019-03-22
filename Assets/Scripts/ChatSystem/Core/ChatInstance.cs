using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
public class ChatInstance{
	int curPairID = 0;
	public Node curRunningNode = null;
	GraphCanvasType curSection;
	public long lastChatTimeStamp;
	float totalRectHeight = 0.0f;
	List<Node> _activeNodes = new List<Node> ();
	string userName;
	string otherUserName;
	public string lastSentence = "";
	public ChatInstanceData saveData;

	public void OnInit(string name,string otherName,int pairId)
	{
		userName = name;
		otherUserName = otherName;
		curPairID = pairId;
		saveData = XMLSaver.saveData.GetInstanceData (curPairID);
		lastChatTimeStamp = saveData.lastChatTimeStamp;
		curSection = ChatManager.Instance.LoadSectionByID(curPairID,saveData.curSectionId);
		if(saveData.curNodeId!=-1)
			curRunningNode = curSection.nodes [saveData.curNodeId];
		Node front = curRunningNode.GetFront ();
		if (front != null)
			lastSentence = GetLastSentence (front);
		totalRectHeight = saveData.totalRectHeight;
	}
	public void OnEnter()
	{
		_activeNodes.Clear ();
		//set panel
	}
	public void OnExecute()
	{
		if (curRunningNode == null) {
			return;
		}
		bool hasFinished = true;
		while (hasFinished&&curRunningNode!=null) {
			hasFinished = false;
			hasFinished = curRunningNode.Execute();
			if (hasFinished) {
				lastSentence = GetLastSentence (curRunningNode);
				curRunningNode = curRunningNode.GetNext ();
				if (curRunningNode == null) {
					saveData.curNodeId = -1;
					return;
				}
			}
		}
		curSection = ChatManager.Instance.LoadSectionByID (curPairID,curRunningNode.sectionId);
		saveData.curNodeId = curRunningNode.nodeId;
		saveData.curSectionId = curSection.sectionID;
		ChatManager.Instance.Refresh ();
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
		return node.GetLastSentence(saveData);
	}
	public Node GetFront()
	{
		Node node = null;
		node = _activeNodes [0].GetFront ();

		_activeNodes.Insert (0,node);
		return node;
	}
	public Node GetNext()
	{
		Node node = null;
		node = _activeNodes [_activeNodes.Count - 1].GetNext ();
		_activeNodes.Add (node);
		return node;
	}
	public void PoolUp()
	{
		if (_activeNodes.Count != 0)
			_activeNodes.RemoveAt (0);
	}
	public void PoolDown()
	{
		if (_activeNodes.Count != 0)
			_activeNodes.RemoveAt (_activeNodes.Count-1);
	}
	Node GetFront(Node curNode)
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
	Node GetNext(Node curNode)
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
	public bool CheckIfUsing(int selectionId)
	{
		if (curSection.sectionID == selectionId)
			return true;
		if (selectionId < _activeNodes [0].sectionId || selectionId > _activeNodes [_activeNodes.Count - 1].sectionId)
			return false;
		return true;
	}
}
