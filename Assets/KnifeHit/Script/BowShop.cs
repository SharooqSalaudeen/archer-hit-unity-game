using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowShop : MonoBehaviour
{
    public GameObject shopUIParent;
    public ShopBowItem shopBowPrefab;
    public Transform shopPageContent;
    public Text unlockBowCounterLbl;
    public Button unlockNowBtn, unlockRandomBtn, AdsBtn;
    public Image selectedBowImageUnlock;
    public Image selectedBowImageLock;
    public GameObject BowBackeffect1, BowBackeffect2;
    public int UnlockPrice = 250, UnlockRandomPrice = 250;
    public List<Bow> shopBowList;
    public static BowShop intance;
    public static ShopBowItem selectedItem;
    public AudioClip onUnlocksfx, RandomUnlockSfx;
    List<ShopBowItem> shopItems;
    ShopBowItem selectedShopItem
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
            SetupShop();
        }
    }

    void Update()
    {
        if (!GamePlayManager.instance.IsAdAvailable())
        {
            AdsBtn.interactable = false;
        }
        else
        {
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
        if (!shopItems[GameManager.SelectedBowIndex].selected)
        {
            shopItems[GameManager.SelectedBowIndex].selected = true;
        }
        UpdateUI();

        CUtils.ShowInterstitialAd();
    }
    void SetupShop()
    {
        unlockNowBtn.GetComponentInChildren<Text>().text = UnlockPrice + "";
        unlockRandomBtn.GetComponentInChildren<Text>().text = UnlockRandomPrice + "";

        shopItems = new List<ShopBowItem>();
        for (int i = 0; i < shopBowList.Count; i++)
        {
            ShopBowItem temp = Instantiate<ShopBowItem>(shopBowPrefab, shopPageContent);
            temp.setup(i, this);
            temp.name = i + "";
            shopItems.Add(temp);
        }

        shopItems[GameManager.SelectedBowIndex].OnClick();
    }
    public void UpdateUI()
    {
        selectedBowImageUnlock.sprite = selectedShopItem.BowImage.sprite;
        selectedBowImageLock.sprite = selectedShopItem.BowImage.sprite;
        selectedBowImageUnlock.gameObject.SetActive(selectedShopItem.BowUnlock);
        selectedBowImageLock.gameObject.SetActive(!selectedShopItem.BowUnlock);

        BowBackeffect1.SetActive(selectedShopItem.BowUnlock);
        BowBackeffect2.SetActive(selectedShopItem.BowUnlock);

        int unlockCount = 0;
        if (shopItems.FindAll((obj) => { return obj.BowUnlock; }) != null)
        {
            unlockCount = shopItems.FindAll((obj) => {
                return obj.BowUnlock;
            }).Count;
        }
        unlockBowCounterLbl.text = unlockCount + "/" + shopBowList.Count;
        if (unlockCount == shopBowList.Count)
        {
            unlockNowBtn.interactable = false;
            unlockRandomBtn.interactable = false;
        }

        GameManager.selectedBowPrefab = shopBowList[GameManager.SelectedBowIndex];
        if (MainMenu.intance != null)
        {
            MainMenu.intance.selectedBowImage.sprite = GameManager.selectedBowPrefab.GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void UnlockBow()
    {
        if (unlockingRandom)
            return;

        if (GameManager.Apple < UnlockPrice)
        {
            Toast.instance.ShowMessage("Opps! Don't have enough apples");
            SoundManager.instance.PlaybtnSfx();
            return;
        }
        if (selectedShopItem.BowUnlock)
        {
            Toast.instance.ShowMessage("It's already unlocked!");
            SoundManager.instance.PlaybtnSfx();
            return;
        }
        GameManager.Apple -= UnlockPrice;
        selectedShopItem.BowUnlock = true;
        selectedShopItem.UpdateUIColor();
        GameManager.SelectedBowIndex = selectedShopItem.index;
        UpdateUI();
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.OnUnlockSFX();
#else
        SoundManager.instance.PlaySingle(onUnlocksfx);
#endif

    }
    bool unlockingRandom = false;
    public void UnlockRandomBow()
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
        StartCoroutine(UnlockRandomCoBow());

    }
    IEnumerator UnlockRandomCoBow()
    {
        unlockingRandom = true;
        List<ShopBowItem> lockedItems = shopItems.FindAll((obj) => { return !obj.BowUnlock; });
        ShopBowItem randomSelect = null;
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
        randomSelect.BowUnlock = true;
        randomSelect.UpdateUIColor();
        GameManager.SelectedBowIndex = randomSelect.index;
        UpdateUI();
        unlockingRandom = false;
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.OnUnlockSFX();
#else
        SoundManager.instance.PlaySingle(onUnlocksfx);
#endif

    }
}
