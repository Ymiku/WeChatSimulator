using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[AddComponentMenu("UI/Clipping Image")]
public class ClippingImage : ImageProxy
{

    protected override void OnPopulateMesh(VertexHelper vh)
    {
#if UNITY_EDITOR
        if (rectTransform.pivot != new Vector2(0.5f, 0.5f))
        {
            Debug.LogError("pivot must be 0.5,0.5");
        }
#endif
        var r = GetPixelAdjustedRect();
        var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
        var uv = new Vector4(0, 0, 1, 1);
        float a = r.x / r.y;
        float b = sprite.bounds.size.x / sprite.bounds.size.y;
        float c = 0.0f;
        if (a < b)
        {
			c = ( b - a ) / b * 0.5f;
            uv = new Vector4(c,0,1.0f-c,1.0f);
        }
        else
        {
			c = (a - b) / a * 0.5f;
            uv = new Vector4(0, c, 1.0f, 1.0f-c);
        }
        var color32 = this.color;
        vh.Clear();
       
        vh.AddVert(new Vector3(v.x, v.y ), color32, new Vector2(uv.x, uv.y));
        vh.AddVert(new Vector3(v.x , v.w ), color32, new Vector2(uv.x, uv.w));
        vh.AddVert(new Vector3(v.z , v.w ), color32, new Vector2(uv.z, uv.w));
        vh.AddVert(new Vector3(v.z , v.y ), color32, new Vector2(uv.z, uv.y));

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}