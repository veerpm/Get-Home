using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleEnemy : MonoBehaviour
{
    public float speed = 1;
    public int attackDamage;
    GameObject player;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        float y = Random.Range(0.4f, -4f);
        transform.position = new Vector3(8f, y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -9)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

}
