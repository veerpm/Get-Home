using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBehaviour : MonoBehaviour
{
    public AudioSource drinkBeerSound;

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
            collider.GetComponent<PlayerHealth>().setFullHealth();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 2);
        }
    }
}
