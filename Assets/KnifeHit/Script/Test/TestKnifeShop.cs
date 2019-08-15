using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestKnifeShop : MonoBehaviour
{
    public GameObject shopUIParent;
    public TestShopKnifeItem shopKnifePrefab;
    //edited 3 lines
    public Transform shopPageAppleContent;
    public Transform shopPageWatchContent;
    public Transform shopPageBuyContent;

    public Text unlockKnifeCounterLbl;
    public Button unlockNowBtn, unlockRandomBtn;
    public Image selectedKnifeImageUnlock;
    public Image selectedKnifeImageLock;
    public GameObject knifeBackeffect1, knifeBackeffect2;
    public int UnlockPrice = 250, UnlockRandomPrice = 250;
    //edited 3 lines
    public List<Knife> shopKnifeAppleList;
    public List<Knife> shopKnifeWatchList;
    public List<Knife> shopKnifeBuyList;

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
            SetupAppleShop();
            SetupWatchShop();
            SetupBuyShop();
            //SetupShop();
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
    /*
    void SetupShop()
    {
        unlockNowBtn.GetComponentInChildren<Text>().text = UnlockPrice + "";
        unlockRandomBtn.GetComponentInChildren<Text>().text = UnlockRandomPrice + "";

        shopItems = new List<TestShopKnifeItem>();
        //edited list name
        for (int i = 0; i < shopKnifeAppleList.Count; i++)
        {
            //edited name
            TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageAppleContent);
            temp.setupApple(i, this);
            temp.name = i + "";
            shopItems.Add(temp);
        }
        for (int i = 0; i < shopKnifeWatchList.Count; i++)
        {
            //edited name
            TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageWatchContent);
            temp.setupWatch(i, this);
            temp.name = i + "";
            shopItems.Add(temp);
        }
        for (int i = 0; i < shopKnifeBuyList.Count; i++)
        {
            //edited name
            TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageBuyContent);
            temp.setupBuy(i, this);
            temp.name = i + "";
            shopItems.Add(temp);
        }
        shopItems[GameManager.SelectedKnifeIndex].OnClick();
    }*/
    /    void SetupAppleShop()
        {
            unlockNowBtn.GetComponentInChildren<Text>().text = UnlockPrice + "";
            unlockRandomBtn.GetComponentInChildren<Text>().text = UnlockRandomPrice + "";

            shopItems = new List<TestShopKnifeItem>();
            //edited list name
            for (int i = 0; i < shopKnifeAppleList.Count; i++)
            {
                //edited name
                TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageAppleContent);
                temp.setupApple(i, this);
                temp.name = i + "";
                shopItems.Add(temp);
            }
            shopItems[GameManager.SelectedKnifeIndex].OnClick();
        }
        void SetupWatchShop()
        {
            unlockNowBtn.GetComponentInChildren<Text>().text = UnlockPrice + "";
            unlockRandomBtn.GetComponentInChildren<Text>().text = UnlockRandomPrice + "";

            shopItems = new List<TestShopKnifeItem>();
            //edited list name
            for (int i = 0; i < shopKnifeWatchList.Count; i++)
            {
                //edited name
                TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageWatchContent);
                temp.setupWatch(i, this);
                temp.name = i + "";
                shopItems.Add(temp);
            }
            shopItems[GameManager.SelectedKnifeIndex].OnClick();
        }

        void SetupBuyShop()
        {
            unlockNowBtn.GetComponentInChildren<Text>().text = UnlockPrice + "";
            unlockRandomBtn.GetComponentInChildren<Text>().text = UnlockRandomPrice + "";

            shopItems = new List<TestShopKnifeItem>();
            //edited list name
            for (int i = 0; i < shopKnifeBuyList.Count; i++)
            {
                //edited name
                TestShopKnifeItem temp = Instantiate<TestShopKnifeItem>(shopKnifePrefab, shopPageBuyContent);
                temp.setupBuy(i, this);
                temp.name = i + "";
                shopItems.Add(temp);
            }
            shopItems[GameManager.SelectedKnifeIndex].OnClick();
        }

        */
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
            unlockCount = shopItems.FindAll((obj) => {
                return obj.KnifeUnlock;
            }).Count;
        }
        //edited added line to count all the knifes
        int totalKnifes = shopKnifeAppleList.Count + shopKnifeWatchList.Count + shopKnifeBuyList.Count;
        //edited name 2 lines and 2nd line changed from shopKnifeAppleList.Count to totalKnifes
        unlockKnifeCounterLbl.text = unlockCount + "/" + totalKnifes;
        if (unlockCount == totalKnifes)
        {
            unlockNowBtn.interactable = false;
            unlockRandomBtn.interactable = false;
        }
        //edited list name
        GameManager.selectedKnifePrefab = shopKnifeAppleList[GameManager.SelectedKnifeIndex];
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
}
