using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCanBehaviour : MonoBehaviour
{
    public GameObject player;
    public float dist = 3f;
    public float height = 2f;
    public int damage = 5;
    public int attackRate = 1;
    private float nextAttackTime = 0f;
    public Transform attackPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && Time.time >= nextAttackTime)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (WithinTriangle(enemy.transform.position))
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }

    }

    /* Returns true if point p lies inside triangle a-b-c */
    bool WithinTriangle(Vector3 enemyPos)
    {
        Vector2 v0 = new Vector2 (dist, height/2);
        Vector2 v1 = new Vector2 (dist, -height/2);
        Vector2 v2 = enemyPos - attackPoint.position;
        float dot00 = Vector2.Dot(v0, v0);
        float dot01 = Vector2.Dot(v0, v1);
        float dot02 = Vector2.Dot(v0, v2);
        float dot11 = Vector2.Dot(v1, v1);
        float dot12 = Vector2.Dot(v1, v2);
        float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
        return (u > 0.0f) && (v > 0.0f) && (u + v < 1.0f);
    }

    private void OnDrawGizmos()
    {
        //Transform attackPoint = player.transform.Find("SprayCanAttackPoint");
        Vector3 pos1 = new Vector3(attackPoint.position.x + dist, attackPoint.position.y + height / 2, attackPoint.position.z);
        Vector3 pos2 = new Vector3(attackPoint.position.x + dist, attackPoint.position.y - height / 2, attackPoint.position.z);
        Gizmos.DrawLine(attackPoint.position, pos1);
        Gizmos.DrawLine(attackPoint.position, pos2);
    }
}
