using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Compiler
{
    public class IDEItem : ItemBase
    {
        public TextProxy text;
        Parameter param;
        void Awake()
        {
            if (GetComponent<Dropdown>() != null)
                GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChange);
        }
        public override void SetData(object o)
        {
            base.SetData(o);
            if (o == null)
            {
                param = null;
                return;
            }
            param = o as Parameter;
            text.text = param.ToString();
            text.width = text.preferredWidth;
            width = text.width + 40.0f;
        }
        public void SetText(string s)
        {
            text.text = s;
            width = text.preferredWidth;
        }
        public void OnValueChange(int i)
        {
            Debug.Log(i);
        }
    }
}