using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private GameObject target;
    public float speed;
    Rigidbody2D throwableRB;
    // Start is called before the first frame update
    void Start()
    {
        throwableRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDirection = (target.transform.position - transform.position).normalized * speed;
        Debug.Log(moveDirection);
        throwableRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(10); ;
        }
    }
}
