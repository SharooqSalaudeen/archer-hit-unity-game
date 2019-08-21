using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
//edited using System is commented to avoid ambeguity in Random function
//using System;

public class GamePlayManager : MonoBehaviour
{
    
    public static GamePlayManager instance;
    [Header("Circle Setting")]
    public Circle[] circlePrefabs;
    public Bosses[] BossPrefabs;

    public Transform circleSpawnPoint;
    //edited circleWidthByScreen=.5f; to circleHeightByScreen=.5f;
    [Range(0f, 1f)] public float circleHeightByScreen = .5f;
    //edited added woodSprites and woodSpriteParticles
    public List<Sprite> WoodSprites;
    public List<ParticleSystem> WoodSpriteParticles;
    int currentWoodSprite;



    [Header("Knife Setting")]
    public Knife knifePrefab;
    public Transform KnifeSpawnPoint;
    //edited knifeHeightByScreen = .1f; to knifeWidthByScreen = .1f;
    [Range(0f, 1f)] public float knifeWidthByScreen = .1f;
    public GameObject ApplePrefab;

    //edited add 4 lines
    [Header("Bow Settings")]
    public Bow bowPrefab;
    public Transform bowSpawnPoint;
    [Range(0f, 1f)] public float bowHeightByScreen = .5f;


    [Header("UI Object")]
    public Text lblScore;
    //edites add StageCounter for gift screen setActive
    public GameObject stageCounterView;
    public GameObject scoreView;
    public Text lblStage;
    public List<Image> stageIcons;
    public Color stageIconActiveColor;
    public Color stageIconNormalColor;

    

    [Header("UI Boss")]

    public GameObject bossFightStart;
    public GameObject bossFightEnd;
    public AudioClip[] bossFightStartSounds;
    public AudioClip[] bossFightEndSounds;

    [Header("mainMenu")]
    public GameObject mainMenuView;


    [Header("Ads Show")]
    public GameObject adsShowView;
    public Image adTimerImage;
    public Text adSocreLbl;


    [Header("GameOver Popup")]
    public GameObject gameOverView;
    public Text gameOverSocreLbl, gameOverStageLbl;
    public GameObject newBestScore;
    public AudioClip gameOverSfx;
    [Space(50)]

    public int cLevel = 0;
    public bool isDebug = false;
    string currentBossName = "";
    Circle currentCircle;
    Knife currentKnife;
    Bow currentBow;
    bool usedAdContinue;
    //edited add stringReady bool
    //bool stringReady;

    public int totalSpawnKnife
    {
        get
        {
            return _totalSpawnKnife;
        }
        set
        {
            _totalSpawnKnife = value;

        }
    }
    int _totalSpawnKnife;

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        //edited commented next line to implement it through main menu
        //startGame();
        //edited commented next line for future enabling and debugging****************************************************
        //CUtils.ShowInterstitialAd();
        currentWoodSprite = Random.Range(0, WoodSprites.Count);
    }

    private void OnEnable()
    {
        Timer.Schedule(this, 0.1f, AddEvents);
    }

    private void AddEvents()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            AdmobController.instance.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        }
#endif
    }

    bool doneWatchingAd = false;
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        if (usedAdContinue)
        {
            doneWatchingAd = true;
            AdShowSucessfully();
        }
    }

    public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
    {
        if (usedAdContinue)
        {
            if (doneWatchingAd == false)
            {
                adsShowView.SetActive(false);
                usedAdContinue = false;
                showGameOverPopup();
            }
        }
    }

    //edited private fuction to public for call in KnifeShop
    public bool IsAdAvailable()
    {
        if (AdmobController.instance.rewardBasedVideo == null) return false;
        bool isLoaded = AdmobController.instance.rewardBasedVideo.IsLoaded();
        return isLoaded;
    }

    private void OnDisable()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
            AdmobController.instance.rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;
        }
