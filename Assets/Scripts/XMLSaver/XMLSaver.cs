using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml;
using System.IO;

public class XMLSaver : Singleton<XMLSaver> {
	// Use this for initialization
	public static SaveData saveData;
	public static void Save () {
		XmlSerializer serializer = new XmlSerializer (typeof(SaveData));
		#if UNITY_EDITOR
		FileStream stream = new FileStream (Application.dataPath + "/save.xml", FileMode.Create);
		#elif
		FileStream stream = new FileStream (Application.persistentDataPath + "/save.xml", FileMode.Create);
		#endif
		serializer.Serialize (stream, saveData);
		stream.Close ();
	}


	public static void Load () {
		if (File.Exists (Application.dataPath + "/save.xml")) {
			XmlSerializer serializer = new XmlSerializer (typeof(SaveData));
			#if UNITY_EDITOR
			FileStream stream = new FileStream (Application.dataPath + "/save.xml", FileMode.Open);
			#elif
			FileStream stream = new FileStream (Application.persistentDataPath + "/save.xml", FileMode.Open);
			#endif
			saveData = serializer.Deserialize (stream) as SaveData;
			stream.Close ();
		} else {
			saveData = new SaveData ();
		}
	}
}
