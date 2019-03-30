using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Text.RegularExpressions;
namespace UnityEngine.UI
{
    public class FInputField:InputField
    {
        [SerializeField]
        [FormerlySerializedAs("fInputType")]
        protected FInputType f_InputType;
        protected void FOnValueChange(string s)
        {
            if ((f_InputType & (FInputType.PhoneNumber | FInputType.CardNumber | FInputType.Money)) != 0)
            {
                if (f_InputType == FInputType.CardNumber || f_InputType == FInputType.PhoneNumber)
                    s = Regex.Match(s, "(\\d|\\s)*").Value;
                else if (f_InputType == FInputType.Money)  // todo 优化  正则表达式匹配金额
                {
					if (s.Contains (".")) {
						s = Regex.Match (s, "\\d*\\.\\d{0,2}").Value;
					} else {
						s = Regex.Match(s, "(\\d)*").Value;
					}
                }
            }
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
			if (caretPosition - 1 < 0||text.Length<caretPosition)
				return;
			if (text [caretPosition - 1].Equals(' ')) {
				caretPosition ++;
			}
		}
    }
}
