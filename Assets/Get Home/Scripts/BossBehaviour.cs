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
    private bool charging = false;
    private float nextChargeTime = 0;
    public float chargingCD;
    private float chargeDuration = 2.0f;
    private float chargingTime;
    private float startTime;

    // Patrol/Charge
    public GameObject pointLeft;
    public GameObject pointHalfway;
    public GameObject pointRight;
    private bool onRightSide = true;
    
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
        ThrowingStance();
        
        // Start charging if offcd
        if(nextChargeTime < Time.time)
        {
            
            Charge();
        }
    
        // Keep charging while duration is active
        if (Time.time - startTime <= 1.1f)
        {
            charging = true;
            //Debug.Log("Start Charge");
            if (transform.position.x > pointLeft.transform.position.x && onRightSide == true)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, pointLeft.transform.position, speed * Time.deltaTime);
            }
            else if(transform.position.x < pointRight.transform.position.x && onRightSide == false)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, pointRight.transform.position, speed * Time.deltaTime);
            }

            nextChargeTime = Time.time + chargingCD;

        }
        else if(Time.time - startTime > 1.0f)
        {
            //Debug.Log("End Charge");
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
        /*
        if (transform.position.x >= pointHalfway.transform.position.x && transform.position != pointRight.transform.position)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, pointRight.transform.position, speed * Time.deltaTime);
        }
        else if (transform.position.x < pointHalfway.transform.position.x && transform.position != pointLeft.transform.position)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, pointLeft.transform.position, speed * Time.deltaTime);
        }
        */
       
        // Throwing while planted! --> Start on the right then charge to the left and start throwing from the left!
        if (nextThrowTime < Time.time && charging == false)
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
    void Charge()
    {
        startTime = Time.time;   
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("CHARGING:"+ charging);
        if (collider.gameObject.CompareTag("Player") && charging == true)
        {
            Debug.Log("This occured");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(chargeDamage);
        }
    }

   /* IEnumerator Charging(Vector3 PLastKnownPos) {

        yield return new WaitForSeconds(5);
        transform.position = Vector2.MoveTowards(this.transform.position, PLastKnownPos, speed * Time.deltaTime);
        yield return new WaitForSeconds(5);
       
        
    }
   */
}
