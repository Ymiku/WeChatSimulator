using System; 
using UnityEngine; 

namespace UnityStandardAssets.CinematicEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Cinematic/Black")]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public class BlackEffect : MonoBehaviour
    { 
		public Shader TintShader = null; 
		private float _count = 1f;
		public float count
		{
			get{
				return _count;
			}
			set{ 
				_count = value;
			}
		}
		public Material TintMaterial = null; 

		public bool CheckResources () 
		{ 
			TintMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(TintShader);
			return true; 
		} 
		void OnRenderImage (RenderTexture source, RenderTexture destination) 
		{ 
			if (CheckResources()==false) 
			{ 
				Graphics.Blit (source, destination); 
				return; 
			} 
			TintMaterial.SetFloat("count", Mathf.Clamp01(_count)); 
			//Do a full screen pass using TintMaterial 
			Graphics.Blit (source, destination, TintMaterial); 
		} 
	} 
}
