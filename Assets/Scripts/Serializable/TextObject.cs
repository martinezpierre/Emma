using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("TextObject")]
public class TextObject{

	[XmlArray("Sentences")]
	[XmlArrayItem("SentenceObject")]
	public List<SentenceObject> sentences;

	[XmlArray("Characters")]
	[XmlArrayItem("CharacterObject")]
	public List<CharacterObject> characters;

	[XmlArray("translations")]
	[XmlArrayItem("TranslationObject")]
	public List<TranslationObject> translations;

	[/*XmlAttribute("currentTranslation")*/XmlIgnore]
	public TranslationObject currentTranslation;

	//#if UNITY_EDITOR

	public void Save(string name)
	{
		string path = Application.dataPath + "/Resources/Save/" + name + ".txt";

		XmlSerializer serializer = new XmlSerializer(typeof(TextObject));
		FileStream stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream, this);
		stream.Close();
		Debug.Log("saved at "+path);
	}

	public void Load(string name)
	{
		string path = Application.dataPath + "/Resources/Save/" + name + ".txt";

		XmlSerializer serializer = new XmlSerializer(typeof(TextObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as TextObject);
		stream.Close();

		RecreateLinks ();
	}

	//#else

	/*public void Load(string name)
	{
		TextAsset temp = Resources.Load("Save/" + name) as TextAsset;            
		XmlDocument _doc = new XmlDocument();
		_doc.LoadXml(temp.text);

		Debug.Log ("loader "+_doc.ChildNodes[1].ChildNodes.Count);

	}*/

	//#endif

	void copy(TextObject textToCopy)
	{
		sentences = textToCopy.sentences;
		characters = textToCopy.characters;
		translations = textToCopy.translations;
		currentTranslation = textToCopy.currentTranslation;
	}

	public void RecreateLinks()
	{
		foreach (SentenceObject sO in sentences) 
		{
			sO.RecreateLinks ();
		}
		foreach (CharacterObject cO in characters) 
		{
			cO.RecreateLinks ();
		}
		foreach (TranslationObject tO in translations) 
		{
			tO.RecreateLinks ();
		}
		if(currentTranslation != null)
			currentTranslation.RecreateLinks ();
	}
}
