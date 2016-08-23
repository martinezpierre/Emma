using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class DetailEditor : EditorWindow {

	static SentenceObject sentence;

	static ImageObject[] imgList;
	static int nbImg;
	static int nbSound;
	static int nbChoice;

	static SoundObject[] soundList;

	static ChoiceObject[] choiceList;

	static SoundObject theme;
    static SoundObject soundscape;

    public static string fileName;

	Vector2 scrollPos;
	Vector2 scrollPosSound;
	Vector2 scrollPosChoice;

	static AnimBool[] m_ShowExtraFields;
	static AnimBool[] m_ShowExtraFieldsSound;

	static float[] animDuration;

	static Vector2[] beginPos;
	static float[] beginRot;
	static Vector2[] beginScale;

	static Vector2[] endPos;
	static float[] endRot;
	static Vector2[] endScale;

	static bool[] appearAfterSentence;

	static bool[] ease;
    static bool[] easeOut;

    static float[] soundFadeInDuration;
	static float[] soundFadeOutDuration;
	static float[] soundDelay;

	static float themeFadeInDuration;
	static float themeFadeOutDuration;
	static float themeDelay;

    static bool stopMusic;

    static float soundscapeFadeInDuration;
    static float soundscapeFadeOutDuration;
    static float soundscapeDelay;

    static string[] stringIndex;
	static string[] stringIndexEnd;

	static bool[] soundLoop;

	static List<CharacterObject> characters;

	public static string[] options;
	public int index;

	bool posGroupEnabled = false;

	float delayBeforeAppear;

	bool isSMS = false;

	SentenceObject.Transitions transition;
	float fadeDuration;
	float fadeDurationIn;
	Color fadeColor;

	public static void ShowWindow(string fN, SentenceObject sO)
	{
		fileName = fN;

		sentence = sO;

		animDuration = new float[100];

		beginPos = new Vector2[100];
		beginRot = new float[100];
		beginScale = new Vector2[100];
		for (int i = 0; i < beginScale.Length; i++) {
			beginScale [i] = Vector2.one;
		}

		endPos = new Vector2[100];
		endRot = new float[100];
		endScale = new Vector2[100];
		for (int i = 0; i < endScale.Length; i++) {
			endScale [i] = Vector2.one;
		}

		soundFadeInDuration = new float[100];
		soundFadeOutDuration = new float[100];
		soundDelay = new float[100];
		soundLoop = new bool[100];

		soundList = new SoundObject[100];
		imgList = new ImageObject[100];
		m_ShowExtraFields = new AnimBool[100];
		m_ShowExtraFieldsSound = new AnimBool[100];

		choiceList = new ChoiceObject[10];
		stringIndex = new string[10];
		stringIndexEnd = new string[10];

		options = new string[100];

		appearAfterSentence = new bool[100];

		ease = new bool[100];
        easeOut = new bool[100];

        nbImg = 0;
		nbSound = 0;
		nbChoice = 0;

		theme = new SoundObject ();
        soundscape = new SoundObject();

        DetailEditor dE = (DetailEditor)EditorWindow.GetWindow(typeof(DetailEditor));
		dE.init ();

	}

	void OnEnable(){
		/*maxSize = new Vector2(500f, 600f);
		minSize = maxSize;*/

	}

	void init()
	{
		transition = SentenceObject.Transitions.Simple;

		index = 0;

		fadeDuration = 1f;

		posGroupEnabled = false;

		LoadModifs ();


	}

	void OnFocus()
	{
		for (int j = 0; j < 100; j++) 
		{
			m_ShowExtraFields[j] = new AnimBool(false);
			m_ShowExtraFields[j].valueChanged.AddListener(Repaint);
		}
		for (int j = 0; j < 100; j++) 
		{
			m_ShowExtraFieldsSound[j] = new AnimBool(false);
			m_ShowExtraFieldsSound[j].valueChanged.AddListener(Repaint);
		}
	}

	void OnGUI()
	{
		if (sentence == null)
			return;


		scrollPos = EditorGUILayout.BeginScrollView(scrollPos,false,false);

		GUILayout.Label ("Text", EditorStyles.boldLabel);
		GUILayout.Label (sentence.text, EditorStyles.helpBox);

		delayBeforeAppear = EditorGUILayout.FloatField ("Delay before appear", delayBeforeAppear);

		GUILayout.Label ("Character", EditorStyles.boldLabel);
		index = EditorGUILayout.Popup(index, options);

		isSMS = EditorGUILayout.ToggleLeft ("SMS", isSMS);

		GUIImages ();

		GUISound ();

		GUIChoice ();

		EditorGUILayout.EndScrollView();

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Save",GUILayout.Height(50))) 
		{
			SaveModifs();
		}
	}

	void GUIImages()
	{
		GUILayout.Label ("Images", EditorStyles.boldLabel);

        posGroupEnabled = EditorGUILayout.Toggle("Change images", posGroupEnabled);

        /*posGroupEnabled = EditorGUILayout.BeginToggleGroup("Change images", posGroupEnabled);
        if (!posGroupEnabled)
        {
            transition = SentenceObject.Transitions.Simple;
        }*/

        GUILayout.BeginHorizontal ();
		transition = (SentenceObject.Transitions)EditorGUILayout.EnumPopup ("Transition type", transition);
		if (transition == SentenceObject.Transitions.Crossfade) 
		{
			fadeDuration = EditorGUILayout.FloatField ("Duration :", fadeDuration);
		}
		else if(transition == SentenceObject.Transitions.Fade)
		{
			GUILayout.BeginVertical ();
			fadeColor = EditorGUILayout.ColorField ("Color :",fadeColor);
			fadeDuration = EditorGUILayout.FloatField ("Fade Out Duration :", fadeDuration);
			fadeDurationIn = EditorGUILayout.FloatField ("Fade In Duration :", fadeDurationIn);
			GUILayout.EndVertical ();
		}

		GUILayout.EndHorizontal ();

		for (int i = 0; i < nbImg; i++) {

			if (imgList [i].isImage) {
				imgList [i].image = EditorGUILayout.ObjectField (imgList [i].image, typeof(Sprite), true, GUILayout.Height (75), GUILayout.Width (75)) as Sprite;
			} 
			else 
			{
				imgList [i].gameObject = EditorGUILayout.ObjectField (imgList [i].gameObject, typeof(GameObject), true, GUILayout.Height (20), GUILayout.Width (100)) as GameObject;
			}

			GUILayout.BeginHorizontal ();
			m_ShowExtraFields [i].target = EditorGUILayout.ToggleLeft ("Edit anim", m_ShowExtraFields [i].target);

			appearAfterSentence[i] = EditorGUILayout.ToggleLeft ("Appear after sentence", appearAfterSentence[i]);
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			//Extra block that can be toggled on and off.
			if (EditorGUILayout.BeginFadeGroup (m_ShowExtraFields [i].faded)) {
				EditorGUI.indentLevel++;

                GUILayout.BeginHorizontal();
                if(GUILayout.Button("Force height fill"))
                {
                    float ratio = 16f / 9f;

                    float imgWidth = imgList[i].image.rect.width;
                    float imgHeight = imgList[i].image.rect.height;

                    float imgNewHeight = imgHeight * ratio;

                    float newRatio = imgWidth / imgNewHeight;
                    
                    beginScale[i] = Vector2.one * newRatio;
                    endScale[i] = Vector2.one * newRatio;
                }
                if (GUILayout.Button("Force width fill"))
                {
                    float ratio = 9f / 16f;

                    float imgWidth = imgList[i].image.rect.width;
                    float imgHeight = imgList[i].image.rect.height;

                    float imgNewWidth = imgWidth * ratio;

                    float newRatio = imgHeight / imgNewWidth;
                    
                    beginScale[i] = Vector2.one * newRatio;
                    endScale[i] = Vector2.one * newRatio;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
				ease [i] = EditorGUILayout.ToggleLeft ("EaseIn", ease [i]);
                easeOut[i] = EditorGUILayout.ToggleLeft("EaseOut", easeOut[i]);
                GUILayout.EndHorizontal();

				GUILayout.Label ("Duration (seconds)",EditorStyles.boldLabel);
				animDuration[i] = EditorGUILayout.FloatField (animDuration[i]);
				GUILayout.BeginHorizontal ();
				GUILayout.BeginVertical ();
				GUILayout.Label ("Begin",EditorStyles.boldLabel);
				beginPos[i] = EditorGUILayout.Vector2Field ("Position (pixels)",beginPos[i]);
				GUILayout.Label ("Rotation (degrees)");
				beginRot[i] = EditorGUILayout.FloatField (beginRot[i]);
				beginScale[i] = EditorGUILayout.Vector2Field ("Scale (1=100%)",beginScale[i]);
				GUILayout.EndVertical ();
				GUILayout.BeginVertical ();
				GUILayout.Label ("End",EditorStyles.boldLabel);
				endPos[i] = EditorGUILayout.Vector2Field ("Position (pixels)",endPos[i]);
				GUILayout.Label ("Rotation (degrees)");
				endRot[i] = EditorGUILayout.FloatField (endRot[i]);
				endScale[i] = EditorGUILayout.Vector2Field ("Scale (1=100%)",endScale[i]);
				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.EndFadeGroup ();

		}

		//EditorGUILayout.EndToggleGroup();

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("New image")) 
		{
			if (!isSMS) 
			{
				nbImg++;
				imgList [nbImg-1] = new ImageObject ();
				imgList [nbImg - 1].isImage = true;
			}

		}
		if (GUILayout.Button ("New game object")) 
		{
			nbImg++;
			imgList [nbImg-1] = new ImageObject ();
			imgList [nbImg - 1].isImage = false;

		}
		GUILayout.EndHorizontal ();
	}

	void GUISound()
	{
		GUILayout.Label ("Sounds", EditorStyles.boldLabel);

        stopMusic = EditorGUILayout.ToggleLeft("Stop music",stopMusic);

        theme.sound = EditorGUILayout.ObjectField ("Theme",theme.sound, typeof(AudioClip), true) as AudioClip;
        m_ShowExtraFieldsSound [0].target = EditorGUILayout.ToggleLeft ("Options", m_ShowExtraFieldsSound [0].target);
		if (EditorGUILayout.BeginFadeGroup (m_ShowExtraFieldsSound [0].faded)) {
			EditorGUI.indentLevel++;

			themeFadeInDuration = EditorGUILayout.FloatField ("Fade In duration",themeFadeInDuration);
			themeFadeOutDuration = EditorGUILayout.FloatField ("Fade Out duration",themeFadeOutDuration);
			themeDelay = EditorGUILayout.FloatField ("Delay before playing",themeDelay);

			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();
        
        soundscape.sound = EditorGUILayout.ObjectField("Soundscape", soundscape.sound, typeof(AudioClip), true) as AudioClip;
        m_ShowExtraFieldsSound[1].target = EditorGUILayout.ToggleLeft("Options", m_ShowExtraFieldsSound[1].target);
        if (EditorGUILayout.BeginFadeGroup(m_ShowExtraFieldsSound[1].faded))
        {
            EditorGUI.indentLevel++;

            soundscapeFadeInDuration = EditorGUILayout.FloatField("Fade In duration", soundscapeFadeInDuration);
            soundscapeFadeOutDuration = EditorGUILayout.FloatField("Fade Out duration", soundscapeFadeOutDuration);
            soundscapeDelay = EditorGUILayout.FloatField("Delay before playing", soundscapeDelay);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        GUILayout.Label ("Sounds");
		//scrollPosSound = EditorGUILayout.BeginScrollView(scrollPosSound,false,false);

		for (int i = 0; i < nbSound; i++) {
			soundList[i].sound = EditorGUILayout.ObjectField (soundList[i].sound, typeof(AudioClip), true) as AudioClip;
			m_ShowExtraFieldsSound [i+2].target = EditorGUILayout.ToggleLeft ("Options", m_ShowExtraFieldsSound [i+2].target);
			if (EditorGUILayout.BeginFadeGroup (m_ShowExtraFieldsSound [i+2].faded)) {
				EditorGUI.indentLevel++;

				soundLoop [i] = EditorGUILayout.ToggleLeft ("Loop", soundLoop [i]);
				soundFadeInDuration[i] = EditorGUILayout.FloatField ("Fade In duration",soundFadeInDuration[i]);
				soundFadeOutDuration[i] = EditorGUILayout.FloatField ("Fade Out duration",soundFadeOutDuration[i]);
				soundDelay[i] = EditorGUILayout.FloatField ("Delay before playing",soundDelay[i]);

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.EndFadeGroup ();
		}

		//EditorGUILayout.EndScrollView();

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("New Sound")) 
		{
			nbSound++;
			soundList [nbSound-1] = new SoundObject ();

		}
		GUILayout.EndHorizontal ();
	}

	void GUIChoice ()
	{

		GUILayout.Label ("Choices", EditorStyles.boldLabel);

		//scrollPosChoice = EditorGUILayout.BeginScrollView(scrollPosChoice,false,false);
		for (int i = 0; i < nbChoice; i++) 
		{
			GUILayout.BeginHorizontal ();

			choiceList [i].answer = EditorGUILayout.TextField (choiceList [i].answer);
			stringIndex [i] = EditorGUILayout.TextField ("go to sequence from",stringIndex [i]);
			stringIndexEnd [i] = EditorGUILayout.TextField ("to",stringIndexEnd [i]);

			GUILayout.EndHorizontal ();

			if (stringIndex [i] != null && stringIndexEnd [i] != null && stringIndex [i].Contains(".") && stringIndexEnd [i].Contains(".")) 
			{
				string[] res = stringIndex [i].Split ('.');
				string[] res2 = stringIndexEnd [i].Split ('.');

				int a, b, a2, b2;

				if (int.TryParse (res [0],out a) && int.TryParse (res [1], out b)) {
					choiceList [i].nextSentenceIndex = a * 1000 + b;
				}
				if (int.TryParse (res2 [0], out a2) && int.TryParse (res2 [1], out b2)) {
					choiceList [i].endSequenceIndex = a2 * 1000 + b2;
				}

				SentenceObject sO = TextManager.GetSentence (choiceList [i].nextSentenceIndex);
				SentenceObject sOEnd = TextManager.GetSentence (choiceList [i].endSequenceIndex);

				GUILayout.BeginHorizontal ();
				GUILayout.Label (sO.text, EditorStyles.helpBox);
				GUILayout.Label ("->");
				GUILayout.Label (sOEnd.text, EditorStyles.helpBox);
				GUILayout.EndHorizontal ();
			}
		}
		//EditorGUILayout.EndScrollView();

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("New Choice")) 
		{
			if (nbChoice < 4) {
				nbChoice++;
				choiceList [nbChoice - 1] = new ChoiceObject ();
			} 
		}

		GUILayout.EndHorizontal ();

	}

	void SaveModifs()
	{
		foreach (SentenceObject sO in TextManager.texts.sentences) 
		{
			if (sO.index == sentence.index) 
			{
				sO.transitionType = transition;
				switch (transition) {
				case SentenceObject.Transitions.Simple:
					sO.fade = false;
					sO.crossfade = false;
					break;
				case SentenceObject.Transitions.Fade:
					sO.fade = true;
					sO.crossfade = false;
					break;
				case SentenceObject.Transitions.Crossfade:
					sO.fade = false;
					sO.crossfade = true;
					break;
				}

				sO.isSMS = isSMS;

				sO.fadeDuration = fadeDuration;

				sO.fadeInDuration = fadeDurationIn;

				sO.fadeColorR = fadeColor.r;
				sO.fadeColorG = fadeColor.g;
				sO.fadeColorB = fadeColor.b;

				sO.keepPreviousImages = !posGroupEnabled;

				sO.characterName = options [index];

				sO.delayBeforeAppear = delayBeforeAppear;

				sO.themes = new List<SoundObject> ();

				theme.soundPath = AssetDatabase.GetAssetPath (theme.sound);
				theme.delayBeforeAppear = themeDelay;
				theme.fadeInDuration = themeFadeInDuration;
				theme.fadeOutDuration = themeFadeOutDuration;

				sO.themes.Add(theme);

                soundscape.soundPath = AssetDatabase.GetAssetPath(soundscape.sound);
                soundscape.delayBeforeAppear = soundscapeDelay;
                soundscape.fadeInDuration = soundscapeFadeInDuration;
                soundscape.fadeOutDuration = soundscapeFadeOutDuration;

                sO.themes.Add(soundscape);

                sO.stopTheme = stopMusic;

                sO.images = new List<ImageObject> ();

				for(int i = 0; i < imgList.Length; i++)
				{
					ImageObject iO = imgList [i];
					if (iO!=null && (iO.image != null || iO.gameObject != null)) 
					{
						if (iO.image != null) 
						{
							iO.imagePath = AssetDatabase.GetAssetPath (iO.image);
						} 
						else 
						{
							iO.imagePath = AssetDatabase.GetAssetPath (iO.gameObject);
						}

						iO.animDuration = animDuration [i];

						iO.beginPosX = beginPos[i].x;
						iO.beginPosY = beginPos[i].y;

						iO.beginRot = beginRot[i];

						iO.beginScaleX = beginScale[i].x;
						iO.beginScaleY = beginScale[i].y;

						iO.endPosX = endPos[i].x;
						iO.endPosY = endPos[i].y;

						iO.endRot = endRot[i];

						iO.endScaleX = endScale[i].x;
						iO.endScaleY = endScale[i].y;

						iO.appearAfterSentence = appearAfterSentence [i];

						iO.ease = ease [i];
                        iO.easeOut = easeOut[i];

						sO.images.Add (iO);
					}
				}

				sO.sounds = new List<SoundObject> ();

				for(int i = 0; i < soundList.Length; i++)
				{
					SoundObject sdO = soundList [i];
					if (sdO!=null && sdO.sound != null) 
					{
						sdO.soundPath = AssetDatabase.GetAssetPath (sdO.sound);

						sdO.fadeInDuration = soundFadeInDuration[i];
						sdO.fadeOutDuration = soundFadeOutDuration[i];
						sdO.delayBeforeAppear = soundDelay[i];
						sdO.loop = soundLoop [i];

						sO.sounds.Add (sdO);
					}
				}

				sO.choices = new List<ChoiceObject> ();

				for(int i = 0; i < choiceList.Length; i++)
				{
					ChoiceObject cO = choiceList [i];
					if (cO!=null && cO.answer != "" && stringIndex [i]!= null && stringIndexEnd [i]!= null) 
					{
						string[] res = stringIndex [i].Split ('.');
						string[] res2 = stringIndexEnd [i].Split ('.');

						cO.nextSentenceIndex = int.Parse(res[0])*1000+ int.Parse(res[1]);
						cO.endSequenceIndex = int.Parse(res2[0])*1000+ int.Parse(res2[1]);

						sO.choices.Add (cO);
					}
				}

				break;
			}
		}

		TextManager.SaveText (fileName);
	}

	void LoadModifs()
	{
		TextManager.LoadText (fileName);

		characters = TextManager.texts.characters;

		int i = 0;

		options = new string[characters.Count];

		foreach (CharacterObject cO in characters) 
		{
			options [i] = cO.name;
			i++;
		}

		for (i = 0; i < options.Length; i++) 
		{
			if(options[i] == sentence.characterName && sentence.characterName != null)
			{
				index = i;
			}
		}

		posGroupEnabled = !sentence.keepPreviousImages;

		transition = sentence.transitionType;

		fadeDuration = sentence.fadeDuration == 0f ? 1f : sentence.fadeDuration;

		fadeDurationIn = sentence.fadeInDuration == 0f ? 1f : sentence.fadeInDuration;

		fadeColor = sentence.fadeColor;

		delayBeforeAppear = sentence.delayBeforeAppear;

		isSMS = sentence.isSMS;

		if (sentence.themes.Count > 0) {
			theme = sentence.themes[0];
			themeDelay = theme.delayBeforeAppear;
			themeFadeInDuration = theme.fadeInDuration;
			themeFadeOutDuration = theme.fadeOutDuration;
		}

        if (sentence.themes.Count > 1)
        {
            soundscape = sentence.themes[1];
            soundscapeDelay = soundscape.delayBeforeAppear;
            soundscapeFadeInDuration = soundscape.fadeInDuration;
            soundscapeFadeOutDuration = soundscape.fadeOutDuration;
        }

        stopMusic = sentence.stopTheme;

        foreach (ImageObject iM in sentence.images) 
		{
			nbImg++;
			imgList [nbImg-1] = iM;

			animDuration [nbImg - 1] = iM.animDuration;

			beginPos[nbImg-1].x = iM.beginPosX;
			beginPos[nbImg-1].y = iM.beginPosY;

			beginRot[nbImg-1] = iM.beginRot;

			bool mustChange = iM.beginScaleX == 0 && iM.beginScaleY == 0 && iM.endScaleX == 0 && iM.endScaleY == 0;

			beginScale[nbImg-1].x = mustChange ? 1 : iM.beginScaleX;
			beginScale [nbImg - 1].y = mustChange ? 1 : iM.beginScaleY ;

			endPos[nbImg-1].x = iM.endPosX;
			endPos[nbImg-1].y = iM.endPosY;

			endRot[nbImg-1] = iM.endRot;

			endScale[nbImg-1].x = mustChange ? 1 : iM.endScaleX;
			endScale[nbImg-1].y = mustChange ? 1 : iM.endScaleY;

			appearAfterSentence [nbImg - 1] = iM.appearAfterSentence;

			ease [nbImg - 1] = iM.ease;
            easeOut[nbImg - 1] = iM.easeOut;
		}

		foreach (SoundObject sM in sentence.sounds) 
		{
			nbSound++;
			soundList [nbSound-1] = sM;

			soundFadeInDuration[nbSound-1] = sM.fadeInDuration;
			soundFadeOutDuration[nbSound-1] = sM.fadeOutDuration;
			soundDelay[nbSound-1] = sM.delayBeforeAppear;
			soundLoop[nbSound-1] = sM.loop;

		}

		foreach (ChoiceObject cM in sentence.choices) 
		{
			nbChoice++;
			choiceList [nbChoice-1] = cM;

			stringIndex[nbChoice-1] = (cM.nextSentenceIndex/1000) + "." + (cM.nextSentenceIndex%1000) ;
			stringIndexEnd[nbChoice-1] = (cM.endSequenceIndex/1000) + "." + (cM.endSequenceIndex%1000) ;

		}
	}

}
