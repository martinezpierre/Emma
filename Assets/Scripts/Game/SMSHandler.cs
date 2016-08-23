using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SMSHandler : MonoBehaviour {

	public VerticalLayoutGroup SMSBoxParent;

	RectTransform sliderContent;

	public RectTransform slider;

	public GameObject SMSPrefab;

	SentenceObject lastSMS;

	int nbLeft = 0;
	int nbRight = 0;

	int SMSpadding;

	bool right = true;

	// Use this for initialization
	void Start () {
		SMSpadding = (int)(SMSBoxParent.GetComponent<RectTransform> ().rect.width / 2);
			
		sliderContent = (RectTransform)SMSBoxParent.transform.parent;

		sliderContent.offsetMin = new Vector2 (sliderContent.offsetMin.x, 0);
	}

	public void ShowSMS(SentenceObject sO, bool showAfterSentence)
	{

        SMSBoxParent.gameObject.SetActive(true);

		GameObject gO = Instantiate (SMSPrefab) as GameObject;;

		if (sO.images.Count > 0 && (!sO.images [0].appearAfterSentence || showAfterSentence)) {
			gO.GetComponent<SMS> ().image.sprite = sO.images [0].image;
		} 
		else 
		{
			gO.GetComponent<SMS> ().imageParent.SetActive (false);
			gO.GetComponent<SMS>().spaceAdder.gameObject.SetActive (false);
		}

		gO.GetComponent<SMS> ().message.text = sO.text;

		if (showAfterSentence) {
			gO.GetComponent<SMS> ().message.gameObject.SetActive (false);
			sO.images [0].alreadyShown = true;
		}

		if (lastSMS==null || sO.characterName != lastSMS.characterName)
			right = !right;

		if (right) 
		{
			gO.GetComponent<SMS> ().layout.padding.left = SMSpadding;
			nbRight++;

		} 
		else 
		{
			gO.GetComponent<SMS> ().layout.padding.right = SMSpadding;
			nbLeft++;
		}
			
		gO.GetComponent<SMS> ().background.color = TextManager.getCharacter (sO.characterName).color;

		gO.GetComponent<SMS> ().characterName.text = sO.characterName;

		gO.transform.SetParent (SMSBoxParent.transform);

		slider.GetComponent<ScrollRect> ().verticalScrollbar.value = 0;

		Transform t = slider.parent;
		slider.SetParent (null);
		slider.SetParent (t);

		if (nbLeft + nbRight > 1) 
		{
			StartCoroutine (ResizeContent (gO.GetComponent<RectTransform>()));
		}

		lastSMS = sO;

	}

	IEnumerator ResizeContent(RectTransform rT)
	{
		yield return new WaitForEndOfFrame ();

		sliderContent.offsetMax = new Vector2 (sliderContent.offsetMax.x, sliderContent.offsetMax.y+rT.rect.height);
	}
}
