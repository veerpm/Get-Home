using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectsHitDetect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("thrown barell Hit!");
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(20);
        }
    }
}
