using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizationInfo : MonoBehaviour
{
	[System.Serializable]
	public struct Traduction
	{
		public string language;
		public string traduction;
	}

	public List<Traduction> traductions;

	public bool ignoreThis;

	public Dictionary<string,string> traductionDico;

	void Awake()
	{
		InitDico ();
	}

	public void InitDico()
	{
		traductionDico = new Dictionary<string,string> ();

		for (int i = 0; i < traductions.Count; i++) 
		{
			traductionDico [traductions [i].language] = traductions [i].traduction;
		}
	}
}
