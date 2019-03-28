﻿using System;
using UIFrameWork;
using UnityEngine;

public static class Utils
{
    public static GameObject FindChild(GameObject parent, string path)
    {
        if (parent != null)
        {
            Transform trans = parent.transform.Find(path);
            if (trans)
                return trans.gameObject;
            else
            {
                Debug.LogError(string.Format("Utils.FindChild parent go {0} not exist child {1}", parent.name, path));
                return null;
            }
        }
        else
        {
            Debug.LogError("Utils.FindChild parent is null");
            return null;
        }
    }

    public static T FindInChild<T>(GameObject parent, string path) where T : Component
    {
        if (parent != null)
        {
            Transform trans = parent.transform.Find(path);
            if (trans)
                return trans.GetComponent<T>();
            else
            {
                Debug.LogError(string.Format("Utils.FindInChild parent go {0} not exist child {1}", parent.name, path));
                return null;
            }
        }
        else
        {
            Debug.LogError("Utils.FindInChild parent is null");
            return null;
        }
    }

    public static bool CheckIsSelfNumber(string number)
    {
        AccountSaveData data = XMLSaver.saveData.GetAccountData(GameManager.Instance.curUserId);
        return data.phoneNumber == number;
    }
    public static string FormatStringForInputField(string s,UnityEngine.UI.FInputField.FInputType type)
    {
        string output = s;
        switch (type)
        {
            case UnityEngine.UI.FInputField.FInputType.PhoneNumber:
                if (s.Length >=8)
                    output = output.Insert(7, " ");
                if(s.Length>=4)
                    output = output.Insert(3, " ");
                break;
            case UnityEngine.UI.FInputField.FInputType.CardNumber:
                break;
        }
        return output;
    }
}
