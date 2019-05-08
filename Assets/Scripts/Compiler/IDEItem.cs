using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Compiler
{
    public class IDEItem : ItemBase
    {
		public Dropdown dropDown;
        public TextProxy text;
        Parameter param;
        void Awake()
        {
            if (GetComponent<Dropdown>() != null)
                GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChange);
        }
        public void SetIDEData(Parameter o)
        {
            param = o;
			width = 1000.0f;
			//text.sizeDelta = new Vector2 (1000.0f,400.0f);
			text.text = param.GenerateCode();
            text.width = text.preferredWidth;
            width = text.width + 40.0f;
			dropDown.ClearOptions ();
			List<System.Type> options = HackStudioCode.Instance.GetTypesByReturnValue (param.paramType);
			List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();
			for (int i = 0; i < options.Count; i++) {
				Dropdown.OptionData d = new Dropdown.OptionData ();
				d.text = options [i].ToString ();
				//dropDown.AddOptions (options[i].ToString());
				optionData.Add(d);
			}
			dropDown.AddOptions (optionData);
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