using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonActiveControlComponent : MonoBehaviour {

    private Button button;
    public InputField[] inputFields;
	// Use this for initialization
	
	// Update is called once per frame
	
    void Refresh(string s)
    {
        bool active = true;
        for (int i = 0; i < inputFields.Length; i++)
        {
            InputField f = inputFields[i];
            if (f is FInputField)
            {
                if (string.IsNullOrEmpty((f as FInputField).GetText()))
                {
                    active = false;
                    break;
                }
                continue;
            }
            if(string.IsNullOrEmpty(f.text))
            {
                active = false;
                break;
            }
        }
        button.interactable = active;
    }
    void OnEnable()
    {
        button = GetComponent<Button>();
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].onValueChanged.AddListener(Refresh);
        }
        Refresh("");
    }
    void OnDisable()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].onValueChanged.RemoveListener(Refresh);
        }
    }
}
