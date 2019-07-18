using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    [Range(0f, 2f)] public float stringRestX = 1f;
    [Range(0f, 4f)] public float stringRestTop = 2f;
    [Range(0f, 4f)] public float stringRestBottom = 2f;
    // the bowstring is a line renderer
    private List<Vector3> bowStringPosition;
    LineRenderer bowStringLinerenderer;

    // position of the line renderers middle part 
    Vector3 stringPullout;
    Vector3 stringRestPosition;

    // Start is called before the first frame update
    void Start()
    {
        stringRestPosition = new Vector3(-stringRestX, 0f, 0f);
        // setup the line renderer representing the bowstring
        bowStringLinerenderer = gameObject.AddComponent<LineRenderer>();
        //bowStringLinerenderer = gameObject.GetComponent<LineRenderer>();
        bowStringLinerenderer.SetVertexCount(3);
        bowStringLinerenderer.SetWidth(0.05F, 0.05F);
        bowStringLinerenderer.useWorldSpace = false;
        bowStringLinerenderer.material = Resources.Load("bowStringMaterial") as Material;
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-stringRestX, stringRestTop, 0f));
        bowStringPosition.Add(new Vector3(-stringRestX, 0f, 0f));
        bowStringPosition.Add(new Vector3(-stringRestX, -stringRestBottom, 0f));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);


    }

    public void StringPull()
    {
        stringPullout = new Vector3(-stringRestX - 1, 0f, 0f);
        bowStringLinerenderer.SetPosition(1, stringPullout);
    }

    public void StringRest()
    {
        stringPullout = stringRestPosition;
        bowStringLinerenderer.SetPosition(1, stringPullout);
    }

    /*public void drawBowString()
    {
        //bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, stringPullout);
        //bowStringLinerenderer.SetPosition(2, bowStringPosition[1]);
    }*/

    public void BowShake()
    {
        LeanTween.moveLocalX(gameObject, -0.4f, 0.05f).setLoopPingPong(1);
    }

}
