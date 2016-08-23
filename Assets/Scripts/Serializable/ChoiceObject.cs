using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[XmlRoot("ChoiceObject")]
public class ChoiceObject{

	[XmlAttribute("answer")]
	public string answer;

	[XmlAttribute("nextSentenceIndex")]
	public int nextSentenceIndex;

	[XmlAttribute("endSequenceIndex")]
	public int endSequenceIndex;

	public ChoiceObject()
	{
		nextSentenceIndex = 0;
		endSequenceIndex = 0;
	}

	public void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ChoiceObject));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public void Deserialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(ChoiceObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as ChoiceObject);
		stream.Close();
	}

	void copy(ChoiceObject choiceToCopy)
	{
		answer = choiceToCopy.answer;
		nextSentenceIndex = choiceToCopy.nextSentenceIndex;
		endSequenceIndex = choiceToCopy.endSequenceIndex;
	}

}
