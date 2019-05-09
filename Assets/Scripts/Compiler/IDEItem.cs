using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Compiler
{
    public class IDEItem : ItemBase
    {
        public Button button;
		public FDropdown dropDown;
        public TextProxy text;
        Parameter param;
       
        public void SetIDEData(Parameter o)
        {
            param = o;
			width = 1000.0f;
			//text.sizeDelta = new Vector2 (1000.0f,400.0f);
			text.text = param.GenerateCode();
            text.width = text.preferredWidth;
            width = text.width + 40.0f;
            RefreshDropdown();
        }
        public void RefreshDropdown()
        {
            dropDown.Clear();
            List<System.Type> options = HackStudioCode.Instance.GetTypesByReturnValue(param.paramType);
            for (int i = 0; i < options.Count; i++)
            {
                dropDown.AddOption(options[i].Name.Substring(options[i].Name.IndexOf("Statement")+9));
            }
        }
		public void SetIDEData(string s)
		{
			param = Parameter.Empty;
			text.text = s;
			width = text.preferredWidth;
		}
        public void OnValueChange(int i)
        {
			StatementBase s = (StatementBase)Activator.CreateInstance (HackStudioCode.Instance.GetTypesByReturnValue (param.paramType) [i], true);
			param.Set (s);
			HackStudioCode.Instance.SetParam (id,param);
			SetIDEData (param);
        }
		public void StepIn()
		{
			GetComponentInParent<HackStudioCode> ().StepIn (id);
		}
    }
}