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
    private float currentDuration;
    public Image epipenOverlay;
    public Image epipenTimer;
    public AudioSource epiSound;
    public GameObject player;

    private PlayerCombatMelee combatScript;

    // Start is called before the first frame update
    void Start()
    {
        epipenOverlay.enabled = false;
        epipenTimer.enabled = false;
        currentDuration = epipenDuration;
        epipenTimer.transform.GetChild(0).GetComponent<Image>().enabled = false;
        combatScript = player.GetComponent<PlayerCombatMelee>();
    }

    // Update is called once per frame
    void Update()
    {
        if (epipenActive)
        {
            epipenTimer.enabled = true;
            epipenTimer.transform.GetChild(0).GetComponent<Image>().enabled = true;
            currentDuration -= Time.deltaTime;
            epipenTimer.fillAmount = currentDuration / epipenDuration;
            if (currentDuration <= 0)
            {
                DisableEpipen();
            }
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
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine("EpipenOverlay");
        }
    }

    private IEnumerator EpipenOverlay()
    {
        epipenOverlay.enabled = true;

        for (int i = epipenDuration; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            epipenOverlay.enabled = !epipenOverlay.enabled;
        }
    }

    public void DisableEpipen()
    {
        epipenActive = false;
        StopCoroutine("EpipenOverlay");
        epipenTimer.fillAmount = 0;
        epipenOverlay.enabled = false;
        epipenTimer.enabled = false;
        epipenTimer.transform.GetChild(0).GetComponent<Image>().enabled = false;
        combatScript.lightAttackDamage = combatScript.lightAttackDamage / epipenDamageBoost;
        combatScript.heavyAttackDamage = combatScript.heavyAttackDamage / epipenDamageBoost;
        combatScript.epiScript = null;
    }

    public void ResetAsNew()
    {
        currentDuration = epipenDuration;
    }

}
