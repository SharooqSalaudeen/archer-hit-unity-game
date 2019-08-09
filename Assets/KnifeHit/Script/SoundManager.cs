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
    int appleHitFID;
    int onUnlockFID;
    int randomUnlockFID;
    int confirmKnifeFID;
    int lockKnifeFID;
    int unlockKnifeFID;

    //edited audio stream reference
    int knifeHitSID;
    int throwKnifeSID;
    int lastHitSID;
    int woodHitSID;
    int appleHitSID;
    int onUnlockSID;
    int randomUnlockSID;
    int confirmKnifeSID;
    int lockKnifeSID;
    int unlockKnifeSID;

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
        appleHitFID = AndroidNativeAudio.load("ev_apple_hit_1.mp3");
        onUnlockFID = AndroidNativeAudio.load("ev_shop_random_unlock.mp3");
        randomUnlockFID = AndroidNativeAudio.load("ev_shop_random_beep.mp3");
        confirmKnifeFID = AndroidNativeAudio.load("ev_shop_select_item_confirm.mp3");
        lockKnifeFID = AndroidNativeAudio.load("ev_shop_select_locked_item.mp3");
        unlockKnifeFID = AndroidNativeAudio.load("ev_shop_select_item.mp3");
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
    public void AppleHitSFX()
    {
        if (GameManager.Sound)
            appleHitSID = AndroidNativeAudio.play(appleHitFID);
    }
    public void OnUnlockSFX()
    {
        if (GameManager.Sound)
            onUnlockSID = AndroidNativeAudio.play(onUnlockFID);
    }
    public void RandomUnlockSFX()
    {
        if (GameManager.Sound)
            randomUnlockSID = AndroidNativeAudio.play(randomUnlockFID);
    }
    public void ConfirmKnifeSFX()
    {
        if (GameManager.Sound)
            confirmKnifeSID = AndroidNativeAudio.play(confirmKnifeFID);
    }
    public void LockKnifeSFX()
    {
        if (GameManager.Sound)
            lockKnifeSID = AndroidNativeAudio.play(lockKnifeFID);
    }
    public void UnlockKnifeSFX()
    {
        if (GameManager.Sound)
            unlockKnifeSID = AndroidNativeAudio.play(unlockKnifeFID);
    }


    // edited Clean up when done
    void OnApplicationQuit()
    {       
        AndroidNativeAudio.unload(knifeHitSID);
        AndroidNativeAudio.unload(throwKnifeSID);
        AndroidNativeAudio.unload(lastHitSID);
        AndroidNativeAudio.unload(woodHitSID);
        AndroidNativeAudio.unload(appleHitSID);
        AndroidNativeAudio.unload(onUnlockSID);
        AndroidNativeAudio.unload(randomUnlockSID);
        AndroidNativeAudio.unload(confirmKnifeSID);
        AndroidNativeAudio.unload(lockKnifeSID);
        AndroidNativeAudio.unload(unlockKnifeSID);
        AndroidNativeAudio.releasePool();
    }
}
