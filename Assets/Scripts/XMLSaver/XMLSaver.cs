using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml;
using System.IO;

public class XMLSaver : MonoBehaviour {
	// Use this for initialization
	SaveData s;
	SaveData b;
	public void Save (SaveData data) {
		XmlSerializer serializer = new XmlSerializer (typeof(SaveData));
		FileStream stream = new FileStream (Application.persistentDataPath + "/save.xml", FileMode.Create);
		serializer.Serialize (stream, data);
		stream.Close ();
	}


	public SaveData Load () {
		SaveData data = null;
		if (File.Exists (Application.dataPath + "/save.xml")) {
			XmlSerializer serializer = new XmlSerializer (typeof(SaveData));
			FileStream stream = new FileStream (Application.persistentDataPath + "/save.xml", FileMode.Open);
			data = serializer.Deserialize (stream) as SaveData;
			stream.Close ();
		} else {
			Debug.LogError ("Save Something First");
		}
		return data;
	}
}
