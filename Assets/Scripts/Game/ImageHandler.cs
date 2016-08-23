using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageHandler : MonoBehaviour {

	public GameObject imagePrefab;

	public GameObject imageParent;

	[HideInInspector]public bool canInterupt = true;

	public TextWriter tW;

    public AnimationCurve curve;

    SMSHandler smsh;

    List<GameObject> goList = new List<GameObject>();

	void Start()
    {
        tW = GameObject.FindObjectOfType<TextWriter> ();
        smsh = GameObject.FindObjectOfType<SMSHandler>();
    }

	public void LoadPrevious()
	{
		int indexToLoad = TextManager.currentIndex;

		while (TextManager.GetSprites (TextManager.texts.sentences[indexToLoad]).Count == 0) {
			indexToLoad--;
		}

		UpdateImage (TextManager.GetSprites (TextManager.texts.sentences[indexToLoad]),true, TextManager.texts.sentences[indexToLoad].keepPreviousImages);
	}

	public List<ImageObject> CheckImagesAfterSentence(List<ImageObject> myList)
	{
		List<ImageObject> toReturn = myList;

		foreach (ImageObject iO in myList) {
			if (iO.appearAfterSentence) {

				GameObject gO;

				if (iO.isImage) {
					gO = Instantiate (imagePrefab);
					gO.transform.SetParent (imageParent.transform, false);
					gO.GetComponent<Image> ().sprite = LocalizationManager.GetTranslatedImage(iO.image);
				} else {
					gO = Instantiate (iO.gameObject);
                    goList.Add(gO);
				}
                
				StartCoroutine (ImageAnimation (gO, iO, false));

				toReturn.Remove (iO);

				break;
			}
		}

		return toReturn;

	}
		
	public void UpdateImage(List<ImageObject> myList, bool loadAfterSentence, bool destroyPrevious)
	{
        
        if (destroyPrevious)
        {
            DestroyImages(true, true);
            smsh.SMSBoxParent.gameObject.SetActive(false);
        }
		
		DisplayImages(myList, loadAfterSentence);

		tW.SetTextFront ();
	}

	public void DestroyImages(bool destroyImages, bool destroyParticles)
	{
		StopAllCoroutines ();

		if (destroyImages) {
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Image")) 
			{
				Destroy (go);
			}
		}
		if (destroyParticles) {
			foreach (GameObject go in goList) 
			{
                Debug.Log(go.name);
				Destroy (go.gameObject);
			}
            goList.Clear();
		}
	}

	public void DisplayImages(List<ImageObject> myList, bool loadAfterSentence){
		foreach (ImageObject iO in myList) 
		{
			if (!iO.appearAfterSentence || loadAfterSentence) 
			{
				GameObject gO;

				if (iO.isImage) {
					gO = Instantiate (imagePrefab);
                    

                    gO.transform.SetParent (imageParent.transform, false);
					gO.GetComponent<Image> ().sprite = LocalizationManager.GetTranslatedImage(iO.image);
				} else {
					gO = Instantiate (iO.gameObject);
                    goList.Add(gO);
				}

				StartCoroutine (ImageAnimation (gO, iO, true));
			}
		}
	}

	//for UI images
	IEnumerator ImageAnimation(GameObject image, ImageObject iO, bool canInterup)
	{
		AnimationCurve curvePosX;
		AnimationCurve curvePosY;
		AnimationCurve curveRot;
		AnimationCurve curveScaleX;
		AnimationCurve curveScaleY;

        float tan45 = Mathf.Tan(Mathf.Deg2Rad * 45);
        
        curvePosX = new AnimationCurve();
        curvePosY = new AnimationCurve();
        curveRot = new AnimationCurve();
        curveScaleX = new AnimationCurve();
        curveScaleY = new AnimationCurve();
        
        AnimationCurve curvePosXref = AnimationCurve.Linear(0, iO.beginPosX, iO.animDuration, iO.endPosX);
        AnimationCurve curvePosYref = AnimationCurve.Linear(0, iO.beginPosY, iO.animDuration, iO.endPosY);
        AnimationCurve curveRotref = AnimationCurve.Linear(0, iO.beginRot, iO.animDuration, iO.beginRot);
        AnimationCurve curveScaleXref = AnimationCurve.Linear(0, iO.beginScaleX, iO.animDuration, iO.endScaleX);
        AnimationCurve curveScaleYref = AnimationCurve.Linear(0, iO.beginScaleY, iO.animDuration, iO.endScaleY);

        if (iO.ease)
		{
            curvePosX.AddKey(new Keyframe(0, iO.beginPosX,0,0));// = AnimationCurve.EaseInOut (0, iO.beginPosX, iO.animDuration, iO.endPosX);
			curvePosY.AddKey(new Keyframe(0, iO.beginPosY, 0, 0)); //= AnimationCurve.EaseInOut (0, iO.beginPosY, iO.animDuration, iO.endPosY);
			curveRot.AddKey(new Keyframe(0, iO.beginRot, 0, 0)); //= AnimationCurve.EaseInOut (0, iO.beginRot, iO.animDuration, iO.beginRot);
			curveScaleX.AddKey(new Keyframe(0, iO.beginScaleX, 0, 0));// = AnimationCurve.EaseInOut (0, iO.beginScaleX, iO.animDuration, iO.endScaleX);
			curveScaleY.AddKey(new Keyframe(0, iO.beginScaleY, 0, 0)); //= AnimationCurve.EaseInOut (0, iO.beginScaleY, iO.animDuration, iO.endScaleY);
		} 
		else 
		{
            curvePosX.AddKey(new Keyframe(0, iO.beginPosX, curvePosXref.keys[0].inTangent, curvePosXref.keys[0].outTangent));
            curvePosY.AddKey(new Keyframe(0, iO.beginPosY, curvePosYref.keys[0].inTangent, curvePosYref.keys[0].outTangent));
            curveRot.AddKey(new Keyframe(0, iO.beginRot, curveRotref.keys[0].inTangent, curveRotref.keys[0].outTangent));
            curveScaleX.AddKey(new Keyframe(0, iO.beginScaleX, curveScaleXref.keys[0].inTangent, curveScaleXref.keys[0].outTangent));
            curveScaleY.AddKey(new Keyframe(0, iO.beginScaleY, curveScaleYref.keys[0].inTangent, curveScaleYref.keys[0].outTangent));
        }

        if (iO.easeOut)
        {
            curvePosX.AddKey(new Keyframe(iO.animDuration, iO.endPosX, 0, 0));// = AnimationCurve.EaseInOut (0, iO.beginPosX, iO.animDuration, iO.endPosX);
            curvePosY.AddKey(new Keyframe(iO.animDuration, iO.endPosY, 0, 0)); //= AnimationCurve.EaseInOut (0, iO.beginPosY, iO.animDuration, iO.endPosY);
            curveRot.AddKey(new Keyframe(iO.animDuration, iO.endRot, 0, 0)); //= AnimationCurve.EaseInOut (0, iO.beginRot, iO.animDuration, iO.beginRot);
            curveScaleX.AddKey(new Keyframe(iO.animDuration, iO.endScaleX, 0, 0));// = AnimationCurve.EaseInOut (0, iO.beginScaleX, iO.animDuration, iO.endScaleX);
            curveScaleY.AddKey(new Keyframe(iO.animDuration, iO.endScaleY, 0, 0)); //= AnimationCurve.EaseInOut (0, iO.beginScaleY, iO.animDuration, iO.endScaleY);
        }
        else
        {
            curvePosX.AddKey(new Keyframe(iO.animDuration, iO.endPosX, curvePosXref.keys[1].inTangent, curvePosXref.keys[1].outTangent));
            curvePosY.AddKey(new Keyframe(iO.animDuration, iO.endPosY, curvePosYref.keys[1].inTangent, curvePosYref.keys[1].outTangent));
            curveRot.AddKey(new Keyframe(iO.animDuration, iO.endRot, curveRotref.keys[1].inTangent, curveRotref.keys[1].outTangent));
            curveScaleX.AddKey(new Keyframe(iO.animDuration, iO.endScaleX, curveScaleXref.keys[1].inTangent, curveScaleXref.keys[1].outTangent));
            curveScaleY.AddKey(new Keyframe(iO.animDuration, iO.endScaleY, curveScaleYref.keys[1].inTangent, curveScaleYref.keys[1].outTangent));
        }

        curve = curvePosX;

        canInterupt = canInterup;

		Transform rT = image.GetComponent<Transform> ();

		rT.localPosition = new Vector3 (iO.beginPos.x, iO.beginPos.y, rT.localPosition.z);
		rT.localEulerAngles = new Vector3 (0, 0, iO.beginRot);

		rT.localScale = new Vector3 (iO.beginScale.x, iO.beginScale.y, rT.localScale.z);;

		float timer = Time.time;

		while (Time.time-timer < iO.animDuration) {
			
			rT.localPosition = new Vector3(curvePosX.Evaluate(Time.time-timer),curvePosY.Evaluate(Time.time-timer),0);

			rT.localEulerAngles = new Vector3(0, 0, curveRot.Evaluate(Time.time-timer));

			rT.localScale = new Vector3(curveScaleX.Evaluate(Time.time-timer),curveScaleY.Evaluate(Time.time-timer),0);

			yield return null;
		}

		rT.localPosition = new Vector3 (iO.endPos.x, iO.endPos.y, rT.localPosition.z);
		rT.localEulerAngles = new Vector3 (0, 0, iO.endRot);
		rT.localScale = new Vector3 (iO.endScale.x, iO.endScale.y, rT.localScale.z);

		yield return null;

		canInterupt = true;

	}

}
