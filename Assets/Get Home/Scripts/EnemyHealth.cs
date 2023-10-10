using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 40;
    protected int currentHealth;
    public Animator animator;
    public GameObject healthBar;

    private Vector3 initialPosition;
    protected bool keepActive = false; // used for child classes

    // Start is called before the first frame update
    void Start()
    {
        if (tag == "Enemy")
        {
            healthBar.GetComponent<Slider>().maxValue = maxHealth;
        }
        SetFullHealth();
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
        DisplayHealth();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DisplayHealth()
    {
        if (tag == "Enemy")
        {
            Debug.Log("hey");
            healthBar.GetComponent<Slider>().value = currentHealth;
        }
    }

    public void SetFullHealth()
    {
        currentHealth = maxHealth;
        DisplayHealth();
    }

    public void resetPosition()
    {
        transform.position = initialPosition;
    }

    void Die()
    {
        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        if (!keepActive)
        {
            this.enabled = false;
        }
    }

    public void Alive()
    {
        animator.SetBool("IsDead", false);

        GetComponent<Collider2D>().enabled = true;
        SetFullHealth();
    }
}
