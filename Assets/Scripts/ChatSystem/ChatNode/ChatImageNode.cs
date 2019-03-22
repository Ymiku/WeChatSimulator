using System;
using NodeEditorFramework;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

	[ValueConnectionKnob("Input", Direction.In, "System.String")]
	public ValueConnectionKnob inputKnob;
	[ValueConnectionKnob("Output", Direction.Out, "System.String")]
	public ValueConnectionKnob outputKnob;
	private Vector2 scroll;
	[FormerlySerializedAs("SayingCharacterPotrait")]
	public Sprite CharacterPotrait;
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
	public override Node GetFront ()
	{
		return inputKnob.connections [0].body;
	}
	public override Node GetNext ()
	{
		if (outputKnob.connections.Count == 0)
			return null;
		return outputKnob.connections [0].body;
	}
	public override string GetLastSentence (ChatInstanceData data = null)
	{
		return "[图片]";
	}
}
