using UnityEngine;
using UnityEngine.UI;

public static class SpringGUIDefaultControls
{
    public struct Resources
    {
        public Sprite standard;
        public Sprite background;
        public Sprite inputField;
        public Sprite knob;
        public Sprite checkmark;
        public Sprite dropdown;
        public Sprite mask;
    }

    private static GameObject CreateUIElementRoot(string name, Vector2 size)
    {
        GameObject child = new GameObject(name);
        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        return child;
    }

    private static DefaultControls.Resources convertToDefaultResources(Resources resources)
    {
        DefaultControls.Resources res = new DefaultControls.Resources();
        res.background = resources.background;
        res.checkmark = resources.checkmark;
        res.dropdown = resources.dropdown;
        res.inputField = resources.inputField;
        res.knob = resources.knob;
        res.mask = resources.mask;
        res.standard = resources.standard;
        return res;
    }

    /// <summary>
    /// Create Line Chart Graph
    /// </summary>
    /// <param name="resources"></param>
    /// <returns></returns>
    public static GameObject CreateLineChartGraph(Resources resources)
    {
        // line chart 
        GameObject lienChart = CreateUIElementRoot("LineChart", new Vector2(425, 200));

        // x axis unit
        GameObject xUnit = DefaultControls.CreateText(convertToDefaultResources(resources));
        xUnit.transform.SetParent(lienChart.transform);
        var xrect = xUnit.GetComponent<RectTransform>();
        xrect.pivot = new Vector2(1, 0.5f);
        xUnit.transform.localPosition = new Vector3(-215, -100);

        // y axis unit 
        GameObject yUnit = DefaultControls.CreateText(convertToDefaultResources(resources));
        yUnit.transform.SetParent(lienChart.transform);
        var yrect = yUnit.GetComponent<RectTransform>();
        yrect.pivot = new Vector2(0.5f, 0f);
        yrect.transform.localPosition = new Vector3(-212.5f, 105);
        return lienChart;
    }

}