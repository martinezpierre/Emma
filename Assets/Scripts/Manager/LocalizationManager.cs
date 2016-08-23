using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LocalizationManager{

	public enum Languages
	{
		French,
		English,
		Spanish,
		Italian,
		German
	}

	public static Dictionary<Languages,string> languagesNames = new Dictionary<Languages,string> ()
	{
		{Languages.French,"french"},
		{Languages.English,"english"},
		{Languages.Spanish,"spanish"},
		{Languages.Italian,"italian"},
		{Languages.German,"german"},
	};

	public static Languages currentTrad;

	public static TranslationObject originTranslation;

	public static int GetImageIndex(Sprite s)
	{
		int res = -1;

		TranslationObject tO = originTranslation;

		for (int i = 0; i < tO.newImages.Count; i++) {
			if (tO.newImages [i] == s) {
				res = i;
			}
		}

		return res;
	}

	public static Sprite GetTranslatedImage(Sprite s)
	{
		Sprite res = s;

		int index = GetImageIndex (s);

		if (index != -1) {
			res = TextManager.texts.currentTranslation.newImages[index];
		}

		return res;
	}
		
	public static void ChangeTrad(Languages l)
	{
		string languageName = languagesNames [l];

		/*foreach (TranslationObject tO in TextManager.texts.translations) {
			if (tO.language == languageName) {
				TextManager.texts.currentTranslation = tO;
			}
		}*/

		foreach (Text t in Resources.FindObjectsOfTypeAll(typeof(Text))) 
		{
			LocalizationInfo lI = t.GetComponent<LocalizationInfo> ();

			if (lI && !lI.ignoreThis) 
			{
				//Debug.Log (t.name + " " + t.text);

				t.text = lI.traductionDico [languageName];
			}
		}

	}

}
