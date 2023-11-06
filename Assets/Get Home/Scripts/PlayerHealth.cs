using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject healthBar;
    public GameObject healthDisplay;
    public AnimationEvents events;
    private PlayerCombatMelee combatScript;
    private bool dead = false;

    //soundFX
    public AudioSource getHitSound;
    public AudioSource drinkBeerSound;
    public AudioSource dieSound;

    public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        events = gameObject.GetComponent<AnimationEvents>();
        combatScript = GetComponent<PlayerCombatMelee>();
        setFullHealth();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !dead)
        {
            //sound FX
            dieSound.Play();

            currentHealth = 0;
            animator.SetTrigger("Dead");
            events.isAttacking = false;
            combatScript.DisableCombo();
            if (combatScript.epipenActive)
            {
                combatScript.DisableEpipen();
            }
            // Disable movement
            combatScript.enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            dead = true;
            DisplayHealth();
        }
        else if (currentHealth > 0)
        {
            //sound FX
            getHitSound.Play();
            DisplayHealth();
        }

    }

    void DisplayHealth()
    {
        healthBar.GetComponent<Slider>().value = currentHealth;
        healthDisplay.GetComponent<Text>().text = currentHealth.ToString("00");
    }

    public void setFullHealth()
    {
        currentHealth = maxHealth;
        dead = false;
        DisplayHealth();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Beer")
        {
            drinkBeerSound.Play();
            currentHealth = maxHealth;
            DisplayHealth();
            collider.gameObject.SetActive(false);
        }
    }

    private void SetDeath()
    {
        gameManager.GetComponent<GamePause>().setDead();
    }
}