using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectsHitDetect : MonoBehaviour
{
    public bool thrown;
    public bool caught;
    public int damage;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        thrown = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && thrown == true)
        {
            audioSource.Play();
            transform.position = new Vector3(0, 40f, 0);
            Destroy(this.gameObject, 2);
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
