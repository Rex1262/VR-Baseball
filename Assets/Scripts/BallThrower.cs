using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallThrower : MonoBehaviour
{
    public VideoScript video;
    public GameObject prefab;
    //public Button handButton;
    private float throwFrequencyPeriod = 5f;
    private float nextTime = 0;
    private int playerHand = 1;

    public float ballMass = 0.149f;

    public float speed;
    private float speedMS;
    private float massForce;
    private float spinRate = 2200f / 60;
    private const float PH0 = 1.2041f;//плотность воздуха в кг на m3
    private const float RADIUS = 0.037f;

    private Vector3 ballPosition = new Vector3(0, 0.3f, 19.4f);

    public int mode = 3;//0 - simple, 1 - fast, 2 - curve, 3 - slider
    private Vector3 velocityVector = new Vector3(0, 1f, -37f);
    private Rigidbody rb;
    private Vector3 magnusForce;
    private bool isOn = true;

    // Start is called before the first frame update
    void Awake()
    {
        
        speedMS = speed * 1.6f * 1000 / 3600;
        massForce = 0.8f * Mathf.PI * Mathf.PI * PH0 * RADIUS * RADIUS * RADIUS * spinRate;
        GameObject.Find("HandButton").GetComponentInChildren<TMPro.TMP_Text>().text = "RightHand";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= nextTime && isOn)
        {
            speedMS = speed * 1.6f * 1000 / 3600;
            Debug.Log("SpeedMS: " + speedMS);
            rb = ThrowBall();
            rb.velocity = DefineVelocity(MirroringScript.mode, velocityVector, speedMS);
            nextTime = Time.time + throwFrequencyPeriod;
        }
        if (rb != null)
        {
            magnusForce = MagnusForce(velocityVector, MirroringScript.mode);
            rb.AddForce(magnusForce * Time.deltaTime, ForceMode.Impulse);
        } 
    }

    public Rigidbody ThrowBall()
    {
        Rigidbody ball = Instantiate(prefab, ballPosition, Quaternion.identity).GetComponent<Rigidbody>();
        return ball;

    }

    private float CurveFunc(float speed)
    {
        return (1 / 2700) * speed * speed - 0.15f * speed + (329 / 30) - 1.55f;
    }

    private float FastFunc(float speed)
    {
        return 0.000007f * speed * speed - 0.08f * speed + 4.1f;
    }

    private float SliderFunc(float speed)
    {
        return 0.000148f * speed * speed - 0.15f * speed + 7;
    }

    private Vector3 DefineVelocity(int mode, Vector3 defaultVelocity, float speed)
    {
        switch(mode)
        {
            case 0:
                return new Vector3(0, 0.5f, -speed);
            case 1:
                return new Vector3(0, 1.5f, -speed);
            case 2:
                return new Vector3(0, 0.1f, -speed);//curveball
            case 3:
                return new Vector3(-1.7f*playerHand, 1f, -speed);
            default:
                return Vector3.zero;
        }
    }

    Vector3 MagnusForce(Vector3 velocity, int mode)
    {
        float forceX = velocity.z * massForce;
        float forceY = velocity.z * massForce;
        switch(mode)
        {
            case 1://done fast
                forceX *= playerHand*0; // no effect because *0
                forceY *= 0.6f;  // lifting up
                break;
            case 2://done curve
                forceX *= playerHand*0; // no effect because *0
                forceY *= -0.9f;  // pushing down
                break;
            case 3://done slider
                forceX *= -playerHand*1.5f;  //bending sideway
                forceY *= 0;  // lifting up
                break;
            default:
                return Vector3.zero;
        }
        Vector3 vect = new Vector3(forceX, forceY, 0);
        return vect;
    }

    public void SimpleModeChange()
    {
        MirroringScript.mode = 0;
        video.SetVideoClips();
    }

    public void FastBallModeChange()
    {
        MirroringScript.mode = 1;
        video.SetVideoClips();
    }

    public void CurveBallModeChange()
    {
        MirroringScript.mode = 2;
        video.SetVideoClips();
    }
    public void SliderModeChange()
    {
        MirroringScript.mode = 3;
        video.SetVideoClips();
    }

    public void ChangeSpeed(GameObject speedField)
    {
        var inputField = speedField.GetComponent<TMPro.TMP_InputField>();
        speed = float.Parse(inputField.text);
        speedMS = speed * 1.6f * 1000 / 3600;
    }

    public void ChangeHand()
    {
        VideoScript.ChangeScale(VideoScript.CurentTransform);
        if (MirroringScript.isRight)
        {
            MirroringScript.isRight = false;
          
          GameObject hand = GameObject.Find("HandButton");
         GameObject TextOfHand = hand.transform.Find("Text").gameObject;
         TextOfHand.GetComponent<TMPro.TMP_Text>().text = "LeftHand";

            playerHand = -1;
        }
        else if (!MirroringScript.isRight)
        {
           
            MirroringScript.isRight = true;
            GameObject hand = GameObject.Find("HandButton").gameObject;
         GameObject TextOfHand = hand.transform.Find("Text").gameObject;
         TextOfHand.GetComponent<TMPro.TMP_Text>().text = "RightHand";
            playerHand = 1;
        }
    }

    public void ThrowFrequencyChange(GameObject throwFrequencyField)
    {
        var inputField = throwFrequencyField.GetComponent<TMPro.TMP_InputField>();
        throwFrequencyPeriod = float.Parse(inputField.text);
    }

    public void EnableBall()
    {
        var toggle = GetComponentInChildren<Toggle>();
        isOn = toggle.isOn;
    }
}
