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
        public enum FInputType
        {
            None,
            PhoneNumber,
            CardNumber
        }
        [SerializeField]
        [FormerlySerializedAs("fInputType")]
        protected FInputType f_InputType;
        [SerializeField]
        [FormerlySerializedAs("f_text")]
        protected Text f_TextComponent;
        string content;
        string line = ".";
        protected void FOnValueChange(string s)
        {
			s = s.Replace (" ","");
			text = Utils.FormatStringForInputField(s,FInputType.PhoneNumber);
			//if (text [caretPosition - 1].Equals (" "))
			//	caretPosition = selectionFocusPosition = caretPositionInternal = caretSelectPositionInternal = caretPosition + 1;
			//UpdateLabel ();
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
		void Update()
		{
			if (caretPosition - 1 < 0)
				return;
			
			if (text [caretPosition - 1].Equals(' ')) {
				caretPosition ++;
			}
		}
    }
}
