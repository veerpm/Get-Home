using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockFrame : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public GameObject gameManager;
    public List<GameObject> enemies;
    public GameObject goPrompt;
    public GameObject surviveSign = null;
    public Slider surviveSlider = null;

    private bool enemiesDefeated;
    private GameObject flag = null;

    public bool useTime = false;
    public bool locked;
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
        float timerValue = Time.time - startTime;

        if(surviveSign != null && timerValue < unlockTime && locked)
        {
            surviveSlider.value = 1-timerValue / unlockTime;
        }

        // if a time was used and is done, unlock player
        if (useTime && Time.time - startTime >= unlockTime && locked)
        {
            Debug.Log("time");
            unlockPlayer();
            locked = false;
            GetComponent<BoxCollider2D>().enabled = false;
            enemiesDefeated = true;
            goPrompt.GetComponent<GoPrompt>().Display();
            if (flag != null)
            {
                flag.SetActive(true);
            }

            // survive sign for bikers
            if (surviveSign != null)
            {
                surviveSign.SetActive(false);
            }
        }

        // only check if enemies have not been beaten
        else if (!useTime && !enemiesDefeated)
        {
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
        if (other.gameObject.tag == "Player" && !enemiesDefeated && !locked)
        {
            lockPlayer();
            locked = true;
            startTime = Time.time;
            // survive sign for bikers
            if(surviveSign != null)
            {
                surviveSign.SetActive(true);
            }
        }
    }

    public void lockPlayer()
    {
        // freeze camera
        mainCamera.GetComponent<CameraMovement>().setFreeze(true);
        // freeze player's position to camera bounds (add width so player doesn't clip)
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
