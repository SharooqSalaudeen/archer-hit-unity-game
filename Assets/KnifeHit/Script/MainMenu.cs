using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour 
{
	[Header("Main View")]
	public Button giftButton;
	public Text giftLable;
	public CanvasGroup giftLableCanvasGroup;
	public GameObject giftBlackScreen;
	public GameObject giftParticle;
	public Image selectedKnifeImage;
    //edited add selectedBowImage for Shop
    public Image selectedBowImage;
    public AudioClip giftSfx;
    public GamePlayManager gamePlayManager;


    public static MainMenu intance;

	// Gift Setting

	int timeForNextGift = 60*8;
	int minGiftApple = 40;// Minimum Apple for Gift
	int maxGiftApple = 70;// Maxmum Apple for Gift
	void Awake()
	{
		intance = this;
	}
	void Start()
	{
        CUtils.ShowInterstitialAd();
		InvokeRepeating ("updateGiftStatus", 0f, 1f);
        //edited commented next two instructions because of no use. UpdateUI will be called on the respective button click and KnifeShop to TestKnifeShop revert in future
        TestKnifeShop.intance.UpdateUI ();
        //edited added line for bow shop updating
        BowShop.intance.UpdateUI ();
	}

	public void OnPlayClick()
	{
		SoundManager.instance.PlaybtnSfx ();
        //GeneralFunction.intance.LoadSceneWithLoadingScreen ("MyGameScene");
        gamePlayManager.startGame();
        gameObject.SetActive(false);
	}
	public void RateGame()
	{
		SoundManager.instance.PlaybtnSfx ();
        CUtils.OpenStore();
	}

	void updateGiftStatus()
	{
        if (GameManager.GiftAvalible) {
			giftButton.interactable = true;
			LeanTween.alphaCanvas (giftLableCanvasGroup, 0f, .4f).setOnComplete (() => {
				LeanTween.alphaCanvas (giftLableCanvasGroup, 1f, .4f);
			});
			giftLable.text="READY!";
		} else {
			giftButton.interactable = false;
			giftLable.text = GameManager.RemendingTimeSpanForGift.Hours.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Minutes.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Seconds.ToString("00");
		}
	}
	[ContextMenu("Get Gift")]
	public void OnGiftClick()
	{
		SoundManager.instance.PlaybtnSfx ();
        //edited added next line
        gameObject.SetActive(false);
		int Gift = UnityEngine.Random.Range (minGiftApple, maxGiftApple);
        Toast.instance.ShowMessage("You got "+Gift+" Apples");
		GameManager.Apple += Gift;
		GameManager.NextGiftTime = DateTime.Now.AddMinutes(timeForNextGift);

        updateGiftStatus ();
		giftBlackScreen.SetActive (true);
		Instantiate<GameObject>(giftParticle);
		SoundManager.instance.PlaySingle (giftSfx);
		Invoke("HideGiftParticle",2f);
	}
	public void HideGiftParticle()
	{
		giftBlackScreen.SetActive (false);
        //edited added next line
        gameObject.SetActive(true);
    }
	public void OpenKnifeShopUI()
	{
		SoundManager.instance.PlaybtnSfx ();
        //KnifeShop to TestKnifeShop revert in future
        TestKnifeShop.intance.showShop ();	
    }
    public void OpenBowShopUI()
    {
        SoundManager.instance.PlaybtnSfx();
        BowShop.intance.showShop();
    }
    public void OpenSettingUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		SettingUI.intance.showUI();	
	}
}

