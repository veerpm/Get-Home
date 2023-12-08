using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject healthBar;
    public GameObject healthDisplay;
    public AnimationEvents events;
    private PlayerCombatMelee combatScript;
    private bool dead = false;
    public GameObject beer;
    private bool spawnedBeer;

    //soundFX
    public AudioSource getHitSound;
    public AudioSource dieSound;

    public GameObject gameManager;
    public GameObject gameCamera;     // shake FX

    // Start is called before the first frame update
    void Start()
    {
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        events = GetComponent<AnimationEvents>();
        combatScript = GetComponent<PlayerCombatMelee>();
        setFullHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level_Boss" && currentHealth <= 50 && !spawnedBeer)
        {
            SpawnBeer();
        }

        if (!GameObject.FindGameObjectWithTag("Beer"))
        {
            spawnedBeer = false;
        }
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
            if (combatScript.comboActive)
            {
                combatScript.DisableCombo();
            }
            if (combatScript.epiScript != null)
            {
                combatScript.epiScript.DisableEpipen();
            }

            // disable spray particle
            GetComponent<SprayCanBehaviour>().ps.Stop();

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
            StartCoroutine(ShakeScreen());
        }

    }

    void DisplayHealth()
    {
        healthBar.GetComponent<Slider>().value = currentHealth;
        healthDisplay.GetComponent<Text>().text = currentHealth.ToString("00");
    }

    public void setHealth(int health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth || currentHealth < 0)
        {
            currentHealth = maxHealth;
        }
        DisplayHealth();
    }

    public void setFullHealth()
    {
        setHealth(maxHealth);
        dead = false;
    }

    public bool IsDead()
    {
        return dead;
    }

    private void SetDeath()
    {
        gameManager.GetComponent<GamePause>().setDead();
    }

    private void SpawnBeer()
    {
        spawnedBeer = true;
        float x = Random.Range(0.05f, 0.95f);
        float y = Random.Range(0.05f, 0.5f);
        Vector3 pos = new Vector3(x, y, 10.0f);
        pos = Camera.main.ViewportToWorldPoint(pos);
        Instantiate(beer, pos, Quaternion.identity);
    }

    // shake screen when getting hit
    IEnumerator ShakeScreen()
    {
        gameCamera.GetComponent<CameraMovement>().shake = true;
        yield return new WaitForSeconds(0.25f);
        gameCamera.GetComponent<CameraMovement>().shake = false;
    }
}