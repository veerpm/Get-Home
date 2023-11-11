using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpipenBehaviour : MonoBehaviour
{
    public int epipenDamageBoost;
    public int epipenDuration;
    public bool epipenActive;
    private float startTimer; // timer to track when epipen was activated
    public Image epipenOverlay;
    public Image epipenTimer;
    public AudioSource epiSound;

    private PlayerCombatMelee combatScript;

    // Start is called before the first frame update
    void Start()
    {
        epipenOverlay.enabled = false;
        epipenTimer.enabled = false;
        epipenTimer.transform.GetChild(0).GetComponent<Image>().enabled = false;
        combatScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatMelee>();
    }

    // Update is called once per frame
    void Update()
    {
        if (epipenActive && Time.time - startTimer > epipenDuration)
        {
            DisableEpipen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //sound FX
            epiSound.Play();

            combatScript.epiScript = this;
            combatScript.DisplayText("Epipen Active X2 Damage!!");
            combatScript.lightAttackDamage = combatScript.lightAttackDamage * epipenDamageBoost;
            combatScript.heavyAttackDamage = combatScript.heavyAttackDamage * epipenDamageBoost;
            startTimer = Time.time;
            epipenActive = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine("EpipenTimer");
        }
    }

    private IEnumerator EpipenTimer()
    {
        epipenOverlay.enabled = true;
        epipenTimer.enabled = true;
        epipenTimer.transform.GetChild(0).GetComponent<Image>().enabled = true;

        for (int i = epipenDuration; i > 0; i--)
        {
            epipenTimer.fillAmount = (float)i / epipenDuration;
            yield return new WaitForSeconds(1);
            epipenOverlay.enabled = !epipenOverlay.enabled;
        }

        DisableEpipen();
    }

    public void DisableEpipen()
    {
        epipenActive = false;
        StopCoroutine("EpipenTimer");
        epipenTimer.fillAmount = 0;
        epipenOverlay.enabled = false;
        epipenTimer.enabled = false;
        epipenTimer.transform.GetChild(0).GetComponent<Image>().enabled = false;
        combatScript.lightAttackDamage = combatScript.lightAttackDamage / epipenDamageBoost;
        combatScript.heavyAttackDamage = combatScript.heavyAttackDamage / epipenDamageBoost;
        combatScript.epiScript = null;
    }

}
