using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Serialization;
namespace NodeEditorFramework
{
	public abstract partial class Node : ScriptableObject
	{
		public enum Cond
		{
			Instance,
			WaitSeconds,
			WaitMinutes,
			ControlByVar,
		}
		public enum NodePos
		{
			Mid,
			First,
			Last,
		}
		//use at runtime
		public int reverseOption{
			get{ return ChatManager.Instance.curExecuteInstance.saveData.GetReverseOption (this);}
			set{ ChatManager.Instance.curExecuteInstance.saveData.AddReverseOption (this, value);}
		}
		[ValueConnectionKnob("Input", Direction.In, "System.String")]
		public ValueConnectionKnob inputKnob;
		[ValueConnectionKnob("Output", Direction.Out, "System.String")]
		public ValueConnectionKnob outputKnob;
		[NonSerialized]
		public int nodeId;
        [NonSerialized]
        public int orderId;
        public int sectionId;
		[FormerlySerializedAs("Name")]
		public string enname;
		[FormerlySerializedAs("Comd")]
		public Cond cond = Cond.WaitSeconds;
		[FormerlySerializedAs("ConaName")]
		public string condName;
		[FormerlySerializedAs("CondParam")]
		public int condParam = 2;
		public NodePos nodePos = NodePos.Mid;
		[NonSerialized]
		protected int startTime = -1;
		public virtual string GetLastSentence(){return "";}
		public virtual bool Execute()
		{
			switch (cond) {
			case Cond.Instance:
				return true;
			case Cond.WaitSeconds:
				if (startTime == -1) {
					startTime = GameManager.Instance.localTime;
				}
				if (GameManager.Instance.localTime - startTime >= condParam) {
					startTime = -1;
					return true;
				}
				break;
			case Cond.WaitMinutes:
				if (startTime == -1) {
					startTime = GameManager.Instance.localTime;
				}
				if (GameManager.Instance.localTime - startTime >= condParam*60) {
					startTime = -1;
					return true;
				}
				break;
			case Cond.ControlByVar:
				return XMLSaver.saveData.Check (condName,condParam);
				break;
			default:
				break;
			}
			return false;
		}
		//

		public Vector2 position;
		private Vector2 autoSize;
		public Vector2 size { get { return AutoLayout? autoSize : DefaultSize; } }
		public Rect rect { get { return new Rect (position, size); } }
		public Rect fullAABBRect { get { return new Rect(position.x - 20, position.y - 20, size.x + 40, size.y + 40); } }

		// Dynamic connection ports
		public List<ConnectionPort> dynamicConnectionPorts = new List<ConnectionPort>();
		// Static connection ports stored in the actual declaration variables
		[NonSerialized] public List<ConnectionPort> staticConnectionPorts = new List<ConnectionPort>();
		// Representative lists of static port declarations aswell as dynamic ports
		[NonSerialized] public List<ConnectionPort> connectionPorts = new List<ConnectionPort> ();
		[NonSerialized] public List<ConnectionPort> inputPorts = new List<ConnectionPort> ();
		[NonSerialized] public List<ConnectionPort> outputPorts = new List<ConnectionPort> ();
		[NonSerialized] public List<ConnectionKnob> connectionKnobs = new List<ConnectionKnob> ();
		[NonSerialized] public List<ConnectionKnob> inputKnobs = new List<ConnectionKnob> ();
		[NonSerialized] public List<ConnectionKnob> outputKnobs = new List<ConnectionKnob> ();

		// Calculation graph
		[HideInInspector] [NonSerialized]
		internal bool calculated = true;

		// Internal
		internal Vector2 contentOffset = Vector2.zero;
		internal Vector2 nodeGUIHeight;
		internal bool ignoreGUIKnobPlacement;
		internal bool isClipped;

		// Style
		public Color backgroundColor = Color.white;


		#region Properties and Settings

		/// <summary>
		/// Gets the ID of the Node
		/// </summary>
		public abstract string GetID { get; }

		/// <summary>
		/// Specifies the node title.
		/// </summary>
		public virtual string Title { get { 
			#if UNITY_EDITOR
				return UnityEditor.ObjectNames.NicifyVariableName (GetID);
			#else
				return name;
			#endif
			} }

		/// <summary>
		/// Specifies the default size of the node when automatic resizing is turned off.
		/// </summary>
		public virtual Vector2 DefaultSize { get { return new Vector2(200, 100); } }

		/// <summary>
		/// Specifies whether the size of this node should be automatically calculated.
		/// If this is overridden to true, MinSize should be set, too.
		/// </summary>
		public virtual bool AutoLayout { get { return false; } }

