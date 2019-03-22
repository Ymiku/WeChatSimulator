using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.Standard
{
	[Node (false, "Example/Graph Root")]
	public class RootGraphNode : Node 
	{
		public const string ID = "rootGraphNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "发送好友请求"; } }
		public override Vector2 DefaultSize { get { return new Vector2 (150, 110); } }

		[ValueConnectionKnob("Output", Direction.Out, "System.String")]
		public ValueConnectionKnob outputKnob;

		public override void NodeGUI () 
		{
			//name = RTEditorGUI.TextField (name);

			foreach (ConnectionKnob knob in connectionKnobs) 
				knob.DisplayLayout ();
		}
		public override Node GetFront ()
		{
			return null;
		}
		public override Node GetNext ()
		{
			return outputKnob.connections [0].body;
		}
	}
}
