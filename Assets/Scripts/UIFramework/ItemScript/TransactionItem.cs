using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIFrameWork;
using UnityEngine.UI;

public class TransactionItem : ItemBase
{
    private TransactionSaveData _data;
    private Button _btn;

    public override void Init()
    {
        base.Init();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickBtn);
    }

    public override void SetData(object o)
    {
        base.SetData(o);
        _data = o as TransactionSaveData;
    }

    private void OnClickBtn()
    {

    }
}