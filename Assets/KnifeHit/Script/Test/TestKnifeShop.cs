using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class TestKnifeShop : MonoBehaviour
{
    public GameObject shopUIParent;
    public TestShopKnifeItem shopKnifePrefab;
    //edited 3 lines
    public Transform shopPageAppleContent;
    public Transform shopPageWatchContent;
    public Transform shopPageBuyContent;

    public Text unlockKnifeCounterLbl;
    //edited removed unlockRandomBtn
    //public Button unlockRandomBtn;
    public Button unlockBtnApple, unlockBtnWatchAds, AdsBtn;
    public Image selectedKnifeImageUnlock;
    public Image selectedKnifeImageLock;
    public GameObject knifeBackeffect1, knifeBackeffect2;
    public int UnlockPrice = 250, UnlockRandomPrice = 250, UnlockAdsAmount = 4;
    //edited 3 lines
    //public List<Knife> shopKnifeAppleList;
    //public List<Knife> shopKnifeWatchList;
    //public List<Knife> shopKnifeBuyList;
    public List<Knife> shopKnifeList;
    public static TestKnifeShop intance;
    public static TestShopKnifeItem selectedItem;
    public AudioClip onUnlocksfx, RandomUnlockSfx;
    List<TestShopKnifeItem> shopItems;
    TestShopKnifeItem selectedShopItem
    {
        get
        {
            return shopItems.Find((obj) => { return obj.selected; });
        }
    }
    void Start()
    {
        if (intance == null)
        {
            intance = this;
            //edited SetupShop() to AetupAppleShop() and addes 2 lines
            //SetupAppleShop();
            //SetupWatchShop();
            //SetupBuyShop();
            SetupShop();
        }
    }

    //added update for checking available ads to enable/disable buttons
    void Update()
    {
        if (!GamePlayManager.instance.IsAdAvailable())
        {
            unlockBtnWatchAds.interactable = false;
            AdsBtn.interactable = false;
        }
        else
        {
            unlockBtnWatchAds.interactable = true;
            AdsBtn.interactable = true;
        }
    }

    [ContextMenu("Clear PlayerPref")]
    void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("Add Apple")]
    void addApple()
    {
        GameManager.Apple += 500;
    }
    public void showShop()
    {
        shopUIParent.SetActive(true);
        if (!shopItems[GameManager.SelectedKnifeIndex].selected)
        {
            shopItems[GameManager.SelectedKnifeIndex].selected = true;
        }
        UpdateUI();

        CUtils.ShowInterstitialAd();
    }

    void SetupShop()
    {
        unlockBtnApple.GetComponentInChildren<Text>().text = UnlockPrice + "";
        unlockBtnWatchAds.GetComponentInChildren<Text>().text = UnlockAdsAmount + "";
        //unlockRandomBtn.GetComponentInChildren<Text>().text = UnlockRandomPrice + "";

        shopItems = new List<TestShopKnifeItem>();
        //edited list name
        //for (int i = 0; i < shopKnifeAppleList.Count; i++)
        for (int i = 0; i < 10; i++)
        {
            //edited name
            TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageAppleContent);
            temp.setup(i, this);
            temp.name = i + "";
            temp.itemType = "apple";
            shopItems.Add(temp);
        }
        //for (int i = 0; i < shopKnifeWatchList.Count; i++)
        for (int i = 10; i < 20; i++)
        {
            //edited name
            TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageWatchContent);
            temp.setup(i, this);
            temp.name = i + "";
            temp.itemType = "ads";
            //temp.adsAmount = UnlockAdsAmount;
            temp.adsLeft = UnlockAdsAmount;
            shopItems.Add(temp);
        }
        //for (int i = 0; i < shopKnifeBuyList.Count; i++)
        /*for (int i = 20; i < 25; i++)
        {
            //edited name
            TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageBuyContent);
            temp.setupBuy(i, this);
            temp.name = i + "";
            shopItems.Add(temp);
        }
        */
        shopItems[GameManager.SelectedKnifeIndex].OnClick();
    }


    //edited added ads call script
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
        }
