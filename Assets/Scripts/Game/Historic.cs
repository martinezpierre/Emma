using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Historic : MonoBehaviour {

	public GameObject historic;

	public Text[] texts;

	public Scrollbar scrollBar;

	[HideInInspector]public bool historicShown = false;

	void Update()
	{
		if (scrollBar.value == 0 && Input.GetAxis ("Mouse ScrollWheel") < 0) 
		{
			CloseHistoric ();
		}
	}
	
	public void ShowHistoric()
	{
		historicShown = true;

		historic.transform.SetParent(null);
		historic.transform.SetParent(transform);

		historic.SetActive (true);

		scrollBar.value = 0;

		for (int i = 0; i < texts.Length; i++) 
		{
			texts [i].text = "";
			texts [i].fontSize = Options.Instance.textSize;
		}
			
		int index = TextManager.currentIndex;

		for (int i = 0; i < texts.Length; i++) 
		{
			SentenceObject sO;

			do 
			{
				index--;

				if(index<0)
				{
					return;
				}

				sO = TextManager.texts.sentences [index];
			} 
			while(sO.isSMS);

			texts [i].text = sO.characterName + "\n" + sO.text;
			texts [i].color = TextManager.getCharacter (sO.characterName).color;

		}
	}

	public void CloseHistoric()
	{
		historicShown = false;

		historic.SetActive (false);
	}
}
