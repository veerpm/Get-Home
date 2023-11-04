using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public bool isAttacking;
    public GameObject tutorial;


    void IsAttacking()
    {
        isAttacking = true;
    }

    void IsNotAttacking()
    {
        isAttacking = false;
    }
}
