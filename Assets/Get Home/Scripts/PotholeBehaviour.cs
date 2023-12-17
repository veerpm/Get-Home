using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotholeBehaviour : MonoBehaviour
{
    public float rx;
    public float ry;
    private Vector2 centre;
    public float offset;
    private GameObject player;
    public Sprite open;
    public Sprite closed;
    bool played = false;


    //SFX
    public AudioSource beartrapSound;

    // Start is called before the first frame update
    void Start()
    {
        centre = new Vector2(transform.position.x, transform.position.y + offset);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        float plx;
        float ply;
        plx = player.transform.position.x;
        ply = player.transform.position.y - 0.8f; // 0.8f to get position of foot
        if (Mathf.Pow((plx - centre.x), 2) / Mathf.Pow(rx, 2) + Mathf.Pow((ply - centre.y), 2) / Mathf.Pow(ry, 2) <= 1)
        {
            if (!played) {
                beartrapSound.Play();
                played = true;
            }
            player.GetComponent<PlayerHealth>().TakeDamage(player.GetComponent<PlayerHealth>().maxHealth);
            GetComponent<SpriteRenderer>().sprite = closed;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = open;
            played = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 centre = new Vector2(transform.position.x, transform.position.y + offset);
        Vector3 point1 = new Vector3(centre.x+ rx, centre.y);
        Vector3 point2 = new Vector3(centre.x, centre.y + ry);
        //Gizmos.DrawLine(centre, point1);
        //Gizmos.DrawLine(centre, point2);
        Gizmos.DrawWireSphere(centre, rx);
        Gizmos.DrawWireSphere(centre, ry);
    }
}
