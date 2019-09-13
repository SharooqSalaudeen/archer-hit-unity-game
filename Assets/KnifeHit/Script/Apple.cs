/**
 * Apple.cs
 * Created by: #FreeBird#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

	public ParticleSystem splatApple;
	public SpriteRenderer Sprite;
	public AudioClip appleHitSfx;

	// Use this for initialization
	public Rigidbody2D rb;
	void Start () {
		rb = GetComponentInChildren<Rigidbody2D> ();    
		rb.isKinematic = true;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Knife")) {
            //if (!other.gameObject.GetComponent<Knife> ().isHitted) {
            //edited add next 5 lines (4th line is original)
#if UNITY_ANDROID && !UNITY_EDITOR
            SoundManager.instance.AppleHitSFX();
#else
            SoundManager.instance.PlaySingle(appleHitSfx);
#endif
				GameManager.Apple = GameManager.Apple + 2;
                //edited added leantween call
                LeanTween.scale(GamePlayManager.instance.AppleCounterView, new Vector3(1.3f, 1.3f, 1f), .1f).setOnComplete(() => {
                    LeanTween.scale(GamePlayManager.instance.AppleCounterView, new Vector3(1, 1, 1), .1f);
                });
                transform.parent = null;
				GetComponent<CircleCollider2D> ().enabled = false;
				Sprite.enabled = false;
				splatApple.Play ();
				Destroy (gameObject, 3f);
			//}
		}
	}
}


