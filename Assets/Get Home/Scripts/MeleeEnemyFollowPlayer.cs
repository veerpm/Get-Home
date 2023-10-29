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
    private bool lookingRight = false;
    private Transform canvas;

    //sound FX
    public AudioSource meleeAttackSound;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.Find("Canvas");
    }
  
    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x && !lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            lookingRight = true;
        }
        else if(player.transform.position.x <this.transform.position.x &&lookingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            canvas.localScale = new Vector2(-canvas.localScale.x, canvas.localScale.y);
            lookingRight = false;
        }
      
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineofSight && distanceFromPlayer > attackRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position + new Vector3(0.2f, 0.2f, 0.0f), speed * Time.deltaTime);
            animator.SetBool("Walking", true);
        }
        else if (distanceFromPlayer <= attackRange && nextAttackTime < Time.time)
        {
            animator.SetTrigger("Enemy1Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                //sound FX
                meleeAttackSound.Play();

                Debug.Log("Melee Hit!");
                enemy.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + AttackCD;
            animator.SetBool("Walking", false);
        }
        
    }
  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineofSight);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}
