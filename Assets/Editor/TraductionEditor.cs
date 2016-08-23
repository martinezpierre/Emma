using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class TraductionEditor : EditorWindow {

	[MenuItem ("MyTools/Translations Editor")]
	public static void  ShowWindow () {

		TextManager.LoadText (TestEditor.fileName);

		TraductionEditor eD = (TraductionEditor)EditorWindow.GetWindow(typeof(TraductionEditor));
		eD.Init ();
	}

	List<Text> myTextList;

	Vector2 scrollPos;

	void Init(){

		myTextList = new List<Text> ();

		foreach (Text t in Resources.FindObjectsOfTypeAll(typeof(Text))) 
		{
			if (!EditorUtility.IsPersistent(t) && !(t.GetComponent<LocalizationInfo>() && t.GetComponent<LocalizationInfo>().ignoreThis)) 
			{
				myTextList.Add (t);
			}
		}

		Load ();

	}

	void OnGUI(){

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false,false);

		GUILayout.BeginHorizontal ();

		GUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Buttons",EditorStyles.boldLabel);

		foreach (Text t in myTextList) 
		{
			EditorGUILayout.LabelField (t.gameObject.name);
		}

		GUILayout.EndVertical ();

		foreach(string s in TextManager.availableLanguages) 
		{

			GUILayout.BeginVertical ();

			EditorGUILayout.LabelField (s,EditorStyles.boldLabel);

			EditorGUILayout.Space ();

			foreach (Text t in myTextList) 
			{
				LocalizationInfo lI = t.gameObject.GetComponent<LocalizationInfo> ();

				if (!lI.traductionDico.ContainsKey(s)) 
				{
					
					lI.traductionDico.Add(s,t.text);

				}

				lI.traductionDico[s] = EditorGUILayout.TextField (lI.traductionDico[s]);

			}

			GUILayout.EndVertical ();
		}

		GUILayout.EndHorizontal ();

		EditorGUILayout.EndScrollView ();

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Save")) {
			Save ();
		}

	}

	void Save()
	{
		foreach (Text t in myTextList) 
		{
			LocalizationInfo lI = t.gameObject.GetComponent<LocalizationInfo> ();

			lI.traductions = new List<LocalizationInfo.Traduction> ();

			foreach(KeyValuePair<string,string> trad in lI.traductionDico)
			{
				LocalizationInfo.Traduction myTrad = new LocalizationInfo.Traduction ();

				myTrad.language = trad.Key;
				myTrad.traduction = trad.Value;

				lI.traductions.Add (myTrad);
			}
		}

		GameObject go = GameObject.FindGameObjectWithTag ("MainCanvas");

		PrefabUtility.ReplacePrefab (go, PrefabUtility.GetPrefabParent (go), ReplacePrefabOptions.ConnectToPrefab);

	}

	void Load()
	{
		foreach (Text t in myTextList) 
		{
			LocalizationInfo lI = t.gameObject.GetComponent<LocalizationInfo> ();

			if (!lI) 
			{
				lI = t.gameObject.AddComponent<LocalizationInfo> ();
			}

			if (lI.traductionDico == null) {
				lI.traductionDico = new Dictionary<string, string>();
			}

			lI.InitDico ();
		}
	}

}
