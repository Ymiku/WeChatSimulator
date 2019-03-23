using UnityEngine;
using UnityEngine.UI;

public interface ILineChart
{
    VertexHelper DrawLineChart(VertexHelper vh, Rect rect, LineChartData basis);
    VertexHelper DrawMesh(VertexHelper vh);
    VertexHelper DrawAxis(VertexHelper vh);
}