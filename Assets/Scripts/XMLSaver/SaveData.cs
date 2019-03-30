using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class SaveData
{
	public List<int> canLoginUserIds = new List<int> ();
	public int lastUser = 0;

}