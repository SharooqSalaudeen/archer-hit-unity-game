using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    [Range(0f, 2f)] public float stringRestX = 1f;
    [Range(0f, 4f)] public float stringRestTop = 2f;
    [Range(0f, 4f)] public float stringRestBottom = 2f;
    public Material Material;
    Color materialColor = new Color(0.6f, 0.5f, 0.6f);
    // the bowstring is a line renderer
    private List<Vector3> bowStringPosition;
    LineRenderer bowStringLinerenderer;

    // position of the line renderers middle part 
    Vector3 stringPullout;
    //Vector3 stringRestPosition;
    public GameObject stringPullPoint;

    // Start is called before the first frame update
    void Start()
    {
        //stringRestPosition = new Vector3(-stringRestX, 0f, 0f);
        // setup the line renderer representing the bowstring
        bowStringLinerenderer = gameObject.AddComponent<LineRenderer>();
        //bowStringLinerenderer = gameObject.GetComponent<LineRenderer>();
        bowStringLinerenderer.SetVertexCount(3);
        bowStringLinerenderer.SetWidth(0.05F, 0.05F);
        bowStringLinerenderer.useWorldSpace = false;
        //bowStringLinerenderer.material = Resources.Load("bowStringMaterial") as Material;
        //bowStringLinerenderer.material = Material;
        bowStringLinerenderer.material = new Material(Shader.Find("Sprites/Default"));
        bowStringLinerenderer.material.color = materialColor;
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-stringRestX, stringRestTop, 0f));
        bowStringPosition.Add(new Vector3(-stringRestX, 0f, 0f));
        bowStringPosition.Add(new Vector3(-stringRestX, -stringRestBottom, 0f));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
        bowStringLinerenderer.sortingLayerName = "String";
        //edited added line for tempPoint
        stringPullPoint.transform.position = new Vector3(0f, 0f, 0f);


    }

    void Update()
    {
        stringPullout = new Vector3(-stringRestX - stringPullPoint.transform.position.x, 0f, 0f);
        bowStringLinerenderer.SetPosition(1, stringPullout);
    }



    public void StringPull()
    {
        //adeed line
        LeanTween.moveLocalX(stringPullPoint, 1f, 0.05f);

        //stringPullout = new Vector3(-stringRestX - 1, 0f, 0f);
        //bowStringLinerenderer.SetPosition(1, stringPullout);
    }

    public void StringRest()
    {
        LeanTween.moveLocalX(stringPullPoint, 0f, 0.05f);
        //stringPullout = stringRestPosition;
        //bowStringLinerenderer.SetPosition(1, stringPullout);
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

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

}
