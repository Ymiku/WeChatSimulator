using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public void SetData(Parameter o)
        {
            param = o;
            text.text = param.ToString();
            text.width = text.preferredWidth;
            width = text.width + 40.0f;
			//dropDown.ClearOptions ();
			List<System.Type> options = HackStudioCode.Instance.GetTypesByReturnValue (param.paramType);
			for (int i = 0; i < options.Count; i++) {
				Dropdown.OptionData d = new Dropdown.OptionData ();
				d.text = options [i].ToString ();
				//dropDown.AddOptions (options[i].ToString());
			}

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
		public void StepIn()
		{
			GetComponentInParent<HackStudioCode> ().StepIn (id);
		}
    }
}