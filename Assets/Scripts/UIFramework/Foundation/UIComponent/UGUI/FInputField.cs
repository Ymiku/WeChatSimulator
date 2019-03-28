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
            content = Utils.FormatStringForInputField(s,FInputType.PhoneNumber);
            UpdateFocusEffect();
        }
        int focusChangeRXId;
        int lineChangeRXId;
        protected override void OnEnable()
        {
            base.OnEnable();
            onValueChanged.AddListener(FOnValueChange);
            focusChangeRXId = FrostRX.Start(this).ExecuteWhen(() => { UpdateFocusEffect(); }, () => { return isFocused; }).
                ExecuteWhen(() => { UpdateFocusEffect(); }, () => { return !isFocused; }).GoToBegin().GetId();
            lineChangeRXId = FrostRX.Start(this).ExecuteAfterTime(() => { line = ""; if(isFocused)UpdateFocusEffect(); }, 1.0f).
                ExecuteAfterTime(() => { line = "."; if (isFocused) UpdateFocusEffect(); }, 1.0f).GoToBegin().GetId();

        }
        protected override void OnDisable()
        {
            base.OnDisable();
            onValueChanged.RemoveListener(FOnValueChange);
            FrostRX.Instance.EndRxById(focusChangeRXId);
            FrostRX.Instance.EndRxById(lineChangeRXId);
        }
        private void UpdateFocusEffect()
        {
            f_TextComponent.text = content+(isFocused?line:"");
        }
    }
}
