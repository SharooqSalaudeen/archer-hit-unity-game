using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShopKnifeItem : MonoBehaviour {

	public int index;
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



	public	void setupApple (int i,TestKnifeShop shop) 
	{
		shopRef=shop;
		index = i;
        //edited list name
		knifeRef = shop.shopKnifeAppleList [index];
		knifeImage.sprite = knifeRef.GetComponent<SpriteRenderer> ().sprite;
		UpdateUIColor ();
	}
    public void setupWatch(int i, TestKnifeShop shop)
    {
        shopRef = shop;
        index = i;
        //edited list name
        knifeRef = shop.shopKnifeWatchList[index];
        knifeImage.sprite = knifeRef.GetComponent<SpriteRenderer>().sprite;
        UpdateUIColor();
    }
    public void setupBuy(int i, TestKnifeShop shop)
    {
        shopRef = shop;
        index = i;
        //edited list name
        knifeRef = shop.shopKnifeBuyList[index];
        knifeImage.sprite = knifeRef.GetComponent<SpriteRenderer>().sprite;
        UpdateUIColor();
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
		}
		if (!selected) {
			selected = true;
			if(!KnifeUnlock )
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.LockKnifeSFX();
#else
                SoundManager.instance.PlaySingle(lockKnifesfx);
#endif
		} 
		if (KnifeUnlock) 
		{
			GameManager.SelectedKnifeIndex = index;
#if UNITY_ANDROID && !UNITY_EDITOR
                SoundManager.instance.UnlockKnifeSFX();
#else
            SoundManager.instance.PlaySingle(unlockKnifesfx);
#endif
		}
		shopRef.UpdateUI ();

	}
	public void UpdateUIColor()
	{
		bgImage.color = KnifeUnlock ? unlockKnifeBGColor : lockKnifeBGColor;
		knifeImage.GetComponent<Mask> ().enabled = !KnifeUnlock;

		knifeImage.transform.GetChild(0).GetComponent<Image>().color = KnifeUnlock ? unlockKnifeColor : lockKnifeColor;
		knifeImage.transform.GetChild (0).gameObject.SetActive (!KnifeUnlock);
	}
}
