using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextManager{

	public static TextObject texts;

	public static int currentIndex;

	public static ChoiceObject currentChoice;

	public static List<string> availableLanguages;

	public static void SaveText(string name){

		SetAccessibility ();

		texts.Save (name+"save");
	}

	public static void LoadText(string name){
		texts = new TextObject ();
		texts.Load (name+"save");
		LoadTrueText(name);

		#if UNITY_EDITOR
		LoadLanguages ();
		#endif

	}

	public static void LoadLanguages()
	{
		availableLanguages = new List<string> ();

		DirectoryInfo info = new DirectoryInfo("Assets/Resources/Texts");
		FileInfo[] fileInfo = info.GetFiles();

		foreach (FileInfo file in fileInfo) {
			if (!file.Name.Contains ("meta") && file.Name.Contains(".txt")) 
			{
				availableLanguages.Add (file.Name.Replace(".txt",""));
			}
		}
	}

	static void LoadTrueText(string name)
	{
		List<SentenceObject> mySentenceList = new List<SentenceObject>();	

		string fileData; //= System.IO.File.ReadAllText(Application.dataPath+"/Resources/Texts/"+name+".txt");

		TextAsset temp = Resources.Load ("Texts/" + name) as TextAsset;
		fileData = temp.text;

		fileData = fileData.Replace("\n","");

		string[] groups = fileData.Split ("["[0]);

		foreach (string myGroup in groups) 
		{
			if (myGroup.Length > 0) {
				
				string[] splited = myGroup.Split ("]" [0]);

				string mySentence = splited[1];
				int index1 = int.Parse(splited [0]);

				string[] st = mySentence.Split ("<"[0]);
				foreach (string s in st) {

					if (s.Length > 0) {
						splited = s.Split (">" [0]);
						string news = splited[1];
						int index2 = int.Parse(splited [0]);

						SentenceObject sentence = new SentenceObject();

						sentence.index = index1 * 1000 + index2;
						sentence.text = news;

						mySentenceList.Add (sentence);

					}

				}

			}
		}

		if(texts.sentences.Count == 0)
			texts.sentences = new List<SentenceObject> ();

		for (int i = 0; i < mySentenceList.Count; i++) 
		{
			bool found = false;

			for(int j = 0; j < texts.sentences.Count; j++)
			{
				
				if (texts.sentences [j].index == mySentenceList [i].index) 
				{
					texts.sentences [j].text = mySentenceList [i].text;
					found = true;
				}
			}

			if (!found) {

				SentenceObject sO = new SentenceObject ();

				sO.text = mySentenceList [i].text;
				sO.index = mySentenceList [i].index;
				sO.keepPreviousImages = true;

				texts.sentences.Add (sO);
			}
		}

		for (int i = 0; i < texts.sentences.Count; i++) 
		{
			bool found = false;

			for(int j = 0; j < mySentenceList.Count; j++)
			{

				if (texts.sentences [i].index == mySentenceList [j].index) 
				{
					texts.sentences [i].text = mySentenceList [j].text;
					found = true;
				}
			}

			if (!found) {

				texts.sentences.Remove (texts.sentences [i]);
			}
		}

		texts.sentences.Sort(delegate(SentenceObject a, SentenceObject b) {
			return (a.index).CompareTo(b.index);
		});
	}

	public static List<ImageObject> GetSprites(SentenceObject sO)
	{
		List<ImageObject> sprites = new List<ImageObject>();

		foreach (ImageObject iO in sO.images) 
		{
			sprites.Add (iO);
		}

		return sprites;
	}

	public static SentenceObject[] GetText(LocalizationManager.Languages newTrad)
	{
		LocalizationManager.currentTrad = newTrad;

		SentenceObject[] myList = new SentenceObject[texts.sentences.Count];

		LoadTrueText(LocalizationManager.languagesNames [newTrad]);

		int i = 0;
		foreach (SentenceObject sO in texts.sentences) {
			myList[i] = sO;
			i++;
		}

		return myList;
	}

	public static SentenceObject GetSentence(int index)
	{
		SentenceObject res = new SentenceObject();

		foreach (SentenceObject sO in texts.sentences) 
		{
			if (sO.index == index) {
				res = sO;
			}
		}

		return res;
	}

	public static void SetAccessibility()
	{
		foreach (SentenceObject sO in texts.sentences) {
			sO.inaccessible = false;
		}

		foreach (SentenceObject sO in texts.sentences) {

			foreach (ChoiceObject cO in sO.choices) {

				SetInaccessible (true, cO.nextSentenceIndex, cO.endSequenceIndex);
			}

		}
	}

	public static void SetInaccessible(bool b, int indexFrom, int indexTo)
	{
		foreach (SentenceObject sO in texts.sentences) 
		{
			if (sO.index >= indexFrom && sO.index <= indexTo) {
				sO.inaccessible = b;
			}
		}
	}

	public static CharacterObject getCharacter(string characterName)
	{
		CharacterObject cO = texts.characters[0];

		foreach(CharacterObject c in texts.characters)
		{
			if (c.name == characterName) 
			{
				cO = c;
			}
		}

		return cO;
	}

	public static bool NextSentenceIsNewScene()
	{
		return texts.sentences [(currentIndex+1 >= texts.sentences.Count ) ? texts.sentences.Count-1 : currentIndex+1].fade;
	}
}
