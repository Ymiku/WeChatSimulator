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
		public InputField input;
        Parameter param;
		void OnEnable()
		{
			if (input != null) {
				input.onEndEdit.AddListener (OnInputEndEdit);
				input.onValueChanged.AddListener (OnInputValueChange);
			}
		}
		void OnDisable()
		{
			if (input != null) {
				input.onEndEdit.RemoveListener (OnInputEndEdit);
				input.onValueChanged.RemoveListener (OnInputValueChange);
			}
		}
		void OnInputValueChange(string s)
		{
			width = 2000.0f;
			width = input.textComponent.preferredWidth + 40.0f;
		}
		void OnInputEndEdit(string s)
		{
			switch (param.paramType) {
			case VarType.Bool:
				if (s.StartsWith ("false"))
					param.Set (false);
				else
					param.Set (true);
				break;
			case VarType.Int:
				param.Set (Convert.ToInt32 (s));
				break;
			default:
				param.Set (s);
				break;
			}
			width = 2000.0f;
			width = Mathf.Max(input.textComponent.preferredWidth + 40.0f,120.0f);
			//SetIDEData (param);
		}
        public void SetIDEData(Parameter o)
        {
            param = o;
			width = 8000.0f;
			//text.sizeDelta = new Vector2 (1000.0f,400.0f);
			text.text = param.GenerateCode();

			width = Mathf.Max(text.preferredWidth + 40.0f,80.0f);
            RefreshDropdown();
        }
        public void RefreshDropdown()
        {
            dropDown.Clear();
			List<System.Type> options = null;

			if (param.isVoid) {
				if (HackStudioCode.Instance.isDefine())
					options = HackStudioCode.Instance.GetValueTypes ();
				else
					options = HackStudioCode.Instance.GetTypesByReturnValue(VarType.Void);
			} else {
				options = HackStudioCode.Instance.GetTypesByReturnValue(param.paramType);
			}
            for (int i = 0; i < options.Count; i++)
            {
                dropDown.AddOption(options[i].Name.Substring(options[i].Name.IndexOf("Statement")+9));
            }
			if (!param.isVoid)
				dropDown.AddOption ("Value");
        }
		public void SetIDEData(string s)
		{
			param = Parameter.Empty;
			text.text = s;
			width = text.preferredWidth;
		}
        public void OnValueChange(int i)
        {
			input.gameObject.SetActive (i==-1);
			text.gameObject.SetActive (i!=-1);
			if (i == -1) {
				return;
			}
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
            //HackStudioCode.Instance.StepIn(id);
        }
		public void StepIn()
		{
			HackStudioCode.Instance.StepIn (id);
		}
    }
}