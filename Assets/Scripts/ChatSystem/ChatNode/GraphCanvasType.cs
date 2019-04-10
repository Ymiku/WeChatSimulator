using UnityEngine;

namespace NodeEditorFramework.Standard
{
	[NodeCanvasType("Graph Canvas")]
	public class GraphCanvasType : NodeCanvas
	{
		public int sectionID;

		public override string canvasName { get { return "Graph"; } }

		private string rootNodeID { get { return "rootGraphNode"; } }
		public Node firstNode;
		public Node lastNode;

		protected override void OnCreate () 
		{
			//Traversal = new GraphTraversal (this);
			firstNode = Node.Create (rootNodeID, Vector2.zero) as RootGraphNode;
			//rootNode = Node.Create (rootNodeID, Vector2.zero) as RootGraphNode;
		}

		private void OnEnable () 
		{
			//if (Traversal == null)
			//	Traversal = new GraphTraversal (this);
			// Register to other callbacks
			//NodeEditorCallbacks.OnDeleteNode += CheckDeleteNode;
		}

		protected override void ValidateSelf ()
		{
			//if (Traversal == null)
			//	Traversal = new GraphTraversal (this);
			//if (rootNode == null && (rootNode = nodes.Find ((Node n) => n.GetID == rootNodeID) as RootGraphNode) == null)
			//	rootNode = Node.Create (rootNodeID, Vector2.zero) as RootGraphNode;
		}

		public override bool CanAddNode (string nodeID)
		{
			//Debug.Log ("Check can add node " + nodeID);
			if (nodeID == rootNodeID)
				return !nodes.Exists ((Node n) => n.GetID == rootNodeID);
			return true;
		}
		public Node GetFirst()
		{
			if (firstNode != null)
				return firstNode;
			for (int i = 0; i < nodes.Count; i++) {
				if (nodes [i].nodePos == Node.NodePos.First) {
					firstNode = nodes [i];
					break;
				}
			}
			if (firstNode == null) {
				firstNode = nodes [0];
				Debug.LogError ("Did not set first node!");
			}
			return firstNode;
		}
		public Node GetLast()
		{
			if (lastNode != null)
				return lastNode;
			for (int i = 0; i < nodes.Count; i++) {
				if (nodes [i].nodePos == Node.NodePos.Last) {
					lastNode = nodes [i];
					break;
				}
			}
			if (lastNode == null) {
				lastNode = nodes [nodes.Count-1];
				Debug.LogError ("Did not set last node!");
			}
			return lastNode;
		}
        public void GenerateOrder(Node node)
        {
            if (node is ChatOptionNode)
            {
                ChatOptionNode optionNode = node as ChatOptionNode;
                for (int i = 0; i < optionNode.dynamicConnectionPorts.Count; i++)
                {
                    optionNode.dynamicConnectionPorts[i].connections[0].body.orderId = node.orderId + 1;
                    GenerateOrder(optionNode.dynamicConnectionPorts[i].connections[0].body);
                    return;
                }
            }
            if (node.outputKnob == null|| node.outputKnob.connections.Count==0)
                return;
            node.outputKnob.connections[0].body.orderId = node.orderId + 1;
            GenerateOrder(node.outputKnob.connections[0].body);
        }
	}
}
