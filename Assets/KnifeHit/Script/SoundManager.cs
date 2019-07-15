using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
    //edited audio file reference
    int knifeHitFID;
    int throwKnifeFID;
    int lastHitFID;
    int woodHitFID;

    //edited audio stream reference
    int knifeHitSID;
    int throwKnifeSID;
    int lastHitSID;
    int woodHitSID;

    public AudioSource efxSource;
	public AudioClip btnSfx;
	public AudioClip timeSfx;
	// Use this for initialization
	void Awake () {

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
		
	}
    //edited add whole fuction start
    void Start()
    {
        AndroidNativeAudio.makePool();
        //loading all FileID's
        knifeHitFID = AndroidNativeAudio.load("ev_knife_hit_1.mp3");
        throwKnifeFID = AndroidNativeAudio.load("ev_throw_1.mp3");
        lastHitFID = AndroidNativeAudio.load("ev_hit_last.mp3");
        woodHitFID = AndroidNativeAudio.load("ev_hit_1.mp3");
    }

	public void PlaySingle(AudioClip clip,float vol=1f)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		if (GameManager.Sound && clip !=null) {
			AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position, vol);
		}
		if (clip !=null)
		{

		//	Debug.LogError ("Sound No verible Null 6e");
		}
	}
	public void PlayTimerSound()
	{
		if (GameManager.Sound) {
			efxSource.clip = timeSfx;
			efxSource.Play ();
		}
	}
	public void StopTimerSound(){
		efxSource.Stop ();
		efxSource.clip = null;
	}
	public void PlaybtnSfx(){
		PlaySingle (btnSfx);
	}

	public void playVibrate()
	{
		if(GameManager.Vibration)
			Handheld.Vibrate ();

	}

    //edited add whole fuctions below StreamID's
    public void KnifeHitSFX()
    {
        if (GameManager.Sound)
            knifeHitSID = AndroidNativeAudio.play(knifeHitFID);
    }
    public void ThrowKnifeSFX()
    {
        if (GameManager.Sound)
            throwKnifeSID = AndroidNativeAudio.play(throwKnifeFID);
    }
    public void LastHitSFX()
    {
        if (GameManager.Sound)
            lastHitSID = AndroidNativeAudio.play(lastHitFID);
    }
    public void WoodHitSFX()
    {
        if (GameManager.Sound)
            woodHitSID = AndroidNativeAudio.play(woodHitFID);
    }


    // edited Clean up when done
    void OnApplicationQuit()
    {       
        AndroidNativeAudio.unload(knifeHitSID);
        AndroidNativeAudio.unload(throwKnifeSID);
        AndroidNativeAudio.unload(lastHitSID);
        AndroidNativeAudio.unload(woodHitSID);
        AndroidNativeAudio.releasePool();
    }
}
