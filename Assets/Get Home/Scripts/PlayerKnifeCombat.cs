using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeCombat : MonoBehaviour
{
    // For Public Variables, Change their values in the inspector since changing it here won't save it in the game!
    public Animator animator;
    public Transform KnifeLightAttackPoint;
    public Transform KnifeHeavyAttackPoint;
    public float KnifeKnifeAttackRange = 0.5f;
    public float KnifeKnifeAttackRangeHeavy = 1.0f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    public float lightAttackRate = 2f;
    public float nextLightAttackTime = 0f;
    //public Animator camAnim;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextLightAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LightAttack();
                nextLightAttackTime = Time.time + 1f / lightAttackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeavyAttack();
        }
    }

    void LightAttack()
    {
        //camAnim.SetTrigger("shake");
        animator.SetTrigger("KnifeLightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(KnifeLightAttackPoint.position, KnifeKnifeAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }

    void HeavyAttack()
    {
        //animator.SetTrigger("HeavyAttack");

        //Collider2D[]hitEnemies = Physics2D.OverlapCircleAll(attackPointHeavy.position, KnifeKnifeAttackRangeHeavy, enemyLayers);

        //foreach (Collider2D enemy in hitEnemies)
        //  {
        // enemy.GetComponent<Enemy>().TakeDamage(attackDamage * 2);
        //enemy.GetComponent<Barrel>().TakeDamage(attackDamage * 2);
        // }

    }

    void OnDrawGizmosSelected()
    {
        if (KnifeLightAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(KnifeLightAttackPoint.position, KnifeKnifeAttackRange);
    }
}


