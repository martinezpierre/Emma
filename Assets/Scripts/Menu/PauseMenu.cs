using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour {

	public GameObject menu;

	public Slider timeLine;

	[HideInInspector]public bool menuShown = false;

	List<Sprite> originButtons;

	// Use this for initialization
	void Start () {
		originButtons = new List<Sprite> ();

		foreach (Button b in GameObject.FindObjectsOfType<Button> ()) {
			originButtons.Add (b.image.sprite);
		}
	}

	public void ShowMenu()
	{
		menuShown = true;

		Time.timeScale = 0f;

		menu.transform.SetParent(null);
		menu.transform.SetParent(transform);

		menu.SetActive (true);

		TranslateMenu ();

		SetTimelinePosition ();
	}

	public void TranslateMenu()
	{
		/*Button[] buttons = GameObject.FindObjectsOfType<Button> ();

		for (int i = 0; i < buttons.Length; i++) 
		{
			buttons[i].image.sprite = LocalizationManager.GetTranslatedImage (originButtons[i]);
		}*/
	}

	public void CloseMenu()
	{
		Time.timeScale = 1f;

		menuShown = false;

		menu.SetActive (false);
	}

	public void SetTimelinePosition()
	{
		int storySize = TextManager.texts.sentences.Count;
		int currentPos = TextManager.currentIndex+1;

		float perc = (currentPos * 1.0f) / (storySize * 1.0f);

		timeLine.value = perc;
	}
}
