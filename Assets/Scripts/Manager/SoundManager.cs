using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	static SoundManager instance;

	public static SoundManager Instance
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		instance = this;
	}

	SoundObject currentTheme;
    SoundObject currentSoundscape;

    AudioSource themeSource;
    AudioSource soundscapeSource;

    public void HandleSounds(SentenceObject sO)
	{
        if (sO.stopTheme)
        {
            StopAllSound();
        }
        else
        {
            if (sO.themes.Count > 0)
            {
                PlaySound(sO.themes[0], true, false);
            }
            if (sO.themes.Count > 1)
            {
                PlaySound(sO.themes[1], false, true);
            }
            foreach (SoundObject sdO in sO.sounds)
            {

                if (sdO.sound != null)
                {
                    PlaySound(sdO, false, false);
                }
            }
        }
	}

    public void StopAllSound()
    {
        StartCoroutine(StopTheme(currentTheme, null, false));
        StartCoroutine(StopTheme(currentSoundscape, null, true));
    }

	public void PlaySound(SoundObject sound, bool isTheme, bool isSoundscape)
	{
		if (isTheme && sound.sound != null) {
			ChangeTheme (sound);
		}
        else if (isSoundscape && sound.sound != null)
        {
            ChangeSoundscape(sound);
        }
        else if(sound.sound != null){
			StartCoroutine(StartSound(sound));
		}

	}

	public void ChangeTheme(SoundObject newTheme)
	{
		if (currentTheme != null) 
		{
			StartCoroutine (StopTheme (currentTheme,newTheme,false));
		} 
		else 
		{
			GameObject go = new GameObject ("AudioTheme");

			themeSource = go.AddComponent<AudioSource> ();

			themeSource.clip = newTheme.sound;
            themeSource.loop = true;

            currentTheme = newTheme;

            StartCoroutine(StartTheme(currentTheme,false));
		}
	}

    public void ChangeSoundscape(SoundObject newSoundscape)
    {
        if (currentSoundscape != null)
        {
            StartCoroutine(StopTheme(currentSoundscape, newSoundscape,true));
        }
        else
        {
            GameObject go = new GameObject("AudioScape");

            soundscapeSource = go.AddComponent<AudioSource>();

            soundscapeSource.clip = newSoundscape.sound;
            soundscapeSource.loop = true;

            currentSoundscape = newSoundscape;

            StartCoroutine(StartTheme(currentSoundscape, true));
        }
    }

    IEnumerator StartSound(SoundObject sO)
	{
		GameObject go = new GameObject ("AudioSound");

		AudioSource aS = go.AddComponent<AudioSource> ();

		aS.clip = sO.sound;
		aS.loop = sO.loop;

		float timer = Time.time;

		while (Time.time - timer < sO.delayBeforeAppear) {
			yield return null;
		}

		aS.Play ();
        
        AnimationCurve aCIn = AnimationCurve.Linear(0, 0, sO.fadeInDuration, 1);
		AnimationCurve aCOut = AnimationCurve.Linear (sO.sound.length - sO.fadeOutDuration, 1, sO.sound.length, 0);

		timer = Time.time;

		while (Time.time - timer < sO.sound.length) 
		{
			if (Time.time - timer < sO.fadeInDuration) {
				aS.volume = aCIn.Evaluate (Time.time - timer);
			} else {
				aS.volume = aCOut.Evaluate (Time.time - timer);
			}

			yield return null;
		}
        
        Destroy(go);
        
        yield return null;
	}

	IEnumerator StartTheme(SoundObject sO, bool soundscape)
	{
		float timer = Time.time;

		while (Time.time - timer < sO.delayBeforeAppear) {
			yield return null;
		}

        if (soundscape)
        {
            soundscapeSource.Play();
        }
        else
        {
            themeSource.Play();
        }
        
		AnimationCurve aCIn = AnimationCurve.Linear (0, 0f, sO.fadeInDuration, 1f);

		timer = Time.time;

		while (Time.time - timer < sO.fadeInDuration) 
		{
            if (soundscape)
            {
                soundscapeSource.volume = aCIn.Evaluate(Time.time - timer);
            }
            else
            {
                themeSource.volume = aCIn.Evaluate(Time.time - timer);
            }
            
			yield return null;
		}

        if (soundscape)
        {
            soundscapeSource.volume = 1f;
        }
        else
        {
            themeSource.volume = 1f;
        }

		yield return null;
	}

	IEnumerator StopTheme(SoundObject sO, SoundObject newTheme, bool soundscape)
	{
		float timer = Time.time;

		AnimationCurve aCOut = AnimationCurve.Linear (0, 1f, sO.fadeOutDuration, 0f);

		timer = Time.time;

		while (Time.time - timer < sO.fadeOutDuration) 
		{
            if (soundscape)
            {
                soundscapeSource.volume = aCOut.Evaluate(Time.time - timer);
            }
            else
            {
                themeSource.volume = aCOut.Evaluate(Time.time - timer);
            }
            yield return null;
		}

        if (soundscape)
        {
            soundscapeSource.volume = 0f;
        }
        else
        {
            themeSource.volume = 0f;
        }

        if (newTheme != null)
        {
            if (soundscape)
            {
                soundscapeSource.clip = newTheme.sound;
            }
            else
            {
                themeSource.clip = newTheme.sound;
            }

            currentTheme = newTheme;

            StartCoroutine(StartTheme(currentTheme, soundscape));
        }
        else
        {
            currentTheme = null;
        }

		yield return null;
	}
}
