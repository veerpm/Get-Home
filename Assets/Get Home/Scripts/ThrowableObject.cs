using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private GameObject target;
    public float speed;
    Rigidbody2D throwableRB;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        throwableRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDirection = (target.transform.position - transform.position).normalized * speed;
        //Debug.Log(moveDirection);
        throwableRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(this.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        // Environmental Objects will act as a shield for the player against throwing stuffs
        if (collider.gameObject.tag == "Trash Can")
        {
            Destroy(this.gameObject);
        }
    }
}
