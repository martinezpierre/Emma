using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject LanguagesMenu;

	List<Sprite> originButtons;

	void Awake(){

		InitTrads ();
	}

	// Use this for initialization
	void Start () {
		
		originButtons = new List<Sprite> ();

		foreach (Button b in GameObject.FindObjectsOfType<Button> ()) {
			originButtons.Add (b.image.sprite);
		}

		TextManager.LoadText (EmmaMainScript.fileName);

		LocalizationManager.originTranslation = TextManager.texts.translations[0];
		TextManager.texts.currentTranslation = TextManager.texts.translations [0];

		if (Application.systemLanguage.Equals (SystemLanguage.French)) {
			Options.Instance.defaultLanguage = LocalizationManager.Languages.French;
		} else if (Application.systemLanguage.Equals (SystemLanguage.Italian)) {
			Options.Instance.defaultLanguage = LocalizationManager.Languages.Italian;
		} else if (Application.systemLanguage.Equals (SystemLanguage.German)) {
			Options.Instance.defaultLanguage = LocalizationManager.Languages.German;
		} else if (Application.systemLanguage.Equals (SystemLanguage.Spanish)) {
			Options.Instance.defaultLanguage = LocalizationManager.Languages.Spanish;
		} else {
			Options.Instance.defaultLanguage = LocalizationManager.Languages.English;
		}

		LocalizationManager.ChangeTrad (Options.Instance.defaultLanguage);

		TranslateMenu ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play()
	{
		StartCoroutine (Fade (1));

		AnalyticsManager.Instance.beginOver = true;
	}

	public void Resume()
	{
		StartCoroutine (Fade (1));

		TextManager.currentIndex = PlayerPrefs.GetInt ("currentIndex");
	}

	IEnumerator Fade(float duration)
	{
		GameObject gO = new GameObject ();
		Image img = gO.AddComponent<Image> ();
		img.color = new Color (0, 0, 0, 0);

		gO.transform.SetParent (transform);

		gO.GetComponent<RectTransform> ().anchorMax = Vector2.one;
		gO.GetComponent<RectTransform> ().anchorMin = Vector2.zero;

		while (img.color.a < 0.99f) 
		{
			img.color = new Color (img.color.r, img.color.g, img.color.b, img.color.a + 0.01f);

			yield return new WaitForSeconds (duration / 100f);
		}

		SceneManager.LoadScene ("Main");

		yield return null;
	}

	public void Languages()
	{

		LanguagesMenu.SetActive (true);

	}

	public void ToogleTrad(Toggle t)
	{
		if (t.isOn) {
			switch (t.name) {
			case "French":
				ChangeTrad (LocalizationManager.Languages.French);
				break;
			case "English":
				ChangeTrad (LocalizationManager.Languages.English);
				break;
			case "Spanish":
				ChangeTrad (LocalizationManager.Languages.Spanish);
				break;
			case "Italian":
				ChangeTrad (LocalizationManager.Languages.Italian);
				break;
			case "German":
				ChangeTrad (LocalizationManager.Languages.German);
				break;
			}
		}
	}

	public void ChangeTrad(LocalizationManager.Languages newTrad)
	{

		LocalizationManager.ChangeTrad (newTrad);

		TranslateMenu ();

		Options.Instance.defaultLanguage = newTrad;

		LanguagesMenu.SetActive (false);
	}

	public void Credits()
	{

	}

	public void Exit()
	{
		Application.Quit ();
	}

	public void TranslateMenu()
	{
		Button[] buttons = GameObject.FindObjectsOfType<Button> ();

		for (int i = 0; i < buttons.Length; i++) 
		{
			buttons[i].image.sprite = LocalizationManager.GetTranslatedImage (originButtons[i]);
		}
	}

	public void InitTrads()
	{
		foreach(LocalizationInfo lI in Resources.FindObjectsOfTypeAll (typeof(LocalizationInfo)))
		{
			if (!lI.ignoreThis) {
				lI.InitDico ();
			}
		}
	}
}
