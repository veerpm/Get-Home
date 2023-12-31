﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private Transform player;
    public float speed = 2;
    public float lineofSight;
    public float throwingRange;
    private float nextThrowTime;
    public float throwingCD;
    public GameObject throwable;
    public GameObject throwableRight;
    public GameObject throwSpotLeft;
    public GameObject throwSpotRight;
    private bool lookingRight = false;
    private Transform canvas;
    public Animator animator;
    public AudioSource throwSound;
    // Charging Stance 
    public int chargeDamage;
    private bool charging = false;
    private float nextChargeTime = 0;
    public float chargingCD;
    private float startTime;

    // Patrol/Charge
    public GameObject pointLeft;
    public GameObject pointHalfway;
    public GameObject pointRight;
    private bool onRightSide = true;

    // Scene controls
    [HideInInspector] public bool started;
    public GameObject gameManager;

    //SFX
    public AudioSource bossRushSound;
    private bool playedSound;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = transform.Find("Canvas");
        charging = false;
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        // don't do anything if haven't started yet
        if (!started)
        {
            return;
        }
        if (player.transform.position.x > transform.position.x && !lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            lookingRight = true;
        }
        // Player to the left of the enemy
        else if (player.transform.position.x < this.transform.position.x && lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            lookingRight = false;
        }
        ThrowingStance();

        // Start charging if offcd
        if (nextChargeTime < Time.time)
        {
            Charge();
        }

        // Keep charging while duration is active
        if (Time.time - startTime <= 1.2f)
        {

            Debug.Log("Start Charge");
            charging = true;

            if (transform.position.x > pointLeft.transform.position.x && onRightSide == true)
            {
                if (!playedSound)
                {
                    bossRushSound.Play();
                    playedSound = true;
                }
                transform.position = Vector2.MoveTowards(this.transform.position, pointLeft.transform.position, speed * Time.deltaTime);
                animator.SetTrigger("LandlordCharge");
            }
            else if (transform.position.x < pointRight.transform.position.x && onRightSide == false)
            {
                if (!playedSound)
                {
                    bossRushSound.Play();
                    playedSound = true;
                }
                transform.position = Vector2.MoveTowards(this.transform.position, pointRight.transform.position, speed * Time.deltaTime);
                animator.SetTrigger("LandlordCharge");
            }

            nextChargeTime = Time.time + chargingCD;

        }
        else if (Time.time - startTime > 1.1f)
        {
            //Debug.Log("End Charge");
            playedSound = false;
            charging = false;
            if (onRightSide)
            {
                onRightSide = false;
            }
            else
            {
                onRightSide = true;
            }
        }

    }

    void ThrowingStance()
    {

        // Throwing while planted! --> Start on the right then charge to the left and start throwing from the left!
        if (nextThrowTime < Time.time && charging == false)
        {
            animator.SetTrigger("LandlordThrow");
            //sound FX
            throwSound.Play();
            Instantiate(throwable, throwSpotLeft.transform.position, Quaternion.identity);
            throwSound.Play();
            Instantiate(throwableRight, throwSpotRight.transform.position, Quaternion.identity);
            nextThrowTime = Time.time + throwingCD;
        }

    }
    void Charge()
    {
        startTime = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("CHARGING:"+ charging);
        if (collider.gameObject.CompareTag("Player") && charging == true)
        {
            Debug.Log("This occured");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(chargeDamage);
        }
    }

    // called when boss has been beaten: stop everything from moving
    public void BossDied()
    {
        gameManager.GetComponent<BossEpilogue>().BossDieCinematic();
    }

    // called at the end of the boss' death animation (activates dialogue)
    public void EndingStart()
    {
        gameManager.GetComponent<BossEpilogue>().StartEndingDialogue();
    }
}
