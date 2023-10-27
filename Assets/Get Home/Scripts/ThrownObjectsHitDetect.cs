using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectsHitDetect : MonoBehaviour
{
    public bool thrown;
    // Start is called before the first frame update
    void Start()
    {
        thrown = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && thrown == true)
        {
            Debug.Log("Thrown Trash Can Hit!");
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(100);
        }
    }
}
