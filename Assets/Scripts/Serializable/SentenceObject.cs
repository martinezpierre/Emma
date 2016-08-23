using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[XmlRoot("SentenceObject")]
public class SentenceObject{

	[XmlAttribute("index")]
	public int index;

	[XmlAttribute("checkbox")]
	public bool checkbox;

	[XmlAttribute("text")]
	public string text;

	[XmlAttribute("fade")]
	public bool fade;

	[XmlAttribute("fadeDuration")]
	public float fadeDuration;

	[XmlAttribute("fadeInDuration")]
	public float fadeInDuration;

	[XmlAttribute("fadeColorR")]
	public float fadeColorR;
	[XmlAttribute("fadeColorG")]
	public float fadeColorG;
	[XmlAttribute("fadeColorB")]
	public float fadeColorB;
	[XmlIgnore]
	public Color fadeColor;

	[XmlAttribute("crossfade")]
	public bool crossfade;

	[XmlAttribute("keepPreviousImages")]
	public bool keepPreviousImages;

	[XmlArray("Images")]
	[XmlArrayItem("ImageObject")]
	public List<ImageObject> images;

	[XmlArray("Sounds")]
	[XmlArrayItem("SoundObject")]
	public List<SoundObject> sounds;

	[XmlArray("Themes")]
	[XmlArrayItem("SoundObject")]
	public List<SoundObject> themes;

    [XmlAttribute("stopTheme")]
    public bool stopTheme;

    [XmlAttribute("characterName")]
	public string characterName;

	[XmlAttribute("delayBeforeAppear")]
	public float delayBeforeAppear;

	[XmlAttribute("inaccessible")]
	public bool inaccessible;

	[XmlArray("Choices")]
	[XmlArrayItem("ChoiceObject")]
	public List<ChoiceObject> choices;

	[XmlAttribute("isSMS")]
	public bool isSMS;

	public enum Transitions{
		Simple,
		Fade,
		Crossfade
	}

	[XmlIgnore]
	public Transitions transitionType;

	public SentenceObject()
	{
		images = new List<ImageObject> ();

		sounds = new List<SoundObject> ();

		themes = new List<SoundObject> ();

		choices = new List<ChoiceObject> ();

		transitionType = Transitions.Simple;
	}

	public void Serialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(SentenceObject));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public void Deserialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(SentenceObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as SentenceObject);
		stream.Close();
	}

	void copy(SentenceObject sentenceToCopy)
	{
		index = sentenceToCopy.index;
		text = sentenceToCopy.text;
		images = sentenceToCopy.images;

		fade = sentenceToCopy.fade;
		crossfade = sentenceToCopy.crossfade;
		keepPreviousImages = sentenceToCopy.keepPreviousImages;
		characterName = sentenceToCopy.characterName;
		fadeDuration = sentenceToCopy.fadeDuration;
		fadeInDuration = sentenceToCopy.fadeInDuration;

		fadeColorR = sentenceToCopy.fadeColorR;
		fadeColorG = sentenceToCopy.fadeColorG;
		fadeColorB = sentenceToCopy.fadeColorB;

		delayBeforeAppear = sentenceToCopy.delayBeforeAppear;

		sounds = sentenceToCopy.sounds;
		themes = sentenceToCopy.themes;

		inaccessible = sentenceToCopy.inaccessible;
		choices = sentenceToCopy.choices;
		isSMS = sentenceToCopy.isSMS;

		checkbox = sentenceToCopy.checkbox;

        stopTheme = sentenceToCopy.stopTheme;
    }

	public void RecreateLinks()
	{
		foreach (ImageObject iO in images) 
		{
			iO.RecreateLinks ();
		}
		foreach (SoundObject sO in sounds) 
		{
			sO.RecreateLinks ();
		}
		foreach (SoundObject sO in themes) 
		{
			sO.RecreateLinks ();
		}

		if (fade) 
		{
			transitionType = Transitions.Fade;
		}
		else if (crossfade)
		{
			transitionType = Transitions.Crossfade;
		} 
		else 
		{
			transitionType = Transitions.Simple;
		}

		fadeColor = new Color (fadeColorR, fadeColorG, fadeColorB);
	}
}
