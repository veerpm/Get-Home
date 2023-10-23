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
    public int maxHits;
    public int currentHits;

    private void Start()
    {
        currentHits = maxHits;
    }
}
