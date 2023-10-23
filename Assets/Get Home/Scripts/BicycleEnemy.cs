using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleEnemy : MonoBehaviour
{
    public float speed = 1;
    public int attackDamage;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         transform.Translate(Vector3.left * speed * Time.deltaTime);

          if (transform.position.x < -9)
          {
                Destroy(gameObject);
          }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

}

