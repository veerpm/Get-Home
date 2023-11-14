using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCanBehaviour : MonoBehaviour
{
    public LayerMask enemyLayers;
    public float dist = 3f;
    public float height = 2f;
    public int damage = 5;
    public int attackRate = 2;
    public float maxDurability = 10f;
    public float currentDurability;
    private float nextAttackTime = 0f;
    public GameObject attackPoint;
    public ParticleSystem ps;
    public GameObject area;
    private List<Collider2D> enemies = new List<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();

    // Start is called before the first frame update
    void Start()
    {
        filter.SetLayerMask(enemyLayers);
        currentDurability = maxDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerMovement>().lookingRight && area.transform.localScale.x < 0)
        {
            var shape = ps.shape;
            shape.rotation = new Vector3(0, 90, 0);
            area.transform.localScale = new Vector2(-area.transform.localScale.x, area.transform.localScale.y);
        }
        else if (!GetComponent<PlayerMovement>().lookingRight && area.transform.localScale.x > 0)
        {
            var shape = ps.shape;
            shape.rotation = new Vector3(0, -90, 0);
            area.transform.localScale = new Vector2(-area.transform.localScale.x, area.transform.localScale.y);
        }

        area.transform.position = attackPoint.transform.position;
        if (Input.GetKey(KeyCode.E) && Time.time >= nextAttackTime)
        {
            Physics2D.OverlapCollider(area.GetComponent<Collider2D>(), filter, enemies);
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            nextAttackTime = Time.time + 1f / attackRate;
            ps.Play();
        }
        else if (!Input.GetKey(KeyCode.E))
        {
            ps.Stop();
        }
        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<WeaponManagement>().AdjustDurability();
        }
    }
}
