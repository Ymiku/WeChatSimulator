using UnityEngine;
using UnityEngine.UI;

public class SequenceImg : Image
{

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
        /*
        if (sprite)
        {
            var r = GetPixelAdjustedRect();
            var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
            var uv = new Vector4(0, 0, 1, 1);
            uv *= 10000;
            float a = r.x / r.y;
            float b = sprite.bounds.size.x / sprite.bounds.size.y;
            float c = 0.0f;
            if (a < b)
            {
                c = (b - a) / b * 0.5f;
                uv += new Vector4(c, 0, 1.0f - c, 1.0f);
            }
            else
            {
                c = (a - b) / a * 0.5f;
                uv += new Vector4(0, c, 1.0f, 1.0f - c);
            }
            var color32 = this.color;
            toFill.Clear();
            toFill.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
            toFill.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
            toFill.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
            toFill.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

            toFill.AddTriangle(0, 1, 2);
            toFill.AddTriangle(2, 3, 0);
        }
        else
            base.OnPopulateMesh(toFill);
    */
    }
}
