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
    public class FInputField : InputField
    {
        [SerializeField]
        [FormerlySerializedAs("fInputType")]
        protected FInputType f_InputType;
        public string GetText()
        {
            return text.Replace(" ","");
        }
        protected void FOnValueChange(string s)
        {
            if ((f_InputType & (FInputType.PhoneNumber | FInputType.CardNumber | FInputType.Money)) != 0)
            {
                switch (f_InputType)
                {
                    case FInputType.PhoneNumber:
                        s = Regex.Match(s, "(\\d|\\s)*").Value;
                        break;
                    case FInputType.CardNumber:
                        if (s.Length > GameDefine.cardIdMaxLength + 4)
                            s = s.Substring(0, GameDefine.cardIdMaxLength + 4);
                        else
                            s = Regex.Match(s, "(\\d|\\s)*").Value;
                        break;
                    case FInputType.Money:
                        if (s.Contains("."))
                        {
                            s = Regex.Match(s, "\\d*\\.\\d{0,2}").Value;
                        }
                        else
                        {
                            s = Regex.Match(s, "(\\d)*").Value;
                        }
                        break;
                }
            }
            s = s.Replace(" ", "");
            text = Utils.FormatStringForInputField(s, f_InputType);
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
        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (caretPosition - 1 < 0 || text.Length < caretPosition)
                return;
            if (text[caretPosition - 1].Equals(' '))
            {
                caretPosition++;
            }
        }
    }
}
