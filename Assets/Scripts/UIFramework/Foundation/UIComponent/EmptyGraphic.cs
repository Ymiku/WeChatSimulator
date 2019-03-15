using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.UI
{
	public class EmptyGraphic : MaskableGraphic {

		protected EmptyGraphic()
		{
			useLegacyMeshGeneration = false;
		}
		protected override void OnPopulateMesh (VertexHelper vh)
		{
			vh.Clear ();
		}
	}
}