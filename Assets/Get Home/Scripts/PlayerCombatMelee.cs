using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatMelee : MonoBehaviour
{
    public Animator animator;
    private Transform attackPoint;
    public LayerMask enemyLayers;
    float nextLightAttackTime = 0f;
    float nextHeavyAttackTime = 0f;
    float lastAttackTime = 0f;
    public GameObject display;
    WeaponManagement weaponManagement;
    private AnimationEvents events;

    //sound FX
    public AudioSource lightAttackSound1;
    public AudioSource lightAttackSound2;
    public AudioSource lightAttackSound3;
    public AudioSource heavyAttackSound1;
    public AudioSource heavyAttackSound2;
    public AudioSource heavyAttackSound3;
    public AudioSource comboSound;

    public EpipenBehaviour epiScript;

    // weapon stats
    float lightAttackRange;
    float heavyAttackRange;
    public int heavyAttackDamage;
    public int lightAttackDamage;
    float lightAttackRate;
    float heavyAttackRate;
    int maxHits;

    // combos
    List<string> attacksList = new List<string>();
    Dictionary<List<string>, int> combos = new Dictionary<List<string>, int>(); // maps order of attacks for combo to the attack damage after combo
    public bool comboActive;
    private float waitTime;

    // critical particles
    public ParticleSystem criticalHitParticles;

    void Start()
    {
        events = gameObject.GetComponent<AnimationEvents>();
        weaponManagement = GetComponent<WeaponManagement>();
        AddCombos();
    }

    // Update is called once per frame
    void Update()
    {
        weaponManagement.ResetAnim();

        if (comboActive && Time.time - lastAttackTime >= 2 )
        {
            DisableCombo();
        }

        if (Time.time >= nextLightAttackTime && Input.GetKeyDown(KeyCode.E) && !GetComponent<PickupObjects>().IsHolding())
        {
            animator.SetTrigger("LightAttack");
            nextLightAttackTime = Time.time + 1f / lightAttackRate;
            //if (!events.isAttacking)
            //{
            //    LightAttack();
            //    nextLightAttackTime = Time.time + 1f / lightAttackRate;
            //}
        }

        if (Time.time >= nextHeavyAttackTime && Input.GetKeyDown(KeyCode.Q) && !GetComponent<PickupObjects>().IsHolding())
        {
            animator.SetTrigger("HeavyAttack");
            nextHeavyAttackTime = Time.time + 1f / heavyAttackRate;
        }
    }


    void LightAttack()
    {
        //camAnim.SetTrigger("shake");

        //sound FX
        int randomSound = Random.Range(0, 2);
        if (randomSound < 1)
        {
            lightAttackSound1.Play();
        } else if (randomSound >= 2)
        {
            lightAttackSound2.Play();
        } else
        {
            lightAttackSound3.Play();
        }

        lightAttackSound1.Play();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, lightAttackRange, enemyLayers);

        if (hitEnemies.Length == 0)
        {
            return;
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                DealWCombo("Light Attack");
            }
            enemy.GetComponent<EnemyHealth>().TakeDamage(lightAttackDamage);

        }

        weaponManagement.AdjustDurability();

    }

    void HeavyAttack()
    {

        //sound FX
        int randomSound = Random.Range(0, 2);
        if (randomSound < 1)
        {
            heavyAttackSound1.Play();
        }
        else if (randomSound >= 2)
        {
            heavyAttackSound2.Play();
        }
        else
        {
            heavyAttackSound3.Play();
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, heavyAttackRange, enemyLayers);

        if (hitEnemies.Length == 0)
        {
            return;
        }


        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                DealWCombo("Heavy Attack");
            }
            enemy.GetComponent<EnemyHealth>().TakeDamage(heavyAttackDamage);

        }

        weaponManagement.AdjustDurability();
    }

    public void SetWeaponStats(GameObject weapon)
    {
        lightAttackRange = weapon.GetComponent<WeaponStats>().lightAttackRange;
        heavyAttackRange = weapon.GetComponent<WeaponStats>().heavyAttackRange;
        heavyAttackDamage = weapon.GetComponent<WeaponStats>().heavyAttackDamage;
        lightAttackDamage = weapon.GetComponent<WeaponStats>().lightAttackDamage;
        lightAttackRate = weapon.GetComponent<WeaponStats>().lightAttackRate;
        heavyAttackRate = weapon.GetComponent<WeaponStats>().heavyAttackRate;
        attackPoint = GameObject.Find(weapon.name + "AttackSpot").transform;

        if (epiScript != null)
        {
            lightAttackDamage = lightAttackDamage * epiScript.epipenDamageBoost;
            heavyAttackDamage = heavyAttackDamage * epiScript.epipenDamageBoost;
        }
    }


    // Epipen Logic

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, lightAttackRange);
    }


    // Combo Logic
    void AddCombos()
    {
        combos.Add(new List<string>() {
            "Light Attack",
            "Light Attack",
            "Heavy Attack",
            "Light Attack"}, 40);
    }

    void DealWCombo(string attack)
    {
        string S = "";

        if (comboActive)
        {
            DisableCombo();
            //return;
        }

        Debug.Log(Time.time - waitTime);
        Debug.Log(Time.time - lastAttackTime <= 2);

        if (lastAttackTime == 0 || Time.time - lastAttackTime <= 2 && Time.time - waitTime >= 1)
        {
            waitTime = 0;
            attacksList.Add(attack);
            foreach (string s in attacksList)
            {
                S += s;
            }
            Debug.Log(S);

            foreach (KeyValuePair<List<string>, int> c in combos)
            {
                if (attacksList.Count == 3 && c.Key.GetRange(0, 3).SequenceEqual(attacksList.GetRange(0, 3)) && epiScript == null)
                {
                    DisplayText(c.Key.Last<string>() + " For Combo!");
                }

                //else if (attacksList.Count == 3 &&  !c.Key.GetRange(0, 3).SequenceEqual(attacksList.GetRange(0, 3)))
                //{
                //    attacksList.Clear();
                //}

                //if (attacksList.Count == 4 && c.Key.GetRange(0, 3).SequenceEqual(attacksList.GetRange(1, 3)) && epiScript == null)
                //{
                //    DisplayText(c.Key.Last<string>() + " For Combo!");
                //}


                if (c.Key.SequenceEqual(attacksList) && epiScript == null)
                {
                    //sound FX
                    comboSound.Play();
                    // visual FX
                    criticalHitParticles.Play();
                    //StartCoroutine(comboVisualFX());
                    

                    lightAttackDamage = c.Value;
                    heavyAttackDamage = c.Value;
                    comboActive = true;
                    attacksList.Clear();
                    waitTime = Time.time;
                }
            }
            if (attacksList.Count == 4)
            {
                attacksList.Clear();
                waitTime = Time.time;
            }

        }

        else
        {
            attacksList.Clear();
            attacksList.Add(attack);
        }

        lastAttackTime = Time.time;
    }

    public void DisableCombo()
    {
        comboActive = false;
        SetWeaponStats(weaponManagement.EquippedWeapon);
        // remove visual FX
        //GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void DisplayText(string s)
    {
        GameObject displayClone = Instantiate(display, transform);
        displayClone.transform.GetChild(0).GetComponent<TextMesh>().text = s;
        Destroy(displayClone, 2f);
    }

    // change color of player during combo (unused)
    IEnumerator comboVisualFX()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0.92f, 0.016f, 1);
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}


