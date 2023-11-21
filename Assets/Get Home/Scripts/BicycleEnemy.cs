using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleEnemy : MonoBehaviour
{
    public float speed = 1;
    public int attackDamage;
    GameObject player;
    public bool toLeft = true;

    //SFX
    public AudioClip bikeLeftSound;
    public AudioClip bikeRightSound;
    public AudioSource bikeSounds;



    // Start is called before the first frame update
    void Start()
    {
        if (toLeft)
        {
            bikeSounds.PlayOneShot(bikeLeftSound);
        }
        else
        {
            bikeSounds.PlayOneShot(bikeRightSound);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toLeft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = false;

        }

        //if (transform.position.x < -15)
        //{
        //      Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

}

