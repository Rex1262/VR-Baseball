using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public GameObject hitEffect;
    private AudioSource hitSound;
    public TMPro.TMP_Text text;
    int hitScore = 0;
    int foulScore = 0;
    int ballScore = 0;
    int strikeScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        hitSound = GetComponent<AudioSource>();
        ChangeScore(0, 0, 0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ball"))
        {
            hitSound.Play();
            Instantiate(hitEffect, hit.transform.position, Quaternion.identity);
        }
        
        Debug.Log("Hit Object Name: " + hit.gameObject.name);
    }

    public void ChangeScore(int hit, int foul, int ball, int strike)
    {
        hitScore += hit;
        foulScore += foul;
        ballScore += ball;
        strikeScore += strike;
        text.text = "Hits: " + hitScore  + "\nFouls: " + foulScore+"\nBalls: "+ballScore + "\nStrikes: " + strikeScore + "\nTotal: " + (hitScore+strikeScore+foulScore+ballScore);
    }
}
