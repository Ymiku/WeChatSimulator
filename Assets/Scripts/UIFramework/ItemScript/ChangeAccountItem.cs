using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
public class ChangeAccountItem : MonoBehaviour {
	public TextProxy text;
	public ImageProxy image;
    public ImageProxy check;
    int userID;
	public void SetUser(int userId)
	{
        AccountSaveData data = XMLSaver.saveData.GetAccountData(userId);
        text.text = Utils.FormatStringForSecrecy(data.phoneNumber,FInputType.PhoneNumber);
        image.sprite = data.GetHeadSprite();
        userID = userId;
        check.enabled = (userId == GameManager.Instance.curUserId);
	}
	public void OnClick()
	{
        if (userID == GameManager.Instance.curUserId)
            return;
        GameManager.Instance.SetUser(userID);
        UIManager.Instance.StartAndResetUILine(UIManager.UILine.Main);
        UIManager.Instance.Push(new HomeContext());
	}
}