#endif
    }

    public void UpdateUI()
    {
        selectedKnifeImageUnlock.sprite = selectedShopItem.knifeImage.sprite;
        selectedKnifeImageLock.sprite = selectedShopItem.knifeImage.sprite;
        selectedKnifeImageUnlock.gameObject.SetActive(selectedShopItem.KnifeUnlock);
        selectedKnifeImageLock.gameObject.SetActive(!selectedShopItem.KnifeUnlock);

        knifeBackeffect1.SetActive(selectedShopItem.KnifeUnlock);
        knifeBackeffect2.SetActive(selectedShopItem.KnifeUnlock);

        int unlockCount = 0;
        if (shopItems.FindAll((obj) => { return obj.KnifeUnlock; }) != null)
        {
            unlockCount = shopItems.FindAll((obj) =>
            {
                return obj.KnifeUnlock;
            }).Count;
        }
        //edited added line to count all the knifes
        int totalKnifes = shopKnifeList.Count;
        //edited name 2 lines and 2nd line changed from shopKnifeAppleList.Count to totalKnifes
        unlockKnifeCounterLbl.text = unlockCount + "/" + totalKnifes;
        if (unlockCount == totalKnifes)
        {
            unlockBtnApple.interactable = false;
            //unlockRandomBtn.interactable = false;
        }
        //edited list name
        GameManager.selectedKnifePrefab = shopKnifeList[GameManager.SelectedKnifeIndex];
        if (MainMenu.intance != null)
        {
            MainMenu.intance.selectedKnifeImage.sprite = GameManager.selectedKnifePrefab.GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void UnlockKnife()
    {
        if (unlockingRandom)
            return;

        if (GameManager.Apple < UnlockPrice)
        {
            Toast.instance.ShowMessage("Opps! Don't have enough apples");
            SoundManager.instance.PlaybtnSfx();
            return;
        }
        if (selectedShopItem.KnifeUnlock)
        {
            Toast.instance.ShowMessage("It's already unlocked!");
            SoundManager.instance.PlaybtnSfx();
            return;
        }
        GameManager.Apple -= UnlockPrice;
        selectedShopItem.KnifeUnlock = true;
        selectedShopItem.UpdateUIColor();
        GameManager.SelectedKnifeIndex = selectedShopItem.index;
        UpdateUI();
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.OnUnlockSFX();
#else
        SoundManager.instance.PlaySingle(onUnlocksfx);
#endif
    }
    bool unlockingRandom = false;
    public void UnlockRandomKnife()
    {
        if (GameManager.Apple < UnlockRandomPrice)
        {
            Toast.instance.ShowMessage("Opps! Don't have enough apples");
            SoundManager.instance.PlaybtnSfx();
            return;
        }
        if (unlockingRandom)
        {
            return;
        }
        StartCoroutine(UnlockRandomCoKnife());

    }
    IEnumerator UnlockRandomCoKnife()
    {
        unlockingRandom = true;
        List<TestShopKnifeItem> lockedItems = shopItems.FindAll((obj) => { return !obj.KnifeUnlock; });
        TestShopKnifeItem randomSelect = null;
        for (int i = 0; i < lockedItems.Count * 2; i++)
        {
            randomSelect = lockedItems[Random.Range(0, lockedItems.Count)];

            if (!randomSelect.selected)
            {
                randomSelect.selected = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.RandomUnlockSFX();
#else
                SoundManager.instance.PlaySingle(RandomUnlockSfx);
#endif
            }
            yield return new WaitForSeconds(.2f);
        }

        GameManager.Apple -= UnlockRandomPrice;
        randomSelect.KnifeUnlock = true;
        randomSelect.UpdateUIColor();
        GameManager.SelectedKnifeIndex = randomSelect.index;
        UpdateUI();
        unlockingRandom = false;
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.OnUnlockSFX();
#else
        SoundManager.instance.PlaySingle(onUnlocksfx);
#endif
    }

    //edited added the rest of the script
    private bool usedUnlockWatchAdsBtn = false;
    public void UnlockKnifeWatchAds()
    {
        if (unlockingRandom)
            return;

        if (selectedShopItem.KnifeUnlock)
        {
            unlockBtnWatchAds.interactable = false;
            Toast.instance.ShowMessage("It's already unlocked!");
            SoundManager.instance.PlaybtnSfx();
            return;
        }

        if (!GamePlayManager.instance.IsAdAvailable())
        {
            Toast.instance.ShowMessage("No ads available at the moment!");
            SoundManager.instance.PlaybtnSfx();
            return;
        }
        if (selectedShopItem.adsLeft > 0)
        {
            usedUnlockWatchAdsBtn = true;
            
            AdmobController.instance.ShowRewardBasedVideo();
            
        }

        
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        if (usedUnlockWatchAdsBtn)
        {
            usedUnlockWatchAdsBtn = false;
            selectedShopItem.adsLeft--;
            int adsleftcount = selectedShopItem.adsLeft;
            unlockBtnWatchAds.GetComponentInChildren<Text>().text = adsleftcount + "";

            if (selectedShopItem.adsLeft == 0)
            {
                selectedShopItem.adsLeft = -1;
                selectedShopItem.KnifeUnlock = true;
                selectedShopItem.UpdateUIColor();
                GameManager.SelectedKnifeIndex = selectedShopItem.index;
                UpdateUI();
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.OnUnlockSFX();
#else
                SoundManager.instance.PlaySingle(onUnlocksfx);
#endif
            }
        }
    }

    private void OnDisable()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
        }
#endif
    }

}