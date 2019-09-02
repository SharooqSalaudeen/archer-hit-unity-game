using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShopKnifeItem : MonoBehaviour {

	public int index;
    //edited added variable
    public string itemType;
	public Image bgImage;
	public Image knifeImage;
	public GameObject selectIamge;
	public Color unlockKnifeBGColor, lockKnifeBGColor;
	public Color unlockKnifeColor, lockKnifeColor;
	public AudioClip unlockKnifesfx, lockKnifesfx,confirmKnifeSfx;
    public bool KnifeUnlock
	{
		get
		{	
				if (index == 0)
					return true;
				return  PlayerPrefs.GetInt ("KnifeUnlock_" + index, 0) == 1;
		}
		set
		{ 
		
			PlayerPrefs.SetInt ("KnifeUnlock_" + index, value?1:0);
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
			if (value) {
				if(TestKnifeShop.selectedItem!=null)
					TestKnifeShop.selectedItem.selected = false;

				TestKnifeShop.selectedItem = this;
			}
			selectIamge.SetActive (value);
		}
	}

	TestKnifeShop shopRef;
	Knife knifeRef;



	public	void setup (int i,TestKnifeShop shop) 
	{
		shopRef=shop;
		index = i;
		knifeRef = shop.shopKnifeList [index];
		knifeImage.sprite = knifeRef.GetComponent<SpriteRenderer> ().sprite;
		UpdateUIColor ();
	}

    public void OnClick()
	{
		if (KnifeUnlock && selected) {
			shopRef.shopUIParent.SetActive (false);
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.ConfirmKnifeSFX();
#else
            SoundManager.instance.PlaySingle(confirmKnifeSfx);
#endif
            TestKnifeShop.intance.unlockBtnWatchAds.GetComponentInChildren<Text>().text = 0 + "";
        }
		if (!selected) {
			selected = true;
			if(!KnifeUnlock )
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.LockKnifeSFX();
#else
                SoundManager.instance.PlaySingle(lockKnifesfx);
#endif
            TestKnifeShop.intance.unlockBtnWatchAds.GetComponentInChildren<Text>().text = adsLeft + "";
        } 
		if (KnifeUnlock) 
		{
			GameManager.SelectedKnifeIndex = index;
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.UnlockKnifeSFX();
#else
            SoundManager.instance.PlaySingle(unlockKnifesfx);
#endif
            TestKnifeShop.intance.unlockBtnWatchAds.GetComponentInChildren<Text>().text = 0 + "";
        }
        if (this.itemType == "apple")
        {
            TestKnifeShop.intance.unlockBtnApple.gameObject.SetActive(true);
            //TestKnifeShop.intance.unlockRandomBtn.gameObject.SetActive(true);
            TestKnifeShop.intance.unlockBtnWatchAds.gameObject.SetActive(false);
        }
        if (this.itemType == "ads")
        {
            TestKnifeShop.intance.unlockBtnApple.gameObject.SetActive(false);
            //TestKnifeShop.intance.unlockRandomBtn.gameObject.SetActive(false);
            TestKnifeShop.intance.unlockBtnWatchAds.gameObject.SetActive(true);
        }
		shopRef.UpdateUI ();

	}

    //edited addes variable and fucion
    public int adsLeft;
    public bool WatchAdsAmount()
    {
        if (adsLeft > 0)
        {
            adsLeft--;
        }

        if (adsLeft == 0)
        {
            return true;
        }
        else
            return false;
    }

    public void UpdateUIColor()
	{
		bgImage.color = KnifeUnlock ? unlockKnifeBGColor : lockKnifeBGColor;
		knifeImage.GetComponent<Mask> ().enabled = !KnifeUnlock;

		knifeImage.transform.GetChild(0).GetComponent<Image>().color = KnifeUnlock ? unlockKnifeColor : lockKnifeColor;
		knifeImage.transform.GetChild (0).gameObject.SetActive (!KnifeUnlock);
	}
}
