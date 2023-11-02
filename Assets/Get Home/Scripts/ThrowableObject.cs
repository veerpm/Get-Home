using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private GameObject target;
    public float speed;
    Rigidbody2D throwableRB;
    public int damage;
    public bool thrown;
    public bool caught;
    private float catchTimer = 0;
    private float timeToCatch = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        caught = false;
        throwableRB = GetComponent<Rigidbody2D>();
        if(caught == false)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            Vector2 moveDirection = (target.transform.position - transform.position).normalized * speed;
            //Debug.Log(moveDirection);
            throwableRB.velocity = new Vector2(moveDirection.x, moveDirection.y);

        }

    }

    // Update is called once per frame
    void Update()
    {
        catchTimer += Time.deltaTime;

        if (caught == false && catchTimer > timeToCatch)
        {
            Destroy(this.gameObject);
            Debug.Log("Ball donezo");
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && caught == false)
        {
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        // Environmental Objects will act as a shield for the player against throwing stuffs
        if (collider.gameObject.tag == "Trash Can")
        {
            Destroy(this.gameObject);
        }
        if (collider.gameObject.tag == "Enemy" && caught == true)
        {
            Debug.Log("2 Number 9's");
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealth>().TakeDamage(damage * 3);
        }
    }
}
