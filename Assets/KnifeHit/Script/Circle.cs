

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Circle : MonoBehaviour {

	public int totalKnife=5;
	public List<RotationVariation> RandomRotation = new List<RotationVariation>();
	public List<LevelVariation> RandomLevels = new List<LevelVariation>();

	public ParticleSystem hitParticle,splashParticle;
	[Space(20)]
	public bool isBoss=false;
    //edited commented next 2 lines and added the list for the same
    //public Sprite woodSprite,blueWoodSprite;
    //public ParticleSystem WoodSplatParticle,BlueWoodSplatParticle;
    //public List<Sprite> WoodSprites;
    //public List<ParticleSystem> WoodSpriteParticles;
    int currentWoodSprite;
    public ParticleSystem WoodSplatParticle;

    [Space(20)]
	public bool isRandomClockWise=false;

	public List<Knife> hitedKnife= new List<Knife>();
	public AudioClip woodHitSfx,LasthitSfx;
	int currentRoationIndex=0;
	int currentLevelndex=0;
	// Use this for initialization
	float valueZ ;
	void Start () {
		if (!isBoss) {
            //edited commented next line and addes the rest of the instructions inside this if statement;
            //GetComponent<SpriteRenderer>().sprite=GameManager.Stage%10<5?woodSprite:blueWoodSprite;
            //currentWoodSprite = Random.Range(0, WoodSprites.Count);
            //GetComponent<SpriteRenderer>().sprite = WoodSprites[currentWoodSprite];
        }

		if (RandomRotation.Count > 0) {
			ApplyRotation ();
		}
		currentLevelndex = Random.Range (0,RandomLevels.Count);
		print ("Current Level"+currentLevelndex);
		if (RandomLevels [currentLevelndex].applePosibility > Random.value) {
			SpawnApple ();
		}
		SpawnKnife ();
	}

    public void SetWoodSprite(ParticleSystem spriteParticle)
    {
        if(!isBoss)
        {
            WoodSplatParticle = spriteParticle;
        }    
    }

	void ApplyRotation()
	{
		currentRoationIndex = (currentRoationIndex + 1) % RandomRotation.Count;
		LeanTween.rotateZ (gameObject, transform.localRotation.eulerAngles.z + RandomRotation [currentRoationIndex].z, RandomRotation [currentRoationIndex].time).setOnComplete (ApplyRotation).setEase (RandomRotation [currentRoationIndex].curve);
	}

	void SpawnApple()
	{
		foreach (float item in RandomLevels[currentLevelndex].AppleAngles) {
			GameObject tempApple = Instantiate<GameObject> (GamePlayManager.instance.ApplePrefab);
			tempApple.transform.SetParent (transform);
			setPosInCircle (transform, tempApple.transform,item, 0.28f, -90f);
			tempApple.transform.localScale = Vector3.one;
		}
	}
	void SpawnKnife()
	{
		foreach (float item in RandomLevels[currentLevelndex].KnifeAngles) {
            //edited to update the same selected knife on circle as default (lines 2)
            //GameObject tempKnife = Instantiate<GameObject> (GamePlayManager.instance.knifePrefab.gameObject);
            GameObject tempKnife = Instantiate<GameObject>(GameManager.selectedKnifePrefab.gameObject);

            tempKnife.transform.SetParent (transform);
			tempKnife.GetComponent<Knife> ().isHitted = true;
			tempKnife.GetComponent<Knife> ().isFire = true;
			tempKnife.GetComponents<BoxCollider2D> () [0].enabled = true;
			tempKnife.GetComponents<BoxCollider2D> () [1].enabled = true;
            //edited (transform, tempKnife.transform,item, 0.12f, 90f) to (transform, tempKnife.transform,item, 0.1f, 90f) position knifes already in circle 
            setPosInCircle(transform, tempKnife.transform,item, 0.07f, 90f);
            //edited Vector3 (0.65f, 0.65f, 0.65f) to Vector3 (1f, 1f, 1f)
            tempKnife.transform.localScale = new Vector3 (1f, 1f, 1f);
        }
	}
	void setPosInCircle(Transform circle,Transform obj,float angle,float spaceBetweenCircleAndObject,float objAngelOffset)
	{
		angle = angle + 90f;
		Vector2 offset = new Vector2(Mathf.Sin(angle*Mathf.Deg2Rad), Mathf.Cos(angle*Mathf.Deg2Rad)) * (circle.GetComponent<CircleCollider2D>().radius+spaceBetweenCircleAndObject);
		obj.localPosition = (Vector2)circle.localPosition + offset;
		obj.localRotation = Quaternion.Euler (0, 0, -angle+90f+objAngelOffset);
	}

	/*void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Knife")
		{			
			
			LeanTween.moveLocalY (gameObject, 0.1f, 0.05f).setLoopPingPong(1).setEaseInBounce();
			coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			coll.rigidbody.velocity = Vector2.zero;
			coll.gameObject.transform.SetParent (transform);
			coll.gameObject.GetComponent<Knife> ().isHitted = true;
			hitedKnife.Add (coll.gameObject.GetComponent<Knife> ());
			playParticle(coll.collider.transform.position,hitParticle);

			if (hitedKnife.Count >= totalKnife) {
				if (!GameManager.isGameOver) {
					StartCoroutine (RelaseAllKnife ());
					SoundManager.instance.PlaySingle (LasthitSfx);
				}
			} else {
				playParticle(GamePlayManager.instance.circleSpawnPoint.transform.position,splashParticle);
				SoundManager.instance.PlaySingle (woodHitSfx);
			}
			GameManager.score++;
		}
	}*/
	public  void OnKnifeHit(Knife k){
			
			k.rb.isKinematic = true;
			k.rb.velocity = Vector2.zero;
			k.transform.SetParent (transform);
			k.isHitted = true;
			hitedKnife.Add (k);

			playParticle(k.transform.position,hitParticle);
        //edited moveLocalX (gameObject, 0.1f, 0.05f) to moveLocalY (gameObject, 0.2f, 0.05f) (make the circle vibrate more and on y axis on hit) 
        LeanTween.moveLocalX (gameObject, 0.2f, 0.05f).setLoopPingPong(1);
		if (hitedKnife.Count >= totalKnife) {
				if (!GameManager.isGameOver) {
					StartCoroutine (RelaseAllKnife ());
                //edited add next 5 lines (4th line is original)
                #if UNITY_ANDROID && !UNITY_EDITOR
                    SoundManager.instance.LastHitSFX();
                #else
                    SoundManager.instance.PlaySingle (LasthitSfx);
                #endif                
				}
		}
        else {
				playParticle(GamePlayManager.instance.circleSpawnPoint.transform.position,splashParticle);
            //edited add next 5 lines (4th line is original)
            #if UNITY_ANDROID && !UNITY_EDITOR
                    SoundManager.instance.WoodHitSFX();
            #else
            SoundManager.instance.PlaySingle (woodHitSfx);
            #endif
			}
			GameManager.score = GameManager.score + GameManager.Stage;
		}


	public void playParticle(Vector3 pos,ParticleSystem _particle)
	{
		ParticleSystem tempParticle= Instantiate(_particle);
		tempParticle.transform.position = pos;
		tempParticle.Play ();

	}

	public IEnumerator RelaseAllKnife()
	{
		LeanTween.cancel (gameObject);
		if (!isBoss) {
            //edited this if statement
            //playParticle (transform.position, GameManager.Stage % 10 < 5 ? WoodSplatParticle : BlueWoodSplatParticle);
            playParticle(transform.position, WoodSplatParticle);
        } else {
            //edited this if statement
            playParticle (transform.position, WoodSplatParticle);
        }
        gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds (0.02f);
		foreach (Transform item in transform) {
			//print (item.name);
			if (item.transform.tag.Equals ("Knife")) {				
				item.GetComponents<BoxCollider2D> ()[0].enabled = false;
				item.GetComponents<BoxCollider2D> ()[1].enabled = false;

				item.GetComponent<Knife> ().rb.isKinematic = false;
				//item.GetComponent<Knife>().transform.parent = null;
				item.GetComponent<Knife> ().rb.gravityScale = 2.5f;
				item.GetComponent<Knife> ().rb.freezeRotation = false;
				item.GetComponent<Knife> ().rb.angularVelocity = Random.Range (-20f, 20f) * 35f;
				item.GetComponent<Knife> ().rb.AddForce (new Vector2 (Random.Range (-10f, 10f), Random.Range (3f, 10f)), ForceMode2D.Impulse);
				item.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder + 1;
				item.GetComponent<Knife> ().DestroyMe ();

			} else if (item.transform.tag.Equals ("Apple")) {
				item.GetComponent<Apple> ().rb.isKinematic = false;
				//item.transform.parent = null;
				item.GetComponent<Apple> ().rb.gravityScale = 2.5f;
				item.GetComponent<Apple> ().rb.freezeRotation = false;
				item.GetComponent<Apple> ().rb.angularVelocity = Random.Range (-20f, 20f) * 35f;
				item.GetComponent<Apple> ().rb.AddForce (new Vector2 (Random.Range (-6f, 6f), Random.Range (3f, 10f)), ForceMode2D.Impulse);
				item.GetComponent<CircleCollider2D> ().enabled = false;
				item.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder + 1;
			}
		}

		GetComponent<CircleCollider2D> ().enabled = false;
		yield return new WaitForSeconds (1.5f);
		GamePlayManager.instance.NextLevel ();

	}
	public void destroyMeAndAllKnives()
	{
		foreach (Knife item in hitedKnife) {
			if(item != null)
				Destroy (item.gameObject);
		}
		Destroy (gameObject);

	}

}

[System.Serializable]
public class RotationVariation{
	[Range(0f,2f)]
	public float time=0f;
	[Range(-180,180f)]
	public float z=0f;
	public AnimationCurve curve;
}

[System.Serializable]
public class LevelVariation{
	[Range(0f,1f)]
	public float applePosibility=0.5f;
	//[Header("Apple SpawnPointList")]
	public List<float> AppleAngles= new List<float>();
	//[Header("Knife SpawnPointList")]
	public List<float> KnifeAngles= new List<float>();
}
