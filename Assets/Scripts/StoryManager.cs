using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : Singleton<StoryManager> {
    // Update is called once per frame
    public static int addFriend = 0;
	public void Execute (int operateId = -1) {
        int main = XMLSaver.saveData.GetValue("main");
		if (main == 0) {
			if (XMLSaver.saveData.accountList.Count != 0 && GameManager.Instance.curUserId == 0) {
                ChatManager.Instance.AddFriend(XMLSaver.saveData.GetAccountData(1).enname);
                XMLSaver.saveData.SetValue("main",main+1);
			}
		} 
		else if (main == 1) {
		
		}
	}
}