		/// <summary>
		/// Specifies the minimum size the node can have if no content is present.
		/// </summary>
		public virtual Vector2 MinSize { get { return new Vector2(100, 50); } }

		/// <summary>
		/// Specifies if this node handles recursive node loops on the canvas.
		/// A loop requires atleast a single node to handle recursion to be permitted.
		/// </summary>
		public virtual bool AllowRecursion { get { return false; } }

		/// <summary>
		/// Specifies if calculation should continue with the nodes connected to the outputs after the Calculation function returns success
		/// </summary>
		public virtual bool ContinueCalculation { get { return true; } }

		#endregion


		#region Node Implementation

		/// <summary>
		/// Initializes the node with Inputs/Outputs and other data if necessary.
		/// </summary>
		protected virtual void OnCreate() {}
		
		/// <summary>
		/// Draws the Node GUI including all controls and potentially Input/Output labels.
		/// By default, it displays all Input/Output labels.
		/// </summary>
		public virtual void NodeGUI () 
		{
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();

			for (int i = 0; i < inputKnobs.Count; i++)
				inputKnobs[i].DisplayLayout ();

			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();

			for (int i = 0; i < outputKnobs.Count; i++)
				outputKnobs[i].DisplayLayout();

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
		}

		/// <summary>
		/// Used to display a custom node property editor in the GUI.
		/// By default shows the standard NodeGUI.
		/// </summary>
		public virtual void DrawNodePropertyEditor ()
		{
			try
			{ // Draw Node GUI without disturbing knob placement
				ignoreGUIKnobPlacement = true;
				NodeEditorGUI.StartNodeGUI(false);
				GUILayout.BeginVertical(GUI.skin.box);
				NodeGUI();
				GUILayout.EndVertical();
				NodeEditorGUI.EndNodeGUI();
			}
			finally
			{ // Be sure to always reset the state to not mess up other GUI code
				ignoreGUIKnobPlacement = false;
			}
		}
		
		/// <summary>
		/// Calculates the outputs of this Node depending on the inputs.
		/// Returns success
		/// </summary>
		public virtual bool Calculate () { return true; }

		#endregion

		#region Callbacks

		/// <summary>
		/// Callback when the node is deleted
		/// </summary>
		protected internal virtual void OnDelete () {}

		/// <summary>
		/// Callback when the given port on this node was assigned a new connection
		/// </summary>
		protected internal virtual void OnAddConnection (ConnectionPort port, ConnectionPort connection) {}

		/// <summary>
		/// Should return all additional ScriptableObjects this Node references
		/// </summary>
		public virtual ScriptableObject[] GetScriptableObjects () { return new ScriptableObject[0]; }

		/// <summary>
		/// Replaces all references to any ScriptableObjects this Node holds with the cloned versions in the serialization process.
		/// </summary>
		protected internal virtual void CopyScriptableObjects (System.Func<ScriptableObject, ScriptableObject> replaceSO) {}

		#endregion


		#region General

		/// <summary>
		/// Creates a node of the specified ID at pos on the current canvas, optionally auto-connecting the specified output to a matching input
		/// </summary>
		public static Node Create (string nodeID, Vector2 pos, ConnectionPort connectingPort = null, bool silent = false, bool init = true)
		{
			return Create (nodeID, pos, NodeEditor.curNodeCanvas, connectingPort, silent, init);
		}

		/// <summary>
		/// Creates a node of the specified ID at pos on the specified canvas, optionally auto-connecting the specified output to a matching input
		/// silent disables any events, init specifies whether OnCreate should be called
		/// </summary>
		public static Node Create (string nodeID, Vector2 pos, NodeCanvas hostCanvas, ConnectionPort connectingPort = null, bool silent = false, bool init = true)
		{
			if (string.IsNullOrEmpty (nodeID) || hostCanvas == null)
				throw new ArgumentException ();
			if (!NodeCanvasManager.CheckCanvasCompability (nodeID, hostCanvas.GetType ()))
				throw new UnityException ("Cannot create Node with ID '" + nodeID + "' as it is not compatible with the current canavs type (" + hostCanvas.GetType ().ToString () + ")!");
			if (!hostCanvas.CanAddNode (nodeID))
				throw new UnityException ("Cannot create Node with ID '" + nodeID + "' on the current canvas of type (" + hostCanvas.GetType ().ToString () + ")!");

			// Create node from data
			NodeTypeData data = NodeTypes.getNodeData (nodeID);
			Node node = (Node)CreateInstance (data.type);
			if(node == null)
				return null;

			// Init node state
			node.enname = node.Title;
			node.autoSize = node.DefaultSize;
			node.position = pos;
			ConnectionPortManager.UpdateConnectionPorts (node);
			if (init)
				node.OnCreate();

			if (connectingPort != null)
			{ // Handle auto-connection and link the output to the first compatible input
				for (int i = 0; i < node.connectionPorts.Count; i++)
				{
					if (node.connectionPorts[i].TryApplyConnection (connectingPort, silent))
						break;
				}
			}

			// Add node to host canvas
			hostCanvas.nodes.Add (node);
			if (!silent)
			{ // Callbacks
				hostCanvas.OnNodeChange(connectingPort != null ? connectingPort.body : node);
				NodeEditorCallbacks.IssueOnAddNode(node);
				hostCanvas.Validate();
				NodeEditor.RepaintClients();
			}

			return node;
		}

