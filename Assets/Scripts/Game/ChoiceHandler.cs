using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoiceHandler : MonoBehaviour {

	public GameObject choicesObject;

	public Button[] choicesButtons;

	Text[] choicesTexts;

	ChoiceObject[] choices;

	public Text characName;
	public Text sentence;

	int nbChoice;

	[HideInInspector]public bool choicesShown;

	EmmaMainScript eMS;

	// Use this for initialization
	void Start () 
	{
		eMS = GameObject.FindObjectOfType<EmmaMainScript> ();

		choices = new ChoiceObject[choicesButtons.Length];

		choicesTexts = new Text[choicesButtons.Length];

		for(int i = 0; i < choicesButtons.Length; i++)
		{
			choicesTexts [i] = choicesButtons [i].GetComponentInChildren<Text> ();
		}
	}

	public void DisplayChoices(SentenceObject sO)
	{
		nbChoice = 0;

		choicesShown = true;

		characName.text = "";
		sentence.text = "";

		foreach (ChoiceObject cO in sO.choices) 
		{
			choices [nbChoice] = cO;
			nbChoice++;
		}

		choicesObject.SetActive (true);

		for (int i = 0; i < sO.choices.Count; i++) 
		{
			choicesButtons [i].gameObject.SetActive (true);
			choicesTexts [i].text = choices [i].answer;
		}
		for (int i = sO.choices.Count; i < choicesButtons.Length; i++) 
		{
			choicesButtons [i].gameObject.SetActive (false);
		}
	}

	public void SelectChoice(int choiceIndex)
	{
		choicesShown = false;

		choicesObject.SetActive (false);

		int newCount = TextManager.texts.sentences.IndexOf (TextManager.GetSentence (choices [choiceIndex].nextSentenceIndex));

		eMS.SetCount (newCount);

		TextManager.currentChoice = choices [choiceIndex];
	}
}
