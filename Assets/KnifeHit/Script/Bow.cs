using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    public void BowShake()
    {
        LeanTween.moveLocalX(gameObject, -0.4f, 0.05f).setLoopPingPong(1);
        //LeanTween.moveLocalX(this.gameObject, 0, 0.1f);
        Debug.Log("I work too");
    }
}
