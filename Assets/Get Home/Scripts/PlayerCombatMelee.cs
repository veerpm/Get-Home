using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatMelee : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private float nextLightAttackTime = 0f;
    public GameObject display;
    private WeaponManagement weaponManagement;
    public GameObject equippedWeapon;
    public AnimationEvents events;
    //public Animator camAnim;

    // weapon stats
    private float lightAttackRange;
    private float heavyAttackRange;
    private int heavyAttackDamage;
    private int lightAttackDamage;
    private float lightAttackRate;
    private float heavyAttackRate;
    private int maxHits;

    public int epipenDamageBoost;
    public int epipenTimer;
    bool epipenActive;
    private float startTimer; // timer to track when epipen was activated


    void Start()
    {
        events = gameObject.GetComponent<AnimationEvents>();
        weaponManagement = GetComponent<WeaponManagement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextLightAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E) && !events.isAttacking)
            {
                LightAttack();
                nextLightAttackTime = Time.time + 1f / lightAttackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeavyAttack();
        }

        if (epipenActive && Time.time - startTimer > epipenTimer)
        {
            lightAttackDamage = lightAttackDamage / epipenDamageBoost;
            epipenActive = false;
        }
    }


    void LightAttack()
    {
        //camAnim.SetTrigger("shake");
        animator.SetTrigger("LightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, lightAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log(enemy.name);
            enemy.GetComponent<EnemyHealth>().TakeDamage(lightAttackDamage);
        }

        weaponManagement.EquippedWeapon.GetComponent<WeaponStats>().maxHits--;

        weaponManagement.DisplayWeaponDurability();

        Debug.Log(lightAttackDamage);

        if (weaponManagement.EquippedWeapon.GetComponent<WeaponStats>().maxHits <= 0 && weaponManagement.EquippedWeapon != weaponManagement.defaultWeapon)
        {
            Debug.Log("Done");
            weaponManagement.EquippedWeapon = weaponManagement.defaultWeapon;
        }

    }

    void HeavyAttack()
    {
        //animator.SetTrigger("HeavyAttack");

        //Collider2D[]hitEnemies = Physics2D.OverlapCircleAll(attackPointHeavy.position, KnifeKnifeAttackRangeHeavy, enemyLayers);

        //foreach (Collider2D enemy in hitEnemies)
        //  {
        // enemy.GetComponent<Enemy>().TakeDamage(attackDamage * 2);
        //enemy.GetComponent<Barrel>().TakeDamage(attackDamage * 2);
        // }

    }

    public void SetWeaponStats(GameObject weapon)
    {
        equippedWeapon = weapon;
        lightAttackRange = equippedWeapon.GetComponent<WeaponStats>().lightAttackRange;
        heavyAttackRange = equippedWeapon.GetComponent<WeaponStats>().heavyAttackRange;
        heavyAttackDamage = equippedWeapon.GetComponent<WeaponStats>().heavyAttackDamage;
        lightAttackDamage = equippedWeapon.GetComponent<WeaponStats>().lightAttackDamage;
        lightAttackRate = equippedWeapon.GetComponent<WeaponStats>().lightAttackRate;
        heavyAttackRate = equippedWeapon.GetComponent<WeaponStats>().heavyAttackRate;
        if (epipenActive)
        {
            lightAttackDamage = lightAttackDamage * epipenDamageBoost;
        }
    }

    private IEnumerator EpipenTimer()
    {
        for (int i = epipenTimer; i >= 0; i--)
        {
            display.GetComponent<Text>().text = "Epipen Active: " + i.ToString();
            yield return new WaitForSeconds(1);
        }
        display.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Epipen")
        {
            lightAttackDamage = lightAttackDamage * epipenDamageBoost;
            startTimer = Time.time;
            epipenActive = true;
            collision.gameObject.SetActive(false);
            StartCoroutine("EpipenTimer");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, lightAttackRange);
    }
}


