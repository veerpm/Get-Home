using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBehaviour : MonoBehaviour
{
    public AudioSource drinkBeerSound;
    public int healValue = 50;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            drinkBeerSound.Play();
            collider.GetComponent<PlayerHealth>().setHealth(healValue);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            collider.transform.Find("BeerPs").GetComponent<ParticleSystem>().Play();
        }
    }
}
