using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite artWork;
    public new string name;
    public float lightAttackRange;
    public float heavyAttackRange;
    public int lightAttackDamage;
    public int heavyAttackDamage;
    public float lightAttackRate;
    public float heavyAttackRate;
    public int maxHits;
}
