using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public Transform attackPointHeavy;
    public float attackRange = 0.5f;
    public float attackRangeHeavy = 1.0f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LightAttack();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeavyAttack();
        }
    }

    void LightAttack()
    {
        animator.SetTrigger("LightAttack");

        Collider2D[]hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }

    void HeavyAttack()
    {
        animator.SetTrigger("HeavyAttack");

        Collider2D[]hitEnemies = Physics2D.OverlapCircleAll(attackPointHeavy.position, attackRangeHeavy, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage * 2);
            //enemy.GetComponent<Barrel>().TakeDamage(attackDamage * 2);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


