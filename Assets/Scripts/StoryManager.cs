using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : Singleton<StoryManager> {
    // Update is called once per frame
    public static int addFriend = 0;
	public void Execute (int operateId = -1) {
        int main = XMLSaver.saveData.varValue[0];
        if (main == 0)
        {
            if (XMLSaver.saveData.accountList.Count != 0&&GameManager.Instance.curUserId==0)
            {
                XMLSaver.saveData.AddAccountData(1);
                main++;
            }
        }
	}
}
