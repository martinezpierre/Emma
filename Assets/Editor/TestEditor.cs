using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class TestEditor : EditorWindow {

	public static bool canShow = false;

	public static string fileName = "french";

	Vector2 scrollPos;

	Color defaultColor;

	[MenuItem ("MyTools/Text Manager")]
	public static void  ShowWindow () {

		EditorWindow.GetWindow(typeof(TestEditor),false,"TextEditor");

		TextManager.texts = new TextObject ();
		TextManager.texts.sentences = new List<SentenceObject>();

		/*DirectoryInfo info = new DirectoryInfo("Assets/Resources/Texts");
		FileInfo[] fileInfo = info.GetFiles();

		foreach (FileInfo file in fileInfo) {
			if (!file.Name.Contains ("meta")) 
			{
				Debug.Log (file.Name);
			}
		}*/

		TextManager.LoadText (fileName);

		canShow = true;

	}

	bool b = true;

	void OnGUI(){

		defaultColor = GUI.color;

		if (b) 
		{
			b = false;
			/*maxSize = new Vector2(750f, 400f);
			minSize = maxSize;*/
		}

		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("Save Text"))
		{
			TextManager.SaveText(fileName);
		}
			
		if(GUILayout.Button("Load Text"))
		{
			TextManager.LoadText(fileName);
		}
		GUILayout.EndHorizontal ();

		if (!canShow) 
		{
			//return;
		}
			
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false,false);

		int currentIndex = 1;

		foreach(SentenceObject sO in TextManager.texts.sentences)
		{
			if (!sO.inaccessible) 
			{
				if (sO.index / 1000 != currentIndex) 
				{
					currentIndex = sO.index/1000;
					GUILayout.Space (20);
				}

				SentenceGUI (sO);
			}

			if (sO.choices.Count > 0) 
			{
				GUILayout.BeginHorizontal ();

				foreach (ChoiceObject cO in sO.choices) 
				{
					GUILayout.BeginVertical ();

					GUILayout.Label (cO.answer,EditorStyles.helpBox);

					int sequenceBegin = TextManager.texts.sentences.IndexOf (TextManager.GetSentence(cO.nextSentenceIndex));
					int sequenceEnd = TextManager.texts.sentences.IndexOf (TextManager.GetSentence(cO.endSequenceIndex));

					for (int i = sequenceBegin; i <= sequenceEnd; i++) 
					{
						SentenceGUI (TextManager.texts.sentences[i]);
					}

					GUILayout.EndVertical ();
				}

				GUILayout.EndHorizontal ();

			}
		}
		EditorGUILayout.EndScrollView();
	}

	void SentenceGUI(SentenceObject sO)
	{
		GUIStyle myStyle = new GUIStyle (EditorStyles.helpBox);



		GUILayout.BeginHorizontal ();
		GUILayout.Label (sO.index/1000 + "." +sO.index%1000 +".",GUILayout.Width ((sO.index/1000 + "." +sO.index%1000+"").Length*10+5), GUILayout.Height (20));

		sO.checkbox = EditorGUILayout.Toggle (sO.checkbox,GUILayout.Width(10));

		if (sO.isSMS) 
		{
			GUI.backgroundColor = TextManager.getCharacter (sO.characterName).color;
			myStyle.normal.textColor = Color.white;
		} 
		else 
		{
			GUI.backgroundColor = Color.black;
			myStyle.normal.textColor = TextManager.getCharacter (sO.characterName).color;
		}

		GUILayout.Label (sO.text,myStyle);

		GUI.backgroundColor = defaultColor;

		if(GUILayout.Button("+",GUILayout.Width (20), GUILayout.Height (20)))
		{
            TextManager.SaveText(fileName);
            DetailEditor.ShowWindow (fileName,sO);
		}
		GUILayout.Space(20);
		GUILayout.EndHorizontal ();
	}

}
