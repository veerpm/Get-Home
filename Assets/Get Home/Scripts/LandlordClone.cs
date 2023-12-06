using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandlordClone : MonoBehaviour
{
    public GameObject gameManager;

    // called at the end of the boss' entry animation
    public void DialogueStart()
    {
        gameManager.GetComponent<BossEpilogue>().StartIntroDialogue();
    }
}
