using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[XmlRoot("SoundObject")]
public class SoundObject{

	[XmlAttribute("soundPath")]
	public string soundPath="";

	[XmlAttribute("soundName")]
	public string soundName="";

	[XmlIgnore]
	public AudioClip sound;

	[XmlIgnore]
	public static string[] stringSeparators = new string[] {"Resources/"};

	[XmlAttribute("delayBeforeAppear")]
	public float delayBeforeAppear;

	[XmlAttribute("fadeInDuration")]
	public float fadeInDuration;

	[XmlAttribute("fadeOutDuration")]
	public float fadeOutDuration;

	[XmlAttribute("loop")]
	public bool loop;

	public void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(SoundObject));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public void Deserialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(SoundObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as SoundObject);
		stream.Close();
	}

	void copy(SoundObject soundToCopy)
	{
		soundPath = soundToCopy.soundPath;
		soundName = soundToCopy.soundName;
		delayBeforeAppear = soundToCopy.delayBeforeAppear;
		fadeInDuration = soundToCopy.fadeInDuration;
		fadeOutDuration = soundToCopy.fadeOutDuration;
		loop = soundToCopy.loop;
	}

	public void RecreateLinks()
	{
		string[] result;

		if (soundPath != "") {
			result = soundPath.Split (stringSeparators, System.StringSplitOptions.None);
			result = result [1].Split ('.');

			sound = (AudioClip)Resources.Load (result [0], typeof(AudioClip));


		}
	}

}
