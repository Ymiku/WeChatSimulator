namespace NodeEditorFramework.Standard
{
	public class GraphTraversal : NodeCanvasTraversal
	{
		GraphCanvasType Canvas;

		public GraphTraversal (GraphCanvasType canvas) : base(canvas)
		{
			Canvas = canvas;
		}

		/// <summary>
		/// Traverse the canvas and evaluate it
		/// </summary>
		public override void TraverseAll () 
		{
			Node rootNode = Canvas.firstNode;
			rootNode.Calculate ();
			//Debug.Log ("RootNode is " + rootNode);
		}
	}
}

