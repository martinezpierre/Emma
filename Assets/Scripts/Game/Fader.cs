using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Fader : MonoBehaviour {

	public bool isFadingIn = false;
	public bool isFadingOut = false;

	Image image;

	GameObject canvas;

	ImageHandler iH;
	EmmaMainScript eMS;
	TextWriter tW;

	void Start()
	{
		iH = GameObject.FindObjectOfType<ImageHandler> ();

		eMS = GameObject.FindObjectOfType<EmmaMainScript> ();

		tW = GameObject.FindObjectOfType<TextWriter> ();

		image = GetComponent<Image> ();

		canvas = GameObject.FindGameObjectWithTag ("MainCanvas");
	}

	public void StartFade(float duration, Color color)
	{
		isFadingIn = true;

		image.color = color;

		transform.SetParent (null);
		transform.SetParent (canvas.transform);

		if (duration > 0) {
			image.color = new Color (image.color.r, image.color.g, image.color.b, 0);

		}

		StartCoroutine (Fade (true,duration));
	}

	IEnumerator Fade(bool fadeIn, float duration)
	{
		if (fadeIn) {
			while (image.color.a < 0.99f) {
				image.color = new Color (image.color.r, image.color.g, image.color.b, image.color.a + 0.01f);

				yield return new WaitForSeconds (duration / 100f);
			}
			isFadingIn = false;
		} else {
			while (image.color.a > 0.01f) {
				image.color = new Color (image.color.r, image.color.g, image.color.b, image.color.a - 0.01f);

				yield return new WaitForSeconds (duration / 100f);
			}
			isFadingOut = false;
		}

		yield return null;
	}

	public void StopFade(float duration)
	{
		isFadingOut = true;

		transform.SetParent (null);
		transform.SetParent (canvas.transform);

		StartCoroutine (Fade (false,duration));
	}


	public void StartCrossfade(List<ImageObject> after, float duration)
	{
		isFadingOut = true;

		StartCoroutine (Crossfade (after, duration));
	}

	IEnumerator Crossfade(List<ImageObject> toDisplay, float duration)
	{
		List<Image> before = new List<Image> ();
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Image")) 
		{
			before.Add (go.GetComponent<Image> ());
		}

		List<Image> after = new List<Image> ();

		iH.DisplayImages (toDisplay, false);

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Image")) 
		{
			after.Add (go.GetComponent<Image> ());
		}
		foreach (Image i in before) {
			Transform t = i.transform.parent;
			i.transform.SetParent (null);
			i.transform.SetParent (t);
			after.Remove (i);
		}

		eMS.SetMenuButtonFront ();
		tW.SetTextFront ();

		while (before [0].color.a > 0) {

			foreach (Image image in before) {
				image.color = new Color (image.color.r, image.color.g, image.color.b, image.color.a - 0.01f);
			}

			yield return new WaitForSeconds (duration / 100f);
		}

		isFadingOut = false;

		foreach (Image i in before) {
			Destroy (i.gameObject);
		}

		yield return null;
	}
}
