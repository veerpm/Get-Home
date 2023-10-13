using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 1.0f;
    public float lineofSight = 5.0f;
    public float attackRange;
    private float nextAttackTime;
    public float AttackCD;
    public int attackDamage;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineofSight && distanceFromPlayer > attackRange)
        {
            //Debug.Log("This works Melee!");
            transform.position = Vector2.MoveTowards(this.transform.position, player.position + new Vector3(0.2f, 0.2f, 0.0f), speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= attackRange && nextAttackTime < Time.time)
        {
            animator.SetTrigger("Enemy1Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Melee Hit!");
                enemy.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + AttackCD;
        }
    }
    /*
    private void meleeAttack(float distanceFromPlayer, float player_x_position, float throwing_enemy_x_position)
    {
        if (distanceFromPlayer < lineofSight && distanceFromPlayer > attackRange)
        {
            //Debug.Log("This works!");
            transform.position = Vector2.MoveTowards(this.transform.position, player.position + new Vector3(0.7f, -0.5f, 0.0f), speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= attackRange && nextAttackTime < Time.time)
        {
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                //Debug.Log(enemy.name);
                enemy.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }

        }
    }
    */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineofSight);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}
