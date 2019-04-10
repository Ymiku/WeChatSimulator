using System;
using NodeEditorFramework;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// basic dialog node class, all other dialog nodes are derived from this
/// </summary>
[Node(false, "Chat/ChatImageNode")]
public class ChatImageNode : Node
{
	public const string ID = "ChatImageNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "图片对话框"; } }
	public override Vector2 DefaultSize { get { return new Vector2 (200, 180); } }

	private Vector2 scroll;
	[FormerlySerializedAs("SayingCharacterPotrait")]
	public Sprite CharacterPotrait;
#if UNITY_EDITOR
    public override void NodeGUI () 
	{
		// Display Float connections
		GUILayout.BeginHorizontal ();
		inputKnob.DisplayLayout ();
		outputKnob.DisplayLayout ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal ();
		//GUILayout.BeginVertical();
		//GUILayout.EndVertical();
		CharacterPotrait = (Sprite)EditorGUILayout.ObjectField(CharacterPotrait, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical();
		/*
		// Get adjacent flow elements
		Node flowSource = flowIn.connected ()? flowIn.connections[0].body : null;
		List<Node> flowTargets = flowOut.connections.Select ((ConnectionKnob input) => input.body).ToList ();

		// Display adjacent flow elements
		GUILayout.Label ("Flow Source: " + (flowSource != null? flowSource.name : "null"));
		GUILayout.Label ("Flow Targets:");
		foreach (Node flowTarget in flowTargets)
			GUILayout.Label ("-> " + flowTarget.name);
		*/
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
	public override Node GetFront (bool showableOnly = false)
	{
        if (inputKnob.connections.Count == 0)
            return null;
        if (inputKnob.connections.Count == 1||IsInEditor())
            return inputKnob.connections[0].body;
		return inputKnob.connections[reverseOption].body;
    }
	public override Node GetNext (bool showableOnly = false)
	{
		if (outputKnob.connections.Count == 0)
			return null;
		return outputKnob.connections [0].body;
	}
	public override string GetLastSentence ()
	{
		return "[图片]";
	}
}
