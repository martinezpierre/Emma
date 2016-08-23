using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[XmlRoot("ImageObject")]
public class ImageObject{

	[XmlAttribute("imagePath")]
	public string imagePath="";

	[XmlIgnore]
	public Sprite image;

	[XmlIgnore]
	public bool alreadyShown;

	[XmlIgnore]
	public GameObject gameObject;

	[XmlAttribute("isImage")]
	public bool isImage;

	[XmlAttribute("appearAfterSentence")]
	public bool appearAfterSentence;

	[XmlIgnore]
	public static string[] stringSeparators = new string[] {"Resources/"};

	[XmlAttribute("animDuration")]
	public float animDuration;

	[XmlAttribute("beginPosX")]
	public float beginPosX;
	[XmlAttribute("beginPosY")]
	public float beginPosY;

	[XmlAttribute("beginRot")]
	public float beginRot;

	[XmlAttribute("beginScaleX")]
	public float beginScaleX;
	[XmlAttribute("beginScaleY")]
	public float beginScaleY;

	[XmlAttribute("endPosX")]
	public float endPosX;
	[XmlAttribute("endPosY")]
	public float endPosY;

	[XmlAttribute("endRot")]
	public float endRot;

	[XmlAttribute("endScaleX")]
	public float endScaleX;
	[XmlAttribute("endScaleY")]
	public float endScaleY;

	[XmlAttribute("ease")]
	public bool ease = true;

    [XmlAttribute("easeOut")]
    public bool easeOut = true;

    [XmlIgnore]
	public Vector2 beginPos;
	[XmlIgnore]
	public Vector2 beginScale;

	[XmlIgnore]
	public Vector2 endPos;
	[XmlIgnore]
	public Vector2 endScale;

	public void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ImageObject));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public void Deserialize(string path)
	{

		XmlSerializer serializer = new XmlSerializer(typeof(ImageObject));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as ImageObject);
		stream.Close();
	}

	void copy(ImageObject imageToCopy)
	{
		imagePath = imageToCopy.imagePath;

		beginPosX = imageToCopy.beginPosX;
		beginPosY = imageToCopy.beginPosY;

		beginRot = imageToCopy.beginRot;

		beginScaleX = imageToCopy.beginScaleX;
		beginScaleY = imageToCopy.beginScaleY;

		endPosX = imageToCopy.endPosX;
		endPosY = imageToCopy.endPosY;

		endRot = imageToCopy.endRot;

		endScaleX = imageToCopy.endScaleX;
		endScaleY = imageToCopy.endScaleY;

		animDuration = imageToCopy.animDuration;

		appearAfterSentence = imageToCopy.appearAfterSentence;

		ease = imageToCopy.ease;
        easeOut = imageToCopy.easeOut;
	}

	public void RecreateLinks()
	{
		string[] result;

		if (imagePath != "") {
			result = imagePath.Split (stringSeparators, System.StringSplitOptions.None);
			result = result [1].Split ('.');

			if (isImage) {
				image = (Sprite)Resources.Load (result [0], typeof(Sprite));
			} else {
				gameObject = (GameObject)Resources.Load (result [0], typeof(GameObject));
			}


		} 

		beginPos = new Vector2 (beginPosX, beginPosY);
		beginScale = new Vector2 (beginScaleX, beginScaleY);

		endPos = new Vector2 (endPosX, endPosY);
		endScale = new Vector2 (endScaleX, endScaleY);
	}

}
