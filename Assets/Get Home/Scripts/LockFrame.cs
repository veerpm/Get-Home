using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFrame : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public GameObject gameManager;
    public List<GameObject> enemies;
    public GameObject goPrompt;

    private bool enemiesDefeated;
    private GameObject flag = null;

    public bool useTime = false;
    public float unlockTime;
    private float startTime;

    private void Start()
    {
        enemiesDefeated = false;
        if(this.gameObject.transform.childCount > 0)
        {
            flag = this.gameObject.transform.GetChild(0).gameObject;
        }
    }

    private void Update()
    {
        if (useTime && Time.time - startTime >= unlockTime)
        {
            Debug.Log("time");
            unlockPlayer();
            if (flag != null)
            {
                flag.SetActive(true);
            }
        }

        // only check if enemies have not been beaten
        else if (!useTime && !enemiesDefeated)
        {
            Debug.Log("enemy");
            bool noEnemies = true;
            foreach (GameObject enemy in enemies)
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
                enemiesDefeated = true;
                goPrompt.GetComponent<GoPrompt>().Display();
                // display flag (if we are a checkpoint)
                if(flag != null)
                {
                    flag.SetActive(true);
                }
            }
        }
    }

    // lock player if touches object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && (useTime && Time.time - startTime < unlockTime || !enemiesDefeated))
        {
            lockPlayer();
            startTime = Time.time;
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

        // also freeze/unfreeze enemies
        if (TryGetComponent<LockEnemies>(out LockEnemies script))
        {
            script.Lock();
        }
    }

    public void unlockPlayer()
    {
        // unfreeze camera
        mainCamera.GetComponent<CameraMovement>().setFreeze(false);
        // unfreeze player's bounds
        player.GetComponent<Boundaries>().unFreeze();
        // new checkpoint (flag's  or player's position)
        if (flag != null)
        {
            gameManager.GetComponent<GamePause>().updateCheckpoint(flag.transform.position);
        }
        else
        {
            gameManager.GetComponent<GamePause>().updateCheckpoint(player.transform.position);
        }

        // also freeze/unfreeze enemies
        if (TryGetComponent<LockEnemies>(out LockEnemies script))
        {
            script.Unlock();
        }
    }

    public bool EnemiesDefeated()
    {
        return enemiesDefeated;
    }

    public void setEnemiesDefeated(bool value)
    {
        enemiesDefeated = value;
    }
}
