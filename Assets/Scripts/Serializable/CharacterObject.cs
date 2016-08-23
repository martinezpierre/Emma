using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;

[XmlRoot("CharacterObject")]
public class CharacterObject{

	[XmlAttribute("name")]
	public string name;

	[XmlAttribute("colorR")]
	public float colorR;
	[XmlAttribute("colorG")]
	public float colorG;
	[XmlAttribute("colorB")]
	public float colorB;

	[XmlAttribute("isLeft")]
	public bool isLeft;

	[XmlIgnore]
	public Color color;

	public void Serialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(CharacterObject));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public void Deserialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(CharacterObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as CharacterObject);
		stream.Close();
	}

	void copy(CharacterObject characterToCopy)
	{
		colorR = characterToCopy.colorR;
		colorG = characterToCopy.colorG;
		colorB = characterToCopy.colorB;
		name = characterToCopy.name;
		isLeft = characterToCopy.isLeft;
	}

	public void RecreateLinks()
	{
		color = new Color (colorR, colorG, colorB);
	}
}

