using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotholeBehaviour : MonoBehaviour
{
    private float rx = 3.12f;
    private float ry = 0.51f;
    private float x;
    private float y;
    private float plx;
    private float ply;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        plx = player.transform.position.x;
        ply = player.transform.position.y - 0.8f; // 0.8f to get position of foot
        if (Mathf.Pow((plx - x), 2) / Mathf.Pow(rx * transform.localScale.x, 2) + Mathf.Pow((ply - y), 2) / Mathf.Pow(ry * transform.localScale.y, 2) <= 1)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(player.GetComponent<PlayerHealth>().maxHealth);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 point1 = new Vector3(transform.position.x+ rx * transform.localScale.x, transform.position.y,transform.position.z);
        Vector3 point2 = new Vector3(transform.position.x, transform.position.y+ ry * transform.localScale.y, transform.position.z);
        Gizmos.DrawLine(transform.position, point1);
        Gizmos.DrawLine(transform.position, point2);
    }
}
