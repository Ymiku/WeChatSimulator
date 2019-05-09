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
			List<System.Type> options = null;
			if (param.isVoid) {
				options = HackStudioCode.Instance.GetTypesByReturnValue(VarType.Void);
			} else {
				options = HackStudioCode.Instance.GetTypesByReturnValue(param.paramType);
			}
			Debug.Log (options.Count);
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
			StatementBase s = null;
			if (param.isVoid) {
				s = (StatementBase)Activator.CreateInstance (HackStudioCode.Instance.GetTypesByReturnValue (VarType.Void) [i], true);
			} else
			{
				s = (StatementBase)Activator.CreateInstance (HackStudioCode.Instance.GetTypesByReturnValue (param.paramType) [i], true);
			}
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