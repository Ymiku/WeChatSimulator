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
[Node(false, "Chat/SetParamNode")]
public class SetParamNode : Node
{
	public enum TapType
	{
		None,
		Continus,
		UnContinus,
		Fear,
		Last,
	}
	public const string ID = "SetParamNode";
	public override string GetID { get { return ID; } }

	public override string Title { get { return "变量设置"; } }
	public override Vector2 DefaultSize { get { return new Vector2 (200, 100); } }

	[ValueConnectionKnob("Input", Direction.In, "System.String")]
	public ValueConnectionKnob inputKnob;
	[ValueConnectionKnob("Output", Direction.Out, "System.String")]
	public ValueConnectionKnob outputKnob;
	protected internal override void DrawNode ()
	{
		// Create a rect that is adjusted to the editor zoom and pixel perfect
		Rect nodeRect = rect;
		Vector2 pos = NodeEditor.curEditorState.zoomPanAdjust + NodeEditor.curEditorState.panOffset;
		nodeRect.position = new Vector2((int)(nodeRect.x+pos.x), (int)(nodeRect.y+pos.y));
		contentOffset = new Vector2 (0, 20);

		GUI.color = backgroundColor;

		// Create a headerRect out of the previous rect and draw it, marking the selected node as such by making the header bold
		Rect headerRect = new Rect (nodeRect.x, nodeRect.y, nodeRect.width, contentOffset.y);
		GUI.color = backgroundColor;
		GUI.Box (headerRect, GUIContent.none, GUI.skin.box);
		GUI.color = Color.white;
		GUI.Label (headerRect, Title, NodeEditor.curEditorState.selectedNode == this? NodeEditorGUI.nodeLabelBoldCentered : NodeEditorGUI.nodeLabelCentered);

		// Begin the body frame around the NodeGUI
		Rect bodyRect = new Rect (nodeRect.x, nodeRect.y + contentOffset.y, nodeRect.width, nodeRect.height - contentOffset.y);
		GUI.color = backgroundColor;
		GUI.BeginGroup (bodyRect, GUI.skin.box);
		GUI.color = Color.white;
		bodyRect.position = Vector2.zero;
		GUILayout.BeginArea (bodyRect);
		// Call NodeGUI
		GUI.changed = false;
		if (GUILayout.Button ("Clear All Connect")) {
			DeleteAllPorts ();
		}
		GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("变量名称");
			condName = EditorGUILayout.TextField (condName);
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("参数");
			condParam = EditorGUILayout.IntField (condParam);
			GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();

		NodeGUI ();

		if(Event.current.type == EventType.Repaint)
			nodeGUIHeight = GUILayoutUtility.GetLastRect().max + contentOffset;

		// End NodeGUI frame
		GUILayout.EndArea ();
		GUI.EndGroup ();

		// Automatically node if desired
		AutoLayoutNode ();
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
	public override bool Execute ()
	{
		XMLSaver.saveData.SetValue (condName,condParam);
		return true;
	}
}
