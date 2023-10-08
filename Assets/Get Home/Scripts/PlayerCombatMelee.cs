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
    public GameObject debug;
    private WeaponManagement weaponManagement;
    public GameObject equippedWeapon;
    //public Animator camAnim;
    private float lightAttackRange;
    private float heavyAttackRange;
    private int heavyAttackDamage;
    private int lightAttackDamage;
    private float lightAttackRate;
    private float heavyAttackRate;
    private int maxHits;
    private int epipenBoost;


    void Start()
    {
        weaponManagement = GetComponent<WeaponManagement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextLightAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LightAttack();
                nextLightAttackTime = Time.time + 1f / lightAttackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeavyAttack();
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
    }

    private IEnumerator EpipenTimer()
    {
        lightAttackDamage = lightAttackDamage * epipenBoost;
        for (int i = 5; i > 0; i--)
        {
            debug.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        debug.GetComponent<Text>().text = "0";
        //lightAttackDamage = lightAttackDamage / epipenBoost;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Epipen")
        {
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


