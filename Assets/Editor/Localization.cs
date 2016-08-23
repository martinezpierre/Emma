using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class Localization : EditorWindow {

	public static int nbImg;

	public static Sprite[] mySpriteListFr;
	public static Sprite[] mySpriteListEn;
	public static Sprite[] mySpriteListSp;
	public static Sprite[] mySpriteListIt;
	public static Sprite[] mySpriteListGe;

	[MenuItem ("MyTools/Images Translations Editor")]
	public static void  ShowWindow () {

		mySpriteListFr = new Sprite [100];
		mySpriteListEn = new Sprite [100];
		mySpriteListSp = new Sprite [100];
		mySpriteListIt = new Sprite [100];
		mySpriteListGe = new Sprite [100];

		TextManager.LoadText (TestEditor.fileName);

		nbImg = 0;

		Load ();

		Localization eD = (Localization)EditorWindow.GetWindow(typeof(Localization));
		eD.Init ();
	}

	void Init(){
		if (TextManager.texts.translations.Count == 0) {
			TranslationObject tO = new TranslationObject ();
			tO.language = "french";
			TextManager.texts.translations.Add (tO);

			TranslationObject tOEng = new TranslationObject ();
			tOEng.language = "english";
			TextManager.texts.translations.Add (tOEng);

			TranslationObject tOSpa = new TranslationObject ();
			tOSpa.language = "spanish";
			TextManager.texts.translations.Add (tOSpa);

			TranslationObject tOIta = new TranslationObject ();
			tOIta.language = "italian";
			TextManager.texts.translations.Add (tOIta);

			TranslationObject tOGer = new TranslationObject ();
			tOGer.language = "german";
			TextManager.texts.translations.Add (tOGer);
		}

		if (TextManager.texts.currentTranslation == null) {
			TextManager.texts.currentTranslation = TextManager.texts.translations[0];
		}
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal ();
		foreach (TranslationObject tO in TextManager.texts.translations) {
			GUILayout.BeginVertical ();

			GUILayout.Label (tO.language,EditorStyles.boldLabel);

			for (int i = 0; i < nbImg; i++) {
				switch (tO.language) {
				case "french":
					mySpriteListFr [i] = EditorGUILayout.ObjectField (mySpriteListFr [i], typeof(Sprite), true, GUILayout.Height (75), GUILayout.Width (75)) as Sprite;
					break;
				case "english":
					mySpriteListEn [i] = EditorGUILayout.ObjectField (mySpriteListEn [i], typeof(Sprite), true, GUILayout.Height (75), GUILayout.Width (75)) as Sprite;
					break;
				case "spanish":
					mySpriteListSp [i] = EditorGUILayout.ObjectField (mySpriteListSp [i], typeof(Sprite), true, GUILayout.Height (75), GUILayout.Width (75)) as Sprite;
					break;
				case "italian":
					mySpriteListIt [i] = EditorGUILayout.ObjectField (mySpriteListIt [i], typeof(Sprite), true, GUILayout.Height (75), GUILayout.Width (75)) as Sprite;
					break;
				case "german":
					mySpriteListGe [i] = EditorGUILayout.ObjectField (mySpriteListGe [i], typeof(Sprite), true, GUILayout.Height (75), GUILayout.Width (75)) as Sprite;
					break;
				}
			}

			if (tO.language == "french")
			{
				if (GUILayout.Button ("Add Image")) {
					AddImage ();
				}
			}

			GUILayout.EndVertical ();
		}
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Save")) {
			Save ();
		}
	}

	void AddImage(){
		//foreach (TranslationObject tO in TextManager.texts.translations) {
			nbImg++;
		//}
	}

	static void Save(){

		for (int i = 0; i < 5; i++) {

			TextManager.texts.translations [i].newImagesPaths = new List<string> ();

			switch (TextManager.texts.translations[i].language) {
			case "french":
				for (int j = 0; j < nbImg; j++) {

					TextManager.texts.translations[i].newImagesPaths.Add(AssetDatabase.GetAssetPath (mySpriteListFr[j]));

				}
				break;
			case "english":
				for (int j = 0; j < nbImg; j++) {

					TextManager.texts.translations[i].newImagesPaths.Add(AssetDatabase.GetAssetPath (mySpriteListEn[j]));

				}
				break;
			case "spanish":
				for (int j = 0; j < nbImg; j++) {

					TextManager.texts.translations[i].newImagesPaths.Add(AssetDatabase.GetAssetPath (mySpriteListSp[j]));

				}
				break;
			case "italian":
				for (int j = 0; j < nbImg; j++) {

					TextManager.texts.translations[i].newImagesPaths.Add(AssetDatabase.GetAssetPath (mySpriteListIt[j]));

				}
				break;
			case "german":
				for (int j = 0; j < nbImg; j++) {

					TextManager.texts.translations[i].newImagesPaths.Add(AssetDatabase.GetAssetPath (mySpriteListGe[j]));

				}
				break;
			}

		}
		TextManager.SaveText (TestEditor.fileName);
	}

	static void Load(){

		TextManager.LoadText (TestEditor.fileName);

		foreach (TranslationObject tO in TextManager.texts.translations) {

			int n =0;

			switch (tO.language) {
			case "french":
				foreach (Sprite s in tO.newImages) {
					mySpriteListFr [n] = s;
					nbImg++;
					n++;
				}
				break;
			case "english":
				foreach (Sprite s in tO.newImages) {
					mySpriteListEn [n] = s;
					n++;
				}
				break;
			case "spanish":
				foreach (Sprite s in tO.newImages) {
					mySpriteListSp [n] = s;
					n++;
				}
				break;
			case "italian":
				foreach (Sprite s in tO.newImages) {
					mySpriteListIt [n] = s;
					n++;
				}
				break;
			case "german":
				foreach (Sprite s in tO.newImages) {
					mySpriteListGe [n] = s;
					n++;
				}
				break;
			}
		}
	}
}
