using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFrame : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public void lockPlayer()
    {
        // freeze camera
        mainCamera.GetComponent<CameraMovement>().setFreeze(true);
        // freeze player's position to camera bounds
        float leftBound = mainCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        float rightBound = mainCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
        float width = player.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
        player.GetComponent<Boundaries>().Freeze(leftBound + width, rightBound - width);
    }

    public void unlockPlayer()
    {
        // unfreeze camera
        mainCamera.GetComponent<CameraMovement>().setFreeze(true);
        // unfreeze player's bounds
        player.GetComponent<Boundaries>().unFreeze();
    }
}
