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
        protected void FOnValueChange(string s)
        {

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
    }
}
