using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

public class AntBillDateItem : ItemBase
{
    public TextProxy date;
    public void SetData(string s)
    {
        date.text = s;
    }
}