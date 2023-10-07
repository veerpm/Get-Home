using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 40;
    protected int currentHealth;
    public Animator animator;

    private Vector3 initialPosition;
    protected bool keepActive = false; // used for child classes

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Play Hurt Animation
        animator.SetTrigger("Hurt");
        // Die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void setFullHealth()
    {
        currentHealth = maxHealth;
    }

    public void resetPosition()
    {
        transform.position = initialPosition;
    }

    void Die()
    {
        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<ThrowingEnemyFollowPlayer>().enabled = false;
        if (!keepActive)
        {
            this.enabled = false;
        }
    }

    public void Alive()
    {
        animator.SetBool("IsDead", false);

        GetComponent<Collider2D>().enabled = true;
        setFullHealth();
    }
}
