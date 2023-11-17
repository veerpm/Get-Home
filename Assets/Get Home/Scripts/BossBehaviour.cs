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
    private float chargingTime;
    private float startTime;

    // Patrol/Charge
    public GameObject pointA;
    public GameObject pointB;
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
        
        if(nextChargeTime < Time.time)
        {
            Debug.Log("Start Charge");
            startTime = Time.time;
            charging = true;

        }
        if (Time.time - startTime <= 1.0f)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position + new Vector3(0.7f, -0.5f, 0.0f), speed * Time.deltaTime);
            Debug.Log("End Charge");
            charging = false;
            nextChargeTime = Time.time + chargingCD;
        }

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

    void Charge()
    {
        startTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("This occured");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(20);
        }
    }

   /* IEnumerator Charging(Vector3 PLastKnownPos) {

        yield return new WaitForSeconds(5);
        transform.position = Vector2.MoveTowards(this.transform.position, PLastKnownPos, speed * Time.deltaTime);
        yield return new WaitForSeconds(5);
       
        
    }
   */
}
