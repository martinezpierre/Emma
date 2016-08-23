using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour {

	static AnalyticsManager instance;

	public static AnalyticsManager Instance
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

	int beginIndex;

	[HideInInspector]public bool beginOver;

	public bool enableAnalytics = true;

	// Use this for initialization
	void Start () 
	{
		beginIndex = PlayerPrefs.GetInt ("currentIndex");

		beginOver = false;
	}

	void OnApplicationQuit()
	{
		if (beginOver) {
			beginIndex = 0;
		}

		if (enableAnalytics) {
			Debug.Log ("Analytics sent : ");
			Debug.Log ("current sentence : " + (TextManager.currentIndex + 1));
			Debug.Log ("number of sentences during this game : " + (TextManager.currentIndex - beginIndex));
			Debug.Log ("game finished : " + (TextManager.currentIndex + 1 == TextManager.texts.sentences.Count));

			Analytics.CustomEvent("gameOver", new Dictionary<string, object>
				{
					{ "current sentence", (TextManager.currentIndex+1) },
					{ "number of sentences during this game", (TextManager.currentIndex - beginIndex) },
					{ "game finished", (TextManager.currentIndex+1 == TextManager.texts.sentences.Count) }
				});
		}
	}
}