		/// <summary>
		/// Deletes this Node from curNodeCanvas and the save file
		/// </summary>
		public void Delete (bool silent = false) 
		{
			if (!NodeEditor.curNodeCanvas.nodes.Contains (this))
				throw new UnityException ("The Node " + enname + " does not exist on the Canvas " + NodeEditor.curNodeCanvas.name + "!");
			if (!silent)
				NodeEditorCallbacks.IssueOnDeleteNode (this);
			NodeEditor.curNodeCanvas.nodes.Remove (this);
			for (int i = 0; i < connectionPorts.Count; i++) 
			{
				connectionPorts[i].ClearConnections (silent);
				DestroyImmediate (connectionPorts[i], true);
			}
			DestroyImmediate (this, true);
			if (!silent)
				NodeEditor.curNodeCanvas.Validate ();
		}

		#endregion

		#region Drawing

#if UNITY_EDITOR
		/// <summary>
		/// If overridden, the Node can draw to the scene view GUI in the Editor.
		/// </summary>
		public virtual void OnSceneGUI()
		{

		}
#endif

        /// <summary>
        /// Draws the node frame and calls NodeGUI. Can be overridden to customize drawing.
        /// </summary>
        protected internal virtual void DrawNode () 
		{
#if UNITY_EDITOR
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
			bool hasFront = (GetFront() != null);
			bool hasNext = (GetNext() != null);
			if((!hasFront)&&hasNext)
			{
				bool isFirst = GUILayout.Toggle(nodePos==NodePos.First,"Is First");
				if(isFirst)
					nodePos = NodePos.First;
				else
					nodePos = NodePos.Mid;
			}
			else if(hasFront&&!hasNext)
			{
				bool isLast = GUILayout.Toggle(nodePos==NodePos.Last,"Is Last");
				if(isLast)
					nodePos = NodePos.Last;
				else
					nodePos = NodePos.Mid;
			}
			else
			{
				nodePos = NodePos.Mid;
			}

			if (GUILayout.Button ("Clear All Connect")) {
				DeleteAllPorts ();
			}

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("姓名");
			enname = EditorGUILayout.TextField (enname);
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();
			GUILayout.Label ("执行条件");
			cond = (Cond)EditorGUILayout.EnumPopup (cond);
			GUILayout.EndVertical ();
			if (cond == Cond.ControlByVar) {
				GUILayout.BeginVertical ();
				GUILayout.Label ("变量名称");
				condName = EditorGUILayout.TextField (condName);
				GUILayout.EndVertical ();
			}
			if (cond != Cond.Instance) {
				GUILayout.BeginVertical ();
				GUILayout.Label ("参数");
				condParam = EditorGUILayout.IntField (condParam);
				GUILayout.EndVertical ();
			}
			GUILayout.EndHorizontal ();

			NodeGUI ();

			if(Event.current.type == EventType.Repaint)
				nodeGUIHeight = GUILayoutUtility.GetLastRect().max + contentOffset;

			// End NodeGUI frame
			GUILayout.EndArea ();
			GUI.EndGroup ();

			// Automatically node if desired
			AutoLayoutNode ();
#endif
        }

        /// <summary>
        /// Resizes the node to either the MinSize or to fit size of the GUILayout in NodeGUI
        /// </summary>
        protected internal virtual void AutoLayoutNode()
		{
			if (!AutoLayout || Event.current.type != EventType.Repaint)
				return;

			Rect nodeRect = rect;
			Vector2 size = new Vector2();
			size.y = Math.Max(nodeGUIHeight.y, MinSize.y) + 4;

			// Account for potential knobs that might occupy horizontal space
			float knobSize = 0;
			List<ConnectionKnob> verticalKnobs = connectionKnobs.Where (x => x.side == NodeSide.Bottom || x.side == NodeSide.Top).ToList ();
			if (verticalKnobs.Count > 0)
				knobSize = verticalKnobs.Max ((ConnectionKnob knob) => knob.GetGUIKnob ().xMax - nodeRect.xMin);
			size.x = Math.Max (knobSize, MinSize.x);
			
			autoSize = size;
			NodeEditor.RepaintClients ();
		}
        /// <summary>
        /// Draws the connectionKnobs of this node
        /// </summary>
        protected internal virtual void DrawKnobs () 
		{
			for (int i = 0; i < connectionKnobs.Count; i++) 
				connectionKnobs[i].DrawKnob ();
		}

