using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;public class EnemyHealth : MonoBehaviour{    public int maxHealth = 40;    protected int currentHealth;    public Animator animator;    public GameObject healthBar;    public GameObject damageText;    private Vector3 initialPosition;    protected bool keepActive = false; // used for child classes
    private bool isDead = false;

    //sound FX
    public AudioSource enemyHitSound;    public AudioSource enemyDieSound;    // Start is called before the first frame update    void Start()    {        if (tag == "Enemy")        {            healthBar.GetComponent<Slider>().maxValue = maxHealth;        }        SetFullHealth();        initialPosition = transform.position;    }    // Update is called once per frame    void Update()    {            }    public void TakeDamage(int damage)    {        if (damageText != null)        {            GameObject damageTextClone = Instantiate(damageText, transform.Find("Canvas"));            damageTextClone.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();            Destroy(damageTextClone, 1f);        }

        //sound FX
        enemyHitSound.Play();        currentHealth -= damage;        // Play Hurt Animation        animator.SetTrigger("Hurt");        // Die        DisplayHealth();        if (currentHealth <= 0)        {            Die();        }    }    void DisplayHealth()    {        if (tag == "Enemy")        {            healthBar.GetComponent<Slider>().value = currentHealth;        }    }    public void SetFullHealth()    {        currentHealth = maxHealth;        DisplayHealth();    }    public void resetPosition()    {        transform.position = initialPosition;    }    private IEnumerator Wait()    {        foreach (Behaviour comp in GetComponents<Behaviour>())        {            if (comp != GetComponent<Animator>())            {                comp.enabled = false;            }        }        yield return new WaitForSeconds(2);        GetComponent<Animator>().enabled = false;        GetComponent<SpriteRenderer>().enabled = false;        foreach (Transform child in transform)        {            child.gameObject.SetActive(false);        }    }    void Die()    {        animator.SetBool("IsDead", true);        if (!keepActive)        {            Wait();            StartCoroutine("Wait");        }    }    public void Alive()    {
        animator.SetBool("IsDead", false);        foreach (Behaviour comp in GetComponents<Behaviour>())        {            comp.enabled = true;        }        GetComponent<SpriteRenderer>().enabled = true;        foreach (Transform child in transform)        {            child.gameObject.SetActive(true);        }        SetFullHealth();    }    public bool IsDead()
    {
        if(currentHealth> 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }}