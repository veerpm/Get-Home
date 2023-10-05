using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatMelee : MonoBehaviour
{

    [SerializeField] Weapon[] weaponsArray;
    public Weapon currentWeapon;
    public Weapon defaultWeapon;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private float nextLightAttackTime = 0f;
    int maxHits;
    //public Animator camAnim;

    void Start()
    {
        currentWeapon = defaultWeapon;
        //maxHits = currentWeapon.maxHits;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextLightAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LightAttack();
                nextLightAttackTime = Time.time + 1f / currentWeapon.lightAttackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeavyAttack();
        }

    }

    public void setWeapon(GameObject selectedWeapon)
    {
        foreach (Weapon weapon in weaponsArray)
        {
            if (weapon.name + "Game" == selectedWeapon.name)
            {
                currentWeapon = weapon;
            }
        }
        maxHits = currentWeapon.maxHits;
    }

    void LightAttack()
    {
        //camAnim.SetTrigger("shake");
        animator.SetTrigger("LightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, currentWeapon.lightAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log(enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(currentWeapon.lightAttackDamage);
        }

        Debug.Log(currentWeapon);

        maxHits--;

        if (maxHits <= 0 && currentWeapon != defaultWeapon)
        {
            Debug.Log("Done");
            currentWeapon = defaultWeapon;
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
        if (attackPoint == null || currentWeapon == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, currentWeapon.lightAttackRange);
    }
}


