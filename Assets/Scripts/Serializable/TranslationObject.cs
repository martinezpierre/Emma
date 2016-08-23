using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("TranslationObject")]
public class TranslationObject{

	[XmlAttribute("language")]
	public string language;

	[XmlArray("newImagesPaths")]
	[XmlArrayItem("string")]
	public List<string> newImagesPaths;

	[XmlArray("newTexts")]
	[XmlArrayItem("string")]
	public List<string> newTexts;

	[XmlIgnore]
	public List<Sprite> newImages;

	[XmlIgnore]
	public static string[] stringSeparators = new string[] {"Resources/"};

	public TranslationObject(){
		newImagesPaths = new List<string>();
		newTexts = new List<string>();
		newImages = new List<Sprite>();
	}

	public void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(TranslationObject));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public void Deserialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(TranslationObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as TranslationObject);
		stream.Close();
	}

	void copy(TranslationObject translationToCopy)
	{
		language = translationToCopy.language;
		newImagesPaths = translationToCopy.newImagesPaths;
		newTexts = translationToCopy.newTexts;
	}

	public void RecreateLinks()
	{
		for (int i = 0; i < newImagesPaths.Count; i++) 
		{
			string imagePath = newImagesPaths [i];

			string[] result;

			if (imagePath != "" && imagePath!=null) {
				result = imagePath.Split (stringSeparators, System.StringSplitOptions.None);
				result = result [1].Split ('.');

				newImages.Add((Sprite)Resources.Load (result [0], typeof(Sprite)));


			} 
		}
			
	}
}
