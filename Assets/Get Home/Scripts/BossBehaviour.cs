using System.Collections;
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
    private bool charging;
    private float nextChargeTime = 0;
    public float chargingCD;
    private float chargeDuration = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = transform.Find("Canvas");
        charging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x && !lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            //canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            lookingRight = true;
        }
        // Player to the left of the enemy
        else if (player.transform.position.x < this.transform.position.x && lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            //canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            lookingRight = false;
        }
        //ThrowingStance();
        ChargingStance();
    }

    void ThrowingStance()
    {
        charging = false;
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        // Throwing while planted! --> Start on the right then charge to the left and start throwing from the left!
        if (distanceFromPlayer < throwingRange && nextThrowTime < Time.time)
        {
            animator.SetTrigger("ThrowAttack");
            //sound FX
            throwSound.Play();
            Instantiate(throwable, throwSpotLeft.transform.position, Quaternion.identity);
            throwSound.Play();
            Instantiate(throwableRight, throwSpotRight.transform.position, Quaternion.identity);
            nextThrowTime = Time.time + throwingCD;
        }
    }

    void ChargingStance()
    {
        
        // Charge 
        if (nextChargeTime < Time.time)
        {
            Debug.Log("Start Charge");
            float elapsedTime = 0;
            elapsedTime += Time.time;
            transform.position = Vector2.MoveTowards(this.transform.position, player.position - new Vector3(-1,0,0), speed * Time.deltaTime);
            
            if(elapsedTime >= Time.time + 2.0f)
            {
                Debug.Log("End Charge");
                nextChargeTime = Time.time + chargingCD;
            }

            
        }
        // Recharge in between charge --> Time for player to smak
        else
        {
            // Play tired animation & panting sound
            Debug.Log("work pls :((");
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && charging == true)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(chargeDamage);
        }
    }
}
