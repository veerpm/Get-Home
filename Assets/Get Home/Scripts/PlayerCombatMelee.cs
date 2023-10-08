using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatMelee : MonoBehaviour
{
    public GameObject equippedWeapon;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private float nextLightAttackTime = 0f;
    private WeaponManagement weaponManagement;
    //public Animator camAnim;

    private float lightAttackRange;
    private float heavyAttackRange;
    private int heavyAttackDamage;
    private int lightAttackDamage;
    private float lightAttackRate;
    private float heavyAttackRate;
    private int maxHits;
    public int epipenBoost;


    void Start()
    {
        weaponManagement = Object.FindObjectOfType<WeaponManagement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextLightAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<PlayerMovement>().enabled = false; 
                LightAttack();
                GetComponent<PlayerMovement>().enabled = true;
                nextLightAttackTime = Time.time + 1f / lightAttackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeavyAttack();
        }

    }

    // put in pickupweapon script


    public void SetWeaponStats(GameObject weapon)
    {
        equippedWeapon = weapon;
        lightAttackRange = equippedWeapon.GetComponent<WeaponStats>().lightAttackRange;
        heavyAttackRange = equippedWeapon.GetComponent<WeaponStats>().heavyAttackRange;
        heavyAttackDamage = equippedWeapon.GetComponent<WeaponStats>().heavyAttackDamage;
        lightAttackDamage = equippedWeapon.GetComponent<WeaponStats>().lightAttackDamage;
        lightAttackRate = equippedWeapon.GetComponent<WeaponStats>().lightAttackRate;
        heavyAttackRate = equippedWeapon.GetComponent<WeaponStats>().heavyAttackRate;
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

        Debug.Log(lightAttackDamage);

        equippedWeapon.GetComponent<WeaponStats>().maxHits--;

        if (equippedWeapon.GetComponent<WeaponStats>().maxHits <= 0 && equippedWeapon.name != "Fists")
        {
            //Debug.Log("Done");
            weaponManagement.EquippedWeapon = GameObject.Find("Fists");
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

    // 

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, lightAttackRange);
    }
}