#endif
    }

    public void startGame()
    {
        GameManager.score = 0;
        GameManager.Stage = 1;
        GameManager.isGameOver = false;
        usedAdContinue = false;
        //edited add fuction SpawnBow()
        SpawnBow();
        if (isDebug)
        {
            GameManager.Stage = cLevel;
        }
        setupGame();
    }
    public void UpdateLable()
    {
        lblScore.text = GameManager.score + "";
        if (GameManager.Stage % 5 == 0)
        {
            for (int i = 0; i < stageIcons.Count - 1; i++)
            {
                stageIcons[i].gameObject.SetActive(false);
            }
            stageIcons[stageIcons.Count - 1].color = stageIconActiveColor;
            lblStage.color = stageIconActiveColor;
            lblStage.text = currentBossName;
        }
        else
        {
            lblStage.text = "STAGE " + GameManager.Stage;
            for (int i = 0; i < stageIcons.Count; i++)
            {
                stageIcons[i].gameObject.SetActive(true);
                stageIcons[i].color = GameManager.Stage % stageIcons.Count <= i ? stageIconNormalColor : stageIconActiveColor;
            }
            lblStage.color = stageIconNormalColor;
        }
    }
    public void setupGame()
    {
        spawnCircle();
        //edited added next 2 lines
        scoreView.SetActive(true);
        stageCounterView.SetActive(true);
        KnifeCounter.intance.setUpCounter(currentCircle.totalKnife);

        totalSpawnKnife = 0;
        StartCoroutine(GenerateKnife());
    }
    //edited made public
    void Update()
    {
        if (currentKnife == null)
            return;
        //edited add if statement to drag back arrow on hold and pull string
        if (Input.GetMouseButtonDown(0) && !currentKnife.isFire)
        {
            currentKnife.DrawArrow();
            currentBow.StringPull();
        }
        //edited (Input.GetMouseButtonDown(0) && !currentKnife.isFire) to (Input.GetMouseButtonUp(0) && !currentKnife.isFire) to fire when released
        if (Input.GetMouseButtonUp(0) && !currentKnife.isFire)
        {  
            KnifeCounter.intance.setHitedKnife(totalSpawnKnife);
            currentKnife.ThrowKnife();
            currentBow.BowShake();
            StartCoroutine(GenerateKnife());
            //edited add call string release/rest function
            currentBow.StringRest();
        }

    }
    public void spawnCircle()
    {
        GameObject tempCircle;
        if (GameManager.Stage % 5 == 0)
        {
            Bosses b = BossPrefabs[Random.Range(0, BossPrefabs.Length)];
            tempCircle = Instantiate<Circle>(b.BossPrefab, circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
            currentBossName = "Boss : " + b.Bossname;
            UpdateLable();
            currentWoodSprite = Random.Range(0, WoodSprites.Count);
            OnBossFightStart();
        }
        else
        {
            if (GameManager.Stage > 50)
            {
                tempCircle = Instantiate<Circle>(circlePrefabs[Random.Range(11, circlePrefabs.Length - 1)], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
                //edited added lines
                
                tempCircle.GetComponent<SpriteRenderer>().sprite = WoodSprites[currentWoodSprite];
            }
            else
            {
                tempCircle = Instantiate<Circle>(circlePrefabs[GameManager.Stage - 1], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
                //edited added line
                tempCircle.GetComponent<SpriteRenderer>().sprite = WoodSprites[currentWoodSprite];
            }
        }
        tempCircle.transform.localScale = Vector3.one;
        //edited (GameManager.ScreenWidth * circleWidthByScreen) to (GameManager.ScreenHeight * circleHeightByScreen)
        float circleScale = (GameManager.ScreenHeight * circleHeightByScreen) / tempCircle.GetComponent<SpriteRenderer>().bounds.size.x;
        tempCircle.transform.localScale = Vector3.one * .2f;
        LeanTween.scale(tempCircle, new Vector3(circleScale, circleScale, circleScale), .3f).setEaseOutBounce();

        //tempCircle.transform.localScale = Vector3.one*circleScale;
        currentCircle = tempCircle.GetComponent<Circle>();
        currentCircle.SetWoodSprite(WoodSpriteParticles[currentWoodSprite]);
    }

    //edited add fuction SpawnBox
    public void SpawnBow()
    {
        GameObject tempBow;
        if (GameManager.selectedKnifePrefab == null)
        {
            //edited KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f to KnifeSpawnPoint.position.x - 2f, KnifeSpawnPoint.position.y
            tempBow = Instantiate<Bow>(bowPrefab, bowSpawnPoint.position, Quaternion.identity, bowSpawnPoint).gameObject;
        }
        else
        {
            //edited KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f to KnifeSpawnPoint.position.x - 2f, KnifeSpawnPoint.position.y
            tempBow = Instantiate<Bow>(GameManager.selectedBowPrefab, bowSpawnPoint.position, Quaternion.identity, bowSpawnPoint).gameObject;

        }
        float bowScale = (GameManager.ScreenHeight * bowHeightByScreen) / tempBow.GetComponent<SpriteRenderer>().bounds.size.y;
        tempBow.transform.localScale = Vector3.one * bowScale;
        currentBow = tempBow.GetComponent<Bow>();
    }

    public IEnumerator OnBossFightStart()
    {
        bossFightStart.SetActive(true);
        //edited add next 5 lines (4th line is original)
#if UNITY_ANDROID && !UNITY_EDITOR
                    SoundManager.instance.BossFightStartSFX();
#else
        SoundManager.instance.PlaySingle(bossFightStartSounds[Random.Range(0, bossFightEndSounds.Length - 1)], 1f);
#endif
        yield return new WaitForSeconds(2f);
        bossFightStart.SetActive(false);
        setupGame();
    }

    public IEnumerator OnBossFightEnd()
    {
        bossFightEnd.SetActive(true);
        //edited add next 5 lines (4th line is original)
#if UNITY_ANDROID && !UNITY_EDITOR
                    SoundManager.instance.BossFightEndSFX();
#else
        SoundManager.instance.PlaySingle(bossFightEndSounds[Random.Range(0, bossFightEndSounds.Length - 1)], 1f);
#endif
        yield return new WaitForSeconds(2f);
        bossFightEnd.SetActive(false);
        setupGame();
    }
    public IEnumerator GenerateKnife()
    {
        //yield return new WaitForSeconds (0.1f);
        yield return new WaitUntil(() => {
            return KnifeSpawnPoint.childCount == 0;
        });
        if (currentCircle.totalKnife > totalSpawnKnife && !GameManager.isGameOver)
        {
            totalSpawnKnife++;
            GameObject tempKnife;
            if (GameManager.selectedKnifePrefab == null)
            {
                //edited KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f to KnifeSpawnPoint.position.x - 2f, KnifeSpawnPoint.position.y
                tempKnife = Instantiate<Knife>(knifePrefab, new Vector3(KnifeSpawnPoint.position.x - 2f, KnifeSpawnPoint.position.y, KnifeSpawnPoint.position.z), Quaternion.identity, KnifeSpawnPoint).gameObject;
            }
            else
            {
                //edited KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f to KnifeSpawnPoint.position.x - 2f, KnifeSpawnPoint.position.y
                tempKnife = Instantiate<Knife>(GameManager.selectedKnifePrefab, new Vector3(KnifeSpawnPoint.position.x - 2f, KnifeSpawnPoint.position.y, KnifeSpawnPoint.position.z), Quaternion.identity, KnifeSpawnPoint).gameObject;

            }
            tempKnife.transform.localScale = Vector3.one;
            //edited (GameManager.ScreenHeight * knifeHeightByScreen) to (GameManager.ScreenWidth * knifeWidthByScreen)
            float knifeScale = (GameManager.ScreenWidth * knifeWidthByScreen) / tempKnife.GetComponent<SpriteRenderer>().bounds.size.y;
            tempKnife.transform.localScale = Vector3.one * knifeScale;
            //edited next line is added to rotate the object to 90 degrees
            tempKnife.transform.Rotate(0, 0, -90, Space.Self);
            //edited moveLocalY(tempKnife, 0, 0.1f); to moveLocalX(tempKnife, 0, 0.1f); to generate knife in X axis motion
            LeanTween.moveLocalX(tempKnife, 0, 0.1f);
            tempKnife.name = "Knife" + totalSpawnKnife;
            currentKnife = tempKnife.GetComponent<Knife>();
        }
    }

    //edited add knife drag back fuction
    //public void DragKnife()
    //{
        //if (currentCircle.totalKnife > totalSpawnKnife && !GameManager.isGameOver)
        // {
        //LeanTween.moveLocalX(GameObject, -1f, 0.1f);
        //currentKnife.transform.position = new Vector3(KnifeSpawnPoint.position.x - 0.4f, KnifeSpawnPoint.position.y, KnifeSpawnPoint.position.z);
            //currentKnife.transform.position= Vector3.Lerp(currentKnife.transform.position, new Vector3(KnifeSpawnPoint.transform.position.x - 1f, KnifeSpawnPoint.transform.position.y, KnifeSpawnPoint.transform.position.z), 0.1f * 1);
       // }

   // }

    public void NextLevel()
    {
        Debug.Log("Next Level");
        if (currentCircle != null)
        {
            currentCircle.destroyMeAndAllKnives();
        }
        if (GameManager.Stage % 5 == 0)
        {
            GameManager.Stage++;
            StartCoroutine(OnBossFightEnd());

        }
        else
        {
            GameManager.Stage++;
            if (GameManager.Stage % 5 == 0)
            {
                StartCoroutine(OnBossFightStart());
            }
            else
            {
                Invoke("setupGame", .3f);
            }
        }
    }

    IEnumerator currentShowingAdsPopup;
    public void GameOver()
    {
        GameManager.isGameOver = true;

        if (usedAdContinue || !IsAdAvailable())
        {
            showGameOverPopup();
        }
        else
        {
            currentShowingAdsPopup = showAdPopup();
            StartCoroutine(currentShowingAdsPopup);
        }
    }
    public IEnumerator showAdPopup()
    {
        adsShowView.SetActive(true);
        adSocreLbl.text = GameManager.score + "";
        SoundManager.instance.PlayTimerSound();
        for (float i = 1f; i > 0; i -= 0.01f)
        {
            adTimerImage.fillAmount = i;
            yield return new WaitForSeconds(0.1f);
        }
        CancleAdsShow();
        SoundManager.instance.StopTimerSound();
    }
    public void OnShowAds()
    {
        doneWatchingAd = false;

        SoundManager.instance.StopTimerSound();
        SoundManager.instance.PlaybtnSfx();
        usedAdContinue = true;
        StopCoroutine(currentShowingAdsPopup);

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        AdmobController.instance.ShowRewardBasedVideo();
#else
        HandleRewardBasedVideoRewarded(null, null);
#endif
    }
    public void AdShowSucessfully()
    {
        adsShowView.SetActive(false);
        totalSpawnKnife--;
        GameManager.isGameOver = false;
        print(currentCircle.hitedKnife.Count);
        print(totalSpawnKnife);
        KnifeCounter.intance.setHitedKnife(totalSpawnKnife);
        if (KnifeSpawnPoint.childCount == 0)
        {
            StartCoroutine(GenerateKnife());
        }
    }
    public void CancleAdsShow()
    {
        SoundManager.instance.StopTimerSound();
        SoundManager.instance.PlaybtnSfx();
        StopCoroutine(currentShowingAdsPopup);
        adsShowView.SetActive(false);
        showGameOverPopup();
    }
    public void showGameOverPopup()
    {
        gameOverView.SetActive(true);
        //edited add next 5 lines to destroy gameobjects for next new session to begin
        currentCircle.destroyMeAndAllKnives();
        currentBow.DestroyMe();
        scoreView.SetActive(false);
        stageCounterView.SetActive(false);
        GameManager.isGameOver = true;

        gameOverSocreLbl.text = GameManager.score + "";
        gameOverStageLbl.text = "Stage " + GameManager.Stage;

        if (GameManager.score >= GameManager.HighScore)
        {
            GameManager.HighScore = GameManager.score;
            newBestScore.SetActive(true);
        }
        else
        {
            newBestScore.SetActive(false);
        }

        CUtils.ShowInterstitialAd();
    }
    public void OpenKnifeShop()
    {
        SoundManager.instance.PlaybtnSfx();
        //edited test revert later
        TestKnifeShop.intance.showShop();
    }
    //edited add whole fuction to open Bow Shop
    public void OpenbowShop()
    {
        SoundManager.instance.PlaybtnSfx();
        BowShop.intance.showShop();
    }
    public void RestartGame()
    {
        SoundManager.instance.PlaybtnSfx();
        //edited Scene name to the new one //edited commment next line and added 3 lines to hide mainmenu and restart game 
        //GeneralFunction.intance.LoadSceneByName("MyGameScene");
        gameOverView.SetActive(false);
        startGame();
    }
    public void BackToHome()
    {
        SoundManager.instance.PlaybtnSfx();
        //edited Scene name to the new one //edited again commented next line and added rest 4 lines to implement mainmenu popup and deactivate gameover view
        //GeneralFunction.intance.LoadSceneByName("MyHomeScene");
        mainMenuView.SetActive(true);
        gameOverView.SetActive(false);
    }
    public void FBClick()
    {
        SoundManager.instance.PlaybtnSfx();
        StartCoroutine(CROneStepSharing());
    }
    public void ShareClick()
    {
        SoundManager.instance.PlaybtnSfx();
        StartCoroutine(CROneStepSharing());
    }
    public void SettingClick()
    {
        SoundManager.instance.PlaybtnSfx();
        SettingUI.intance.showUI();
    }

    IEnumerator CROneStepSharing()
    {
        yield return new WaitForEndOfFrame();
        MobileNativeShare.ShareScreenshot("screenshot", "");
    }
}

[System.Serializable]
public class Bosses
{
    public string Bossname;
    public Circle BossPrefab;
}
