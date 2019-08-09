using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBowItem : MonoBehaviour
{

    public int index;
    public Image bgImage;
    public Image BowImage;
    public GameObject selectIamge;
    public Color unlockBowBGColor, lockBowBGColor;
    public Color unlockBowColor, lockBowColor;
    public AudioClip unlockKnifesfx, lockKnifesfx, confirmKnifeSfx;
    public bool BowUnlock
    {
        get
        {
            if (index == 0)
                return true;
            return PlayerPrefs.GetInt("BowUnlock_" + index, 0) == 1;
        }
        set
        {

            PlayerPrefs.SetInt("BowUnlock_" + index, value ? 1 : 0);
        }
    }
    public bool selected
    {
        get
        {
            return selectIamge.activeSelf;
        }
        set
        {
            if (value)
            {
                if (BowShop.selectedItem != null)
                    BowShop.selectedItem.selected = false;

                BowShop.selectedItem = this;
            }
            selectIamge.SetActive(value);
        }
    }

    BowShop shopRef;
    Bow BowRef;
    public void setup(int i, BowShop shop)
    {
        shopRef = shop;
        index = i;
        BowRef = shop.shopBowList[index];
        BowImage.sprite = BowRef.GetComponent<SpriteRenderer>().sprite;
        UpdateUIColor();
    }
    public void OnClick()
    {
        if (BowUnlock && selected)
        {
            shopRef.shopUIParent.SetActive(false);
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.ConfirmKnifeSFX();
#else
            SoundManager.instance.PlaySingle(confirmKnifeSfx);
#endif
        }
        if (!selected)
        {
            selected = true;
            if (!BowUnlock)
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.LockKnifeSFX();
#else
                SoundManager.instance.PlaySingle(lockKnifesfx);
#endif
        }
        if (BowUnlock)
        {
            GameManager.SelectedBowIndex = index;
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.UnlockKnifeSFX();
#else
            SoundManager.instance.PlaySingle(unlockKnifesfx);
#endif
        }
        shopRef.UpdateUI();

    }
    public void UpdateUIColor()
    {
        bgImage.color = BowUnlock ? unlockBowBGColor : lockBowBGColor;
        BowImage.GetComponent<Mask>().enabled = !BowUnlock;

        BowImage.transform.GetChild(0).GetComponent<Image>().color = BowUnlock ? unlockBowColor : lockBowColor;
        BowImage.transform.GetChild(0).gameObject.SetActive(!BowUnlock);
    }
}
