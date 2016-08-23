using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextWriter : MonoBehaviour {

	float letterPause;

	public GameObject parent;
	public Text characterName;
	public Text text;

	public GameObject canvas;

	public Image imageSentence;
	public Image imageScene;

	[HideInInspector] public bool isWriting;

	string currentMessage;
	CharacterObject currentCharacter;

	EmmaMainScript eMS;

	int policeSize;

	// Use this for initialization
	void Awake () {

		eMS = GameObject.FindObjectOfType<EmmaMainScript> ();

		imageSentence.enabled = false;
		imageScene.enabled = false;

		letterPause = Options.Instance.textSpeed;

		currentMessage = "";

		characterName.text = "";

		isWriting = false;

		//ResizeText ();
	
	}

	void ResizeText()
	{
		if (Screen.height + Screen.width > 3000) {
			Options.Instance.textSize = Options.Instance.textSize * 2;
			text.GetComponent<RectTransform> ().localScale = new Vector3 (.5f, .5f, 5f);
			text.GetComponent<RectTransform> ().sizeDelta = text.GetComponent<RectTransform> ().sizeDelta * 2;
		} else if (Screen.height + Screen.width > 2000) {
			Options.Instance.textSize = (int)(Options.Instance.textSize * 1.5);
			text.GetComponent<RectTransform> ().localScale = new Vector3 (.6f, .6f, 6f); 
			text.GetComponent<RectTransform> ().sizeDelta = text.GetComponent<RectTransform> ().sizeDelta * 1.5f;
		} else if (Screen.height + Screen.width > 1000) {
			Options.Instance.textSize = Options.Instance.textSize * 1;
			text.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f); 
			text.GetComponent<RectTransform> ().sizeDelta = text.GetComponent<RectTransform> ().sizeDelta * 1;
		} else {
			Options.Instance.textSize = (int)(Options.Instance.textSize * 0.5);
			text.GetComponent<RectTransform> ().localScale = new Vector3 (2f, 2f, 2f); 
			text.GetComponent<RectTransform> ().sizeDelta = text.GetComponent<RectTransform> ().sizeDelta * 0.5f;
		}
	}

	public void WriteText(SentenceObject message)
	{
		StopAllCoroutines ();

		currentMessage = message.text;

		currentCharacter = TextManager.getCharacter (message.characterName);

		characterName.color = currentCharacter.color;
		characterName.text = "";
		characterName.fontSize = Options.Instance.nameSize;

		if (currentCharacter.isLeft) {
			characterName.alignment = TextAnchor.MiddleLeft;
		} else {
			characterName.alignment = TextAnchor.MiddleRight;
		}

		/*parent.transform.SetParent(null);
		parent.transform.SetParent(canvas.transform);*/

		text.color = currentCharacter.color;

		imageSentence.color = currentCharacter.color;
		imageScene.color = currentCharacter.color;

		isWriting = true;

		text.fontSize = Options.Instance.textSize;

		SetTextFront();

		StartCoroutine(TypeText (currentMessage,message.delayBeforeAppear));
	}

	public void SetTextFront(){
		text.text = "";
        characterName.text = "";
        parent.transform.SetParent(null);
		parent.transform.SetParent(canvas.transform);
		imageSentence.enabled = false;
		imageScene.enabled = false;
	}

	public void FinishText()
	{
		StopAllCoroutines ();
		isWriting = false;
		text.text = currentMessage;

		if (TextManager.NextSentenceIsNewScene ()) {
			imageScene.enabled = true;
		} else {
			imageSentence.enabled = true;
		}
	}

	IEnumerator TypeText (string message, float delay) 
	{
		float timer = Time.time;

		eMS.SetTextDelay (true);

		while (Time.time - timer < delay) {
			yield return null;
		}

		eMS.SetTextDelay (false);

		characterName.text = currentCharacter.name;

		foreach (char letter in message.ToCharArray()) 
		{
			text.text += letter;

			/*if (sound)
				GetComponent<AudioSource>().PlayOneShot (sound);*/

			if (letter > 'A' && letter < 'z') 
			{
				yield return new WaitForSeconds (letterPause);
			}
		}   

		yield return 0;

		isWriting = false;

		if (TextManager.NextSentenceIsNewScene ()) {
			imageScene.enabled = true;
		} else {
			imageSentence.enabled = true;
		}
	}
}
