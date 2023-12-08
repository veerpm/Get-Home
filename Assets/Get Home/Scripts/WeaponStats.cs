using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float lightAttackRange;
    public float heavyAttackRange;
    public int lightAttackDamage;
    public int heavyAttackDamage;
    public float lightAttackRate;
    public float heavyAttackRate;
    public float maxHits;
    public float currentHits;
    public Vector3 initialPostion;

    private void Start()
    {
        currentHits = maxHits;
        initialPostion = transform.position;
    }

    public void ResetAsNew()
    {
        currentHits = maxHits;
        transform.position = initialPostion;
    }
}
