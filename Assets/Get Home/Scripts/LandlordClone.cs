using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandlordClone : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject gameCamera;
    public float delayDialogue;

    // called at the end of the boss' entry animation
    public void DialogueStart()
    {
        StartCoroutine(DelayedDialogue());
    }

    public void StartCameraShake()
    {
        gameCamera.GetComponent<CameraMovement>().shake = true;
    }

    public void StopCameraShake()
    {
        gameCamera.GetComponent<CameraMovement>().shake = false;
    }

    IEnumerator DelayedDialogue()
    {
        yield return new WaitForSeconds(delayDialogue);
        gameManager.GetComponent<BossEpilogue>().StartIntroDialogue();
    }
}
