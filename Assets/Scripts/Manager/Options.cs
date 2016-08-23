using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Options : MonoBehaviour {

	static Options instance;

	public static Options Instance
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		instance = this;

		DontDestroyOnLoad(transform.gameObject);
	}

	[HideInInspector]
	public LocalizationManager.Languages defaultLanguage = LocalizationManager.Languages.French;

	[Header("Text")]

	public float textSpeed = 0.02f;
	public int nameSize = 50;
	public int textSize = 40;

	[Header("Controls")]

	public List<KeyCode> nextSentenceInput;

	public List<KeyCode> openMenuInput;

	[Header("Debug")]

	public string beginingSentenceNumber = "1.1";

}
