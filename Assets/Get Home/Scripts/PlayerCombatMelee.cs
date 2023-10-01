using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatMelee : MonoBehaviour
{

    [SerializeField] Weapon[] weaponsArray;
    private Weapon currentWeapon;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private float nextLightAttackTime = 0f;
    //public Animator camAnim;

    void Start()
    {

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
            if (weapon.name == selectedWeapon.name)
            {
                currentWeapon = weapon;
            }
        }
    }

    void LightAttack()
    {
        //camAnim.SetTrigger("shake");
        animator.SetTrigger("KnifeLightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, currentWeapon.lightAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(currentWeapon.lightAttackDamage);
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

    //void OnDrawGizmosSelected()
    //{
    //    if (attackPoint == null)
    //        return;
    //    Gizmos.DrawWireSphere(attackPoint.position, currentWeapon.lightAttackRange);
    //}
}


