using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private LineRenderer lineRend;
    private Bat batBehav;
    private int vertexIndex;
    private bool drawLine = true;
    private bool hitBat = false;
    private bool hitFloor = false;
    private bool inStrikeZone = false;
    public TMPro.TMP_Text text;

    BallThrower bt;

    // Start is called before the first frame update
    void Awake()
    {
        bt = FindObjectOfType<BallThrower>();
        lineRend = gameObject.AddComponent<LineRenderer>();
        lineRend.loop = false;
        lineRend.positionCount = 0;
        lineRend.startWidth = 0.03f;
        //lineRend.startColor = Color.red;
        Material lineMat = new Material(Shader.Find("Unlit/Color"));
        lineMat.color = Color.white;
        switch (MirroringScript.mode)
        {
            case 1://fast
                lineMat.color = Color.green;
                break;
            case 2://curve
                lineMat.color = Color.red;
                break;
            case 3://slider
                lineMat.color = Color.blue;
                break;
            default:
                lineMat.color = Color.white;
                break;
        }


        lineRend.material = lineMat;
        vertexIndex = 0;
        batBehav = GameObject.Find("bat").GetComponent<Bat>();

        StartCoroutine(BallDestroy());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (drawLine)
        {
            lineRend.positionCount++;
            lineRend.SetPosition(vertexIndex, gameObject.transform.position);
            vertexIndex++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            hitFloor = true;
            drawLine = false;
        }
        if (collision.gameObject.tag == "Bat")
        {
            hitBat = true;
        }
    }

    IEnumerator BallDestroy()
    {
        yield return new WaitForSeconds(4f);
        if(hitBat){
            if(hitFloor)  batBehav.ChangeScore(1, 0, 0,0);
            else  batBehav.ChangeScore(1, 0, 0,0);
        }
        else
        {
            if (inStrikeZone)
            {
                batBehav.ChangeScore(0, 0, 0, 1);
            }
            else
            {
                batBehav.ChangeScore(0, 0, 1, 0);
            }
        }
        Object.Destroy(gameObject);
    }

    public void ConfirmStrikeZone()
    {
        inStrikeZone = true;
    }
}
