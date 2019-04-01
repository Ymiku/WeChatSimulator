using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectPaywayItem : ItemBase
{
    private Button _selectBtn;

    private void Awake()
    {
        _selectBtn = FindInChild<Button>("");
    }

    public override void SetData(object o)
    {

    }
}
