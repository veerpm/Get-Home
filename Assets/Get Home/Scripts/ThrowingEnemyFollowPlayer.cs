using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingEnemyFollowPlayer : MonoBehaviour
{
    private Transform player;
    private float speed = 2;
    public float lineofSight = 5.0f;
    public float throwingRange;
    public float escapeRange;
    private float nextThrowTime;
    public float throwingCD = 1f;
    public GameObject throwable;
    public GameObject throwSpot;
    private bool lookingRight = false;
    private Transform canvas;
    public Animator animator;

    private Vector3 escapeVector = new Vector3(2, 0, 0);

    //sound FX
    public AudioSource throwSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = transform.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        // Player to the right of the enemy
        if (player.transform.position.x > transform.position.x && !lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            escapeVector = new Vector3(-2, 0, 0);
            lookingRight = true;
        }
        // Player to the left of the enemy
        else if (player.transform.position.x < this.transform.position.x && lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            Debug.Log("Rocket!!!");
            escapeVector = new Vector3(2, 0, 0);
            lookingRight = false;
        }

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        // Run towards 
        if (distanceFromPlayer < lineofSight && distanceFromPlayer > throwingRange)
        {
            //Debug.Log("This works!");
            transform.position = Vector2.MoveTowards(this.transform.position, player.position + new Vector3(0.7f, -0.5f, 0.0f), speed * Time.deltaTime);
            animator.SetBool("Walking", true);
        }
        // Throwly Poly
        else if (escapeRange < distanceFromPlayer && distanceFromPlayer < throwingRange && nextThrowTime < Time.time)
        {
            animator.SetTrigger("ThrowAttack");
            //sound FX
            throwSound.Play();

            Instantiate(throwable, throwSpot.transform.position, Quaternion.identity);
            nextThrowTime = Time.time + throwingCD;
            animator.SetBool("Walking", false);
        }
        // Skaddadle
        else if (distanceFromPlayer < escapeRange)
        {
            Debug.Log("THIS CODE EXISTS!" + escapeVector);
            transform.position = Vector2.MoveTowards(this.transform.position, this.transform.position + escapeVector, (speed / 2) * Time.deltaTime);
            animator.SetBool("Walking", true);
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineofSight);
        Gizmos.DrawWireSphere(transform.position, throwingRange);
    }

}