		/// <summary>
		/// Draws the connections from this node's connectionPorts
		/// </summary>
		protected internal virtual void DrawConnections () 
		{
			if (Event.current.type != EventType.Repaint)
				return;
			for (int i = 0; i < outputPorts.Count; i++)
				outputPorts[i].DrawConnections ();
		}

		#endregion

		#region Node Utility

		/// <summary>
		/// Tests the node whether the specified position is inside any of the node's elements and returns a potentially focused connection knob.
		/// </summary>
		public bool ClickTest(Vector2 position, out ConnectionKnob focusedKnob)
		{
			focusedKnob = null;
			if (rect.Contains(position))
				return true;
			for (int i = 0; i < connectionKnobs.Count; i++)
			{ // Check if any nodeKnob is focused instead
				if (connectionKnobs[i].GetCanvasSpaceKnob().Contains(position))
				{
					focusedKnob = connectionKnobs[i];
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns whether the node acts as an input (no inputs or no inputs assigned)
		/// </summary>
		public bool isInput()
		{
			for (int i = 0; i < inputPorts.Count; i++)
				if (inputPorts[i].connected())
					return false;
			return true;
		}

		/// <summary>
		/// Returns whether the node acts as an output (no outputs or no outputs assigned)
		/// </summary>
		public bool isOutput()
		{
			for (int i = 0; i < outputPorts.Count; i++)
				if (outputPorts[i].connected())
					return false;
			return true;
		}

		/// <summary>
		/// Returns whether every direct ancestor has been calculated
		/// </summary>
		public bool ancestorsCalculated () 
		{
			for (int i = 0; i < inputPorts.Count; i++)
			{
				ConnectionPort port = inputPorts[i];
				for (int t = 0; t < port.connections.Count; t++)
				{
					if (!port.connections[t].body.calculated)
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Recursively checks whether this node is a child of the other node
		/// </summary>
		public bool isChildOf (Node otherNode)
		{
			if (otherNode == null || otherNode == this)
				return false;
			if (BeginRecursiveSearchLoop ()) return false;
			for (int i = 0; i < inputPorts.Count; i++)
			{
				ConnectionPort port = inputPorts[i];
				for (int t = 0; t < port.connections.Count; t++)
				{
					ConnectionPort conPort = port.connections[t];
					if (conPort.body != startRecursiveSearchNode && (conPort.body == otherNode || conPort.body.isChildOf(otherNode)))
					{
						StopRecursiveSearchLoop();
						return true;
					}
				}
			}
			EndRecursiveSearchLoop ();
			return false;
		}

		/// <summary>
		/// Recursively checks whether this node is in a loop
		/// </summary>
		internal bool isInLoop ()
		{
			if (BeginRecursiveSearchLoop ()) return this == startRecursiveSearchNode;
			for (int i = 0; i < inputPorts.Count; i++)
			{
				ConnectionPort port = inputPorts[i];
				for (int t = 0; t < port.connections.Count; t++)
				{
					if (port.connections[t].body.isInLoop())
					{
						StopRecursiveSearchLoop();
						return true;
					}
				}
			}
			EndRecursiveSearchLoop ();
			return false;
		}

		/// <summary>
		/// Recursively checks whether any node in the loop to be made allows recursion.
		/// Other node is the node this node needs connect to in order to fill the loop (other node being the node coming AFTER this node).
		/// That means isChildOf has to be confirmed before calling this!
		/// </summary>
		internal bool allowsLoopRecursion (Node otherNode)
		{
			if (AllowRecursion)
				return true;
			if (otherNode == null)
				return false;
			if (BeginRecursiveSearchLoop ()) return false;
			for (int i = 0; i < inputPorts.Count; i++)
			{
				ConnectionPort port = inputPorts[i];
				for (int t = 0; t < port.connections.Count; t++)
				{
					if (port.connections[t].body.allowsLoopRecursion(otherNode))
					{
						StopRecursiveSearchLoop();
						return true;
					}
				}
			}
			EndRecursiveSearchLoop ();
			return false;
		}

		/// <summary>
		/// A recursive function to clear all calculations depending on this node.
		/// Usually does not need to be called manually
		/// </summary>
		public void ClearCalculation () 
		{
			calculated = false;
			if (BeginRecursiveSearchLoop ()) return;
			for (int i = 0; i < outputPorts.Count; i++)
			{
				ConnectionPort port = outputPorts[i];
				if (port is ValueConnectionKnob)
					(port as ValueConnectionKnob).ResetValue ();
				for (int t = 0; t < port.connections.Count; t++)
				{
					ConnectionPort conPort = port.connections[t];
					if (conPort is ValueConnectionKnob)
						(conPort as ValueConnectionKnob).ResetValue ();
					conPort.body.ClearCalculation ();
				}
			}
			EndRecursiveSearchLoop ();
		}

		#region Recursive Search Helpers

		[NonSerialized] private List<Node> recursiveSearchSurpassed;
		[NonSerialized] private Node startRecursiveSearchNode; // Temporary start node for recursive searches

		/// <summary>
		/// Begins the recursive search loop and returns whether this node has already been searched
		/// </summary>
		internal bool BeginRecursiveSearchLoop ()
		{
			if (startRecursiveSearchNode == null || recursiveSearchSurpassed == null) 
			{ // Start search
				recursiveSearchSurpassed = new List<Node> ();
				startRecursiveSearchNode = this;
			}

			if (recursiveSearchSurpassed.Contains (this))
				return true;
			recursiveSearchSurpassed.Add (this);
			return false;
		}

		/// <summary>
		/// Ends the recursive search loop if this was the start node
		/// </summary>
		internal void EndRecursiveSearchLoop () 
		{
			if (startRecursiveSearchNode == this) 
			{ // End search
				recursiveSearchSurpassed = null;
				startRecursiveSearchNode = null;
			}
		}

		/// <summary>
		/// Stops the recursive search loop immediately. Call when you found what you needed.
		/// </summary>
		internal void StopRecursiveSearchLoop () 
		{
			recursiveSearchSurpassed = null;
			startRecursiveSearchNode = null;
		}

		#endregion

		#endregion

		#region Knob Utility

		public ConnectionPort CreateConnectionPort(ConnectionPortAttribute specificationAttribute)
		{
			ConnectionPort port = specificationAttribute.CreateNew(this);
			if (port == null)
				return null;
			dynamicConnectionPorts.Add(port);
			ConnectionPortManager.UpdateRepresentativePortLists(this);
			return port;
		}

		public ConnectionKnob CreateConnectionKnob(ConnectionKnobAttribute specificationAttribute)
		{
			return (ConnectionKnob)CreateConnectionPort(specificationAttribute);
		}

		public ValueConnectionKnob CreateValueConnectionKnob(ValueConnectionKnobAttribute specificationAttribute)
		{
			return (ValueConnectionKnob)CreateConnectionPort(specificationAttribute);
		}

		public void DeleteConnectionPort(ConnectionPort dynamicPort)
		{
			dynamicPort.ClearConnections ();
			dynamicConnectionPorts.Remove(dynamicPort);
			DestroyImmediate(dynamicPort);
			ConnectionPortManager.UpdateRepresentativePortLists(this);
		}

		public void DeleteConnectionPort(int dynamicPortIndex)
		{
			if (dynamicPortIndex >= 0 && dynamicPortIndex < dynamicConnectionPorts.Count)
				DeleteConnectionPort(dynamicConnectionPorts[dynamicPortIndex]);
		}
		public void DeleteAllPorts()
		{
			for (int i = 0; i < connectionPorts.Count; i++) {
				connectionPorts[i].ClearConnections (true);
			}
		}
		public virtual Node GetFront (bool showableOnly = false){
			if (inputKnob==null||inputKnob.connections.Count == 0)
				return null;
			Node result = null;
			if (inputKnob.connections.Count == 1||IsInEditor())
				result = inputKnob.connections[0].body;
			else
				result = inputKnob.connections[reverseOption].body;
			if (showableOnly&&result is SetParamNode)
				return result.GetFront (showableOnly);
			return result;
		}
		public virtual Node GetNext (bool showableOnly = false){
			if (outputKnob==null||outputKnob.connections.Count == 0)
				return null;
			Node result = outputKnob.connections [0].body;
			if (showableOnly&&result is SetParamNode)
				return result.GetFront (showableOnly);
			return result;
		}
		public virtual void TrySetReverseOption(Node front)
		{
			
		}
		protected bool IsInEditor()
		{
			return XMLSaver.saveData == null;
		}
		#endregion
	}
}
