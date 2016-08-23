using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class EmmaMainScript : MonoBehaviour {

	SentenceObject myText;

	GameObject gO;

	public static string fileName = "french";

	int count;

	SentenceObject[] myList;

	Button menuButton;

	TextWriter textWriter;
	Historic histo;
	PauseMenu pauseMenu;
	ImageHandler imageHandler;
	ChoiceHandler choiceHandler;
	SMSHandler sMSHandler;
	Fader fader;

	bool canClick;

	bool textDelay;

	public bool canShowHistoric;

	List<ImageObject> currentSprites;

	bool gameEnded = false;

	void Awake(){
		InitTrads ();

		canShowHistoric = true;

		canClick = true;

		textWriter = GameObject.FindObjectOfType<TextWriter> ();

		histo = GameObject.FindObjectOfType<Historic> ();

		pauseMenu = GameObject.FindObjectOfType<PauseMenu> ();

		imageHandler = GameObject.FindObjectOfType<ImageHandler> ();

		choiceHandler = GameObject.FindObjectOfType<ChoiceHandler> ();

		sMSHandler = GameObject.FindObjectOfType<SMSHandler> ();

		fader = GameObject.FindObjectOfType<Fader> ();

		menuButton = GameObject	.FindGameObjectWithTag ("MainCanvas").GetComponentInChildren<Button> ();

		TextManager.LoadText (fileName);
	}

	// Use this for initialization
	void Start () {

		LocalizationManager.originTranslation = TextManager.texts.translations[0];
		TextManager.texts.currentTranslation = TextManager.texts.translations [0];

		count = 0;//Options.Instance.beginingSentenceNumber-1;

		string[] res = Options.Instance.beginingSentenceNumber.Split ('.');

		int a, b;

		if (int.TryParse (res [0],out a) && int.TryParse (res [1], out b)) {
			int indexToReach = a * 1000 + b;
			int index = 0;
			foreach (SentenceObject s in TextManager.texts.sentences) 
			{
				if (s.index == indexToReach)
				{
					count = index;
				}
				index++;
			}
		}

		if (TextManager.currentIndex != 0) {
			count = TextManager.currentIndex;
			imageHandler.LoadPrevious ();
		}

		myList = TextManager.GetText (Options.Instance.defaultLanguage);

		LocalizationManager.ChangeTrad (Options.Instance.defaultLanguage);

		currentSprites = TextManager.GetSprites (myList [count]);

		TextManager.currentChoice = new ChoiceObject ();

		if (myList [0].fade) {
			StartCoroutine (BeginFade ());
		} else {
			UpdateGame ();
		}
	}

	IEnumerator BeginFade()
	{
		fader.StartFade (0, myList [0].fadeColor);

		//UpdateImage ();

		UpdateGame ();

		fader.StopFade (myList [count].fadeInDuration);

		while (fader.isFadingOut) {
			yield return new WaitForEndOfFrame ();
		}

		//UpdateText (true);
	}

	void Update()
	{
		if (textDelay || choiceHandler.choicesShown || gameEnded)
			return;

		bool input = false;

		foreach (KeyCode kC in Options.Instance.nextSentenceInput) 
		{
			if (Input.GetKeyDown (kC))
				input = true;
		}

		if (input && canClick) {
			
			if (histo.historicShown) 
			{
				histo.CloseHistoric ();
			}
			else if (!textWriter.isWriting) {

				if (myList [count].choices.Count > 0) 
				{
					choiceHandler.DisplayChoices (myList [count]);
					return;
				}

				if (!myList [count].isSMS) {
					currentSprites = imageHandler.CheckImagesAfterSentence (currentSprites);
				}

				if (imageHandler.canInterupt) 
				{
					UpdateIndex ();
				}

			} else {
				textWriter.FinishText ();
			}

		} 
		else if (Input.GetAxis ("Mouse ScrollWheel") > 0 && !histo.historicShown && canShowHistoric) {
			histo.ShowHistoric ();
		} 
		else {

			input = false;

			foreach (KeyCode kC in Options.Instance.openMenuInput) 
			{
				if (Input.GetKeyDown (kC))
					input = true;
			}
			if (input) 
			{
				if (pauseMenu.menuShown) 
				{
					ClosePauseMenu ();
				} 
				else 
				{
					ShowPauseMenu ();
				}
			}
		}
		
	}

	void UpdateIndex()
	{
		if (!(myList [count].images.Count > 0 && myList [count].images [0].appearAfterSentence && !myList [count].images [0].alreadyShown && myList [count].isSMS)) {
			int newIndex = count;

			do 
			{
				newIndex++;

				if(newIndex >= myList.Length){
					EndGame();
					break;
				}
			} 
			while (myList [newIndex].inaccessible && (myList [newIndex].index < TextManager.currentChoice.nextSentenceIndex || myList [newIndex].index > TextManager.currentChoice.endSequenceIndex));

			SetCount (newIndex);
		} else {
			sMSHandler.ShowSMS (myText,true);
		}

	}

	void EndGame()
	{
		gameEnded = true;
		SceneManager.LoadScene (0);
	}

	public void SetCount(int n)
	{
		if (Mathf.Abs (count - n) > 1) 
		{
			TextManager.currentIndex = n;
			imageHandler.LoadPrevious ();
		}

		count = Mathf.Clamp (n, 0, myList.Length - 1);

		currentSprites = TextManager.GetSprites (myList [count]);

		if (myList [count].fade) {
			StartCoroutine (WaitForFade (false));
		} else if (myList [count].crossfade) {
			StartCoroutine (WaitForFade (true));
		}else {
			UpdateGame ();
		}
	}

	IEnumerator WaitForFade(bool crossfade)
	{
		canClick = false;

		if (!crossfade) 
		{
			fader.StartFade (myList [count].fadeDuration, myList [count].fadeColor);

			while (fader.isFadingIn) {
				yield return new WaitForEndOfFrame ();
			}

			//UpdateImage ();

			UpdateGame ();

			canClick = true;

			fader.StopFade (myList [count].fadeInDuration);

			while (fader.isFadingOut) {
				yield return new WaitForEndOfFrame ();
			}
		}
		else 
		{
			fader.StartCrossfade (currentSprites,myList [count].fadeDuration);
            
            UpdateText(true);
            
            canClick = true;

            while (fader.isFadingOut) {
				yield return new WaitForEndOfFrame ();
			}


        }

		//UpdateText (true);


	}

	void UpdateGame()
	{
		if (!myList [count].isSMS) 
		{
			UpdateImage (myList[count].keepPreviousImages);
		}

		UpdateText (true);

		Transform t = fader.transform.parent;
		fader.transform.SetParent (null);
		fader.transform.SetParent (t);
	}

	void UpdateImage(bool keepPrevious)
	{
		imageHandler.UpdateImage (currentSprites,false,!keepPrevious);
	}

	void UpdateText(bool updateMusic)
	{
		myText = myList[count];
		TextManager.currentIndex = count;

		if (myText.isSMS) 
		{
			sMSHandler.ShowSMS (myText,false);
		} 
		else 
		{
			textWriter.WriteText (myText);
		}

		if (updateMusic) {
			SoundManager.Instance.HandleSounds (myText);
		}

		SetMenuButtonFront ();
	}

	public void SetMenuButtonFront(){
		Transform t = menuButton.transform.parent;
		menuButton.transform.SetParent (null);
		menuButton.transform.SetParent (t);
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
		myList = TextManager.GetText (newTrad);
		UpdateText (false);

		LocalizationManager.ChangeTrad (newTrad);

		Transform t = pauseMenu.menu.transform.parent;
		pauseMenu.menu.transform.SetParent (null);
		pauseMenu.menu.transform.SetParent (t);

		pauseMenu.TranslateMenu ();
	}

	public void SetTextDelay(bool b){
		textDelay = b;
	}

	public void SetCanClick(bool b){
		canClick = b;
	}

	public void SetCanShowHisto(bool b){
		canShowHistoric = b;
	}

	public void ShowPauseMenu()
	{
		pauseMenu.ShowMenu ();
		menuButton.gameObject.SetActive (false);
		canClick = false;
	}

	public void ClosePauseMenu()
	{
		pauseMenu.CloseMenu ();
		menuButton.gameObject.SetActive (true);
		canClick = true;
	}

	public void Exit()
	{
		Application.Quit ();
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt ("currentIndex",TextManager.currentIndex);
		//PlayerPrefs.SetString ("currentLanguage", LocalizationManager.languagesNames [LocalizationManager.currentTrad]);
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
