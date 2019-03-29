using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    public class FInputField:InputField
    {
        [SerializeField]
        [FormerlySerializedAs("fInputType")]
        protected FInputType f_InputType;
        protected void FOnValueChange(string s)
        {
			s = s.Replace (" ","");
			text = Utils.FormatStringForInputField(s,f_InputType);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            onValueChanged.AddListener(FOnValueChange);
		
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            onValueChanged.RemoveListener(FOnValueChange);
        }
		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			if (caretPosition - 1 < 0)
				return;
			Debug.Log (text);
			if (text [caretPosition - 1].Equals(' ')) {
				caretPosition ++;
			}
		}
    }
}
