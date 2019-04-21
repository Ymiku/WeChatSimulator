using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[AddComponentMenu("UI/Clipping Image")]
public class ClippingImage : Image
{

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        var r = GetPixelAdjustedRect();
        var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
        var uv = new Vector4(0, 0, 1, 1);
        float a = r.x / r.y;
        float b = sprite.bounds.size.x / sprite.bounds.size.y;
        float c = 0.0f;
        if (a < b)
        {
            /*
            float rWidth = r.y * b;
            float d = rWidth - r.x;
            d *= 0.5f;
            c = d / rWidth;
            */
            c = (r.y * b - r.x) / (r.y*b*2.0f);
            uv = new Vector4(c,0,1.0f-c,1.0f);
        }
        else
        {
            /*
            float rHeight = r.x / b;
            float d2 = rHeight - r.y;
            d2 *= 0.5f;
            c = d2 / rHeight;
            */
            c = (r.x / b - r.y) / (r.x*2.0f/b);
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