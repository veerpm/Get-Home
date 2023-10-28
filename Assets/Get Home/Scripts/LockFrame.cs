using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFrame : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public GameObject gameManager;
    public List<GameObject> enemies;

    private void Update()
    {
        bool noEnemies = true;
        foreach(GameObject enemy in enemies)
        {
            // check if remaining enemies
            if (!enemy.GetComponent<EnemyHealth>().IsDead())
            {
                noEnemies = false;
            }
        }

        // if all enemies are dead, release player & deactivate
        if (noEnemies)
        {
            unlockPlayer();
            this.gameObject.SetActive(false);
        }
    }

    // lock player if touches object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            lockPlayer();
        }
    }

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
        mainCamera.GetComponent<CameraMovement>().setFreeze(false);
        // unfreeze player's bounds
        player.GetComponent<Boundaries>().unFreeze();
        // new checkpoint
        gameManager.GetComponent<GamePause>().updateCheckpoint(player.transform.position);
    }
}
