using UnityEngine;
using System.Collections.Generic;
using NodeEditorFramework.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using NodeEditorFramework;
[Node(false, "Chat/Option Node")]
public class ChatOptionNode : Node
{
	public string tip;
	public const string ID = "ChatOptionNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "玩家选择框"; } }
	public override Vector2 MinSize { get { return new Vector2(260, 10); } }
	public override bool AutoLayout { get { return true; } } // IMPORTANT -> Automatically resize to fit list

	public List<string> labels = new List<string>();
	[ValueConnectionKnob("Input", Direction.In, "System.String")]
	public ValueConnectionKnob inputKnob;
	private ValueConnectionKnobAttribute dynaCreationAttribute = new ValueConnectionKnobAttribute("Output", Direction.Out, "System.String");
#if UNITY_EDITOR
    public override void NodeGUI()
	{
		if (dynamicConnectionPorts.Count != labels.Count)
		{ // Make sure labels and ports are synchronised
			while (dynamicConnectionPorts.Count > labels.Count)
				DeleteConnectionPort(dynamicConnectionPorts.Count - 1);
			while (dynamicConnectionPorts.Count < labels.Count)
				CreateValueConnectionKnob(dynaCreationAttribute);
		}
				

		// Display text field and add button
		GUILayout.BeginHorizontal();
		inputKnob.DisplayLayout ();
		if (cond == Cond.ControlByVar) {
			GUILayout.Label ("提示");
			tip = EditorGUILayout.TextField (tip);
		}
		if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
		{
			labels.Add("聊天选项");
			CreateValueConnectionKnob(dynaCreationAttribute);
		}
		GUILayout.EndHorizontal();

		for (int i = 0; i < labels.Count; i++)
		{ // Display label and delete button
			GUILayout.BeginHorizontal();
			labels[i] = EditorGUILayout.TextField(labels[i]);
			((ValueConnectionKnob)dynamicConnectionPorts[i]).SetPosition();
			if(GUILayout.Button("x", GUILayout.ExpandWidth(false)))
			{ // Remove current label
				labels.RemoveAt (i);
				DeleteConnectionPort(i);
				i--;
			}
			GUILayout.EndHorizontal();
		}
	}
#endif
	public override void TrySetReverseOption (Node front)
	{
		base.TrySetReverseOption (front);
		if (inputKnob.connections.Count <= 1)
			return;
		for (int i = 0; i < inputKnob.connections.Count; i++) {
			if (inputKnob.connections [i].body == front) {
				reverseOption = i;
				return;
			}
		}

	}
    public override Node GetFront ()
	{
        if (inputKnob.connections.Count == 0)
            return null;
        if (inputKnob.connections.Count == 1||IsInEditor())
            return inputKnob.connections[0].body;
		return inputKnob.connections[reverseOption].body;
    }
	public override Node GetNext ()
	{
		if (dynamicConnectionPorts.Count == 0)
			return null;
		if(IsInEditor())
			return dynamicConnectionPorts[0].connections[0].body;
		return dynamicConnectionPorts[option].connections[0].body;
	}
	public override string GetLastSentence ()
	{
		return labels[option];
	}
	public int option{
		get{ return ChatManager.Instance.curExecuteInstance.saveData.GetOption (this);}
		set{ ChatManager.Instance.curExecuteInstance.saveData.AddOption (this, value);}
	}
	public override bool Execute()
	{
        if (ChatManager.Instance.curInstance == null||ChatManager.Instance.curInstance.curRunningNode != this)
			return false;
		switch (cond) {
		case Cond.Instance:
			if (option == -2)
				option++;
			return option>=0;
		case Cond.WaitSeconds:
			if (option == -2)
				option++;
			if (startTime == -1) {
				startTime = GameManager.Instance.localTime;
			}
			if (GameManager.Instance.localTime - startTime >= condParam) {
				startTime = -1;
				return true;
			}
			break;
		case Cond.WaitMinutes:
			if (option == -2)
				option++;
			if (startTime == -1) {
				startTime = GameManager.Instance.localTime;
			}
			if (GameManager.Instance.localTime - startTime >= condParam*60) {
				startTime = -1;
				return true;
			}
			break;
		case Cond.ControlByVar:
			if (XMLSaver.saveData.Check (condName, condParam))
				option = -1;
			return option>=0;//XMLSaver.saveData.Check (condName,condParam);
			break;
		default:
			break;
		}
		return false;
	}
}