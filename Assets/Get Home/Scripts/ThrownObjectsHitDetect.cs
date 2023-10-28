using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectsHitDetect : MonoBehaviour
{
    public bool thrown;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        thrown = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && thrown == true)
        {
            Debug.Log("Thrown Trash Can Hit!");
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
