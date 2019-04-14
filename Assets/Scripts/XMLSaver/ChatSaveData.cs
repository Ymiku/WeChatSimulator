using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NodeEditorFramework;
using NodeEditorFramework.Standard;

public partial class SaveData
{
	public List<int> friendRequests = new List<int> ();
	public List<int> instanceID = new List<int> ();
	public List<ChatInstanceData> instanceData = new List<ChatInstanceData> ();
    //对话控制变量
	public List<string> varName = new List<string>();
	public List<int> varValue = new List<int>();
	public int GetValue(string valueName)
	{
		int i = varName.IndexOf (valueName);
		if (i != -1)
			return varValue [i];
        SetValue(valueName,0);
		return 0;
	}
	public void SetValue(string valueName,int value)
	{
		int i = varName.IndexOf (valueName);
        if (i != -1)
        {
            varValue[i] = value;
            return;
        }
		varName.Add (valueName);
		varValue.Add (value);
	}
	public List<int> GetFriendsLst(int selfId)
	{
		List<int> result = new List<int> ();
		for (int i = 0; i < instanceID.Count; i++) {
			int m = (instanceID [i] >> 8);
			int n = (instanceID [i] & 255);
			if(m!=selfId&&n!=selfId)
				continue;
			if(m==selfId)
				result.Add(n);
			else
				result.Add(m);
		}
		return result;
	}
	public ChatInstanceData GetInstanceData(int pairId)
	{
		int i = instanceID.IndexOf (pairId);
		if (i == -1) {
			instanceID.Add (pairId);
			instanceData.Add (new ChatInstanceData());
			i = instanceData.Count - 1;
		}
		return instanceData[i];
	}
	public bool Check(string varname,int varvalue)
	{
		int index = varName.IndexOf(varname);
		if (index == -1) {
			varName.Add (varname);
			varValue.Add (0);
			index = varName.Count - 1;
		}
		return varValue [index] <= varvalue;
	}
}
[System.Serializable]
public class ChatInstanceData
{
	public int curSectionId;
	public int curNodeId = ChatManager.need2Init;
	public int readSectionId;
	public int readNodeId;
	public long lastChatTimeStamp;
    public int redCount;
	public float totalRectHeight;

	public List<int> nodeIds = new List<int>();
	public List<int> nodeOptions = new List<int>();
	public List<int> reverseNodeIds = new List<int>();
	public List<int> reverseNodeOptions = new List<int>();

	//section<<8+id
	public void AddOption(Node node,int option)
	{
		int index = nodeIds.IndexOf (node.nodeId + (node.sectionId << 8));
		if (index == -1) {
			nodeIds.Add (node.nodeId + (node.sectionId << 8));
			nodeOptions.Add (option);
		} else {
			nodeOptions [index] = option;
		}
	}
	public int GetOption(Node node)
	{
		int index = nodeIds.IndexOf (node.nodeId + (node.sectionId << 8));
		if (index == -1) {
			nodeIds.Add (node.nodeId + (node.sectionId << 8));
			nodeOptions.Add (ChatManager.need2Init);
			return ChatManager.need2Init;
		}
		return nodeOptions [index];
	}
	public void AddReverseOption(Node node,int option)
	{
		int index = reverseNodeIds.IndexOf (node.nodeId + (node.sectionId << 8));
		if (index == -1) {
			reverseNodeIds.Add (node.nodeId + (node.sectionId << 8));
			reverseNodeOptions.Add (option);
		} else {
			reverseNodeOptions [index] = option;
		}
	}
	public int GetReverseOption(Node node)
	{
		int index = reverseNodeIds.IndexOf (node.nodeId + (node.sectionId << 8));
		if (index == -1) {
			reverseNodeIds.Add (node.nodeId + (node.sectionId << 8));
			reverseNodeOptions.Add (-2);
			return -2;
		}
		return reverseNodeOptions [index];
	}

}