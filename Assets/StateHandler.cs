using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    private float nextSwitchTime;
    public float switchCD;
    string currentState;
    // Start is called before the first frame update
    void Start()
    {
        nextSwitchTime = Time.time + switchCD;
        currentState = "Ranged";
        GetComponent<MeleeEnemyFollowPlayer>().enabled = false;
        GetComponent<ThrowingEnemyFollowPlayer>().enabled = true;
    }

    // Update is called once per frame
    void Update()

    {
        Debug.Log("Time: " + Time.time);
        Debug.Log("NextSwtichTime: " + nextSwitchTime);

        if (nextSwitchTime <= Time.time && currentState == "Ranged")
        {
            Debug.Log("Yay1");
            GetComponent<ThrowingEnemyFollowPlayer>().enabled = false;
            GetComponent<MeleeEnemyFollowPlayer>().enabled = true;
            currentState = "Melee";
            nextSwitchTime = Time.time + switchCD;
        }

        if(nextSwitchTime <= Time.time && currentState == "Melee")
        {
            Debug.Log("Yay2");
            GetComponent<MeleeEnemyFollowPlayer>().enabled = false;
            GetComponent<ThrowingEnemyFollowPlayer>().enabled = true;
            currentState = "Throwing";
            nextSwitchTime = Time.time + switchCD;
        }
    }
}
