using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CharacterEditor : EditorWindow {

	Vector2 scrollPos;

	public static string fileName = "french";

	static CharacterObject[] myList;

	static int nbCharact;

	[MenuItem ("MyTools/Character Manager")]
	public static void  ShowWindow () {

		myList = new CharacterObject[100];

		nbCharact = 0;

		LoadCharacters ();

		EditorWindow.GetWindow(typeof(CharacterEditor));

	}

	void OnEnable()
	{
		maxSize = new Vector2(350f, 400f);
		minSize = maxSize;
	}

	void OnGUI(){

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false,false);
		for(int i = 0; i < nbCharact; i++)
		{
			GUILayout.BeginHorizontal ();

			if (myList[i].name == null)
				myList[i].name = "";
			
			myList[i].name = GUILayout.TextField (myList[i].name);

			myList[i].color = EditorGUILayout.ColorField (myList[i].color);

			myList [i].isLeft = GUILayout.Toggle (myList [i].isLeft, "Name Left");

			GUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndScrollView();

		if(GUILayout.Button("New Character"))
		{
			nbCharact++;

			myList [nbCharact-1] = new CharacterObject ();
		}

		GUILayout.FlexibleSpace ();

		if(GUILayout.Button("Save"))
		{
			TextManager.texts.characters = new List<CharacterObject> ();

			for (int i = 0; i < nbCharact;i++) 
			{
				myList[i].colorR = myList[i].color.r;
				myList[i].colorG = myList[i].color.g;
				myList[i].colorB = myList[i].color.b;
				TextManager.texts.characters.Add (myList[i]);
			}

			TextManager.SaveText (fileName);
		}
	}

	static void LoadCharacters()
	{
		TextManager.LoadText (fileName);

		foreach (CharacterObject cO in TextManager.texts.characters) 
		{
			nbCharact++;

			myList [nbCharact - 1] = cO;
		}
	}

}
