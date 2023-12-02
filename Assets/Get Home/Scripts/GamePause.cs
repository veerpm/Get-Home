using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    public GameObject player;
    public GameObject pauseScreen;
    public GameObject deathScreen;
    public AudioSource music;
    public GameObject mainCamera;
    public GameObject pauseManager;
    public GameObject weaponHolder;

    public Vector3 checkpoint;

    private bool gamePaused = false;
    private bool dead = false;
    public bool dialogueOngoing = false;

    // boss fight
    public GameObject bossEnemy = null;
    private Vector2 bossPostition;
    private Vector2 randyPosition;

    // Start is called before the first frame update
    void Start()
    {
        // sets start position of boss and randy
        if (bossEnemy != null)
        {
            bossPostition = bossEnemy.transform.position;
            randyPosition = player.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // activate pause screen
        if (Input.GetKeyDown(KeyCode.P) && !dead && !dialogueOngoing)
        {
            gamePaused = !gamePaused;
            pauseScreen.SetActive(gamePaused);
            pauseManager.GetComponent<PauseMenu>().Activate(gamePaused);
            pause(gamePaused);
        }

        // if player died, wait for input to restart
        if (dead)
        {
            if (Input.GetKeyDown("space"))
            {
                // alive
                bringBackAlive();
            }
        }
    }

    public void bringBackAlive()
    {
        pause(false);

        // resets player position & health
        player.transform.position = checkpoint;
        player.GetComponent<PlayerHealth>().setFullHealth();

        // defreeze in case player was freezed in combat
        mainCamera.GetComponent<CameraMovement>().setFreeze(false);
        player.GetComponent<Boundaries>().unFreeze();

        // resets enemies position & health
        GameObject[] enemyStops = GameObject.FindGameObjectsWithTag("EnemyStop");
        foreach (GameObject enemyStop in enemyStops)
        {
            // dead-enemies are disabled & passed
            if (enemyStop.GetComponent<LockFrame>().EnemiesDefeated())
            {
                foreach(GameObject enemy in enemyStop.GetComponent<LockFrame>().enemies)
                {
                    enemy.SetActive(false);
                    continue;
                }
            }

            // bring back alive each enemy
            foreach (GameObject enemy in enemyStop.GetComponent<LockFrame>().enemies)
            {
                EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();

                if (healthScript != null) // checks if this enemy has healthscript
                {
                    healthScript.Alive();
                    healthScript.resetPosition();
                }
            }
        }

        // Some enemies are dead but not assigned to enemyStops:
        // to MAKE SURE they are TRULY DEAD, manually check and deactivate.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyHealth>() != null && enemy.GetComponent<EnemyHealth>().IsDead())
            {
                enemy.SetActive(false);
            }

            // used for resetting when dead in level2 bike area
            if (enemy.GetComponent<BicycleEnemySpawner>() != null || enemy.GetComponent<BicycleEnemy>() != null)
            {
                Level2Manager level2 = GameObject.Find("GameManager").GetComponent<Level2Manager>();
                if (level2 != null)
                {
                    if (level2.enemyStop1.GetComponent<LockFrame>().locked)
                    {
                        level2.enemyStop1.GetComponent<LockFrame>().locked = false;
                        level2.enemyStop1.GetComponent<LockFrame>().startTime = 0;
                        level2.enemyStop1.GetComponent<BoxCollider2D>().enabled = true;
                        level2.activated1 = false;
                    }
                    else if (level2.enemyStop2.GetComponent<LockFrame>().locked)
                    {
                        level2.enemyStop2.GetComponent<LockFrame>().locked = false;
                        level2.enemyStop2.GetComponent<LockFrame>().startTime = 0;
                        level2.enemyStop2.GetComponent<BoxCollider2D>().enabled = true;
                        level2.activated2 = false;
                    }
                    else if (!level2.enemyStop2.GetComponent<LockFrame>().EnemiesDefeated())
                    {
                        level2.activated3 = false;
                    }
                    level2.enemyStop1.GetComponent<LockFrame>().surviveSign.SetActive(false);
                    Destroy(enemy);
                }
            }
        }


        // (extra) brings back boss alive for lvl 3
        if(bossEnemy!= null)
        {
            bossEnemy.GetComponent<EnemyHealth>().SetFullHealth();

            // removes all beers left during boss battle

            GameObject[] beers = GameObject.FindGameObjectsWithTag("Beer");
            foreach (GameObject beer in beers)
            {
                Destroy(beer);
            }

            // resets player and boss position
            bossEnemy.transform.position = bossPostition;
            player.transform.position = randyPosition;

            // remove bicycles on screen

            GameObject[] bikeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject bikeEnemy in bikeEnemies)
            {
                if (bikeEnemy.GetComponent<BicycleEnemy>() != null)
                {
                    Destroy(bikeEnemy);
                }
            }
        }

        // bring back all weapons that were destroyed
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

        foreach (Transform child in weaponHolder.transform)
        {
            GameObject weapon = child.gameObject;
            print(weapon);
            if (player.GetComponent<WeaponManagement>().equippedWeapon != weapon)
            {
                weapon.SetActive(true);
            }
        }

        // restart sound
        music.Play();

        // remove death scree & unpause
        dead = false;
        deathScreen.SetActive(false);
    }

    public void setDead()
    {
        music.Stop();
        // show dead screen & pause
        dead = true;
        deathScreen.SetActive(true);
        pause(true);
    }

    public void pause(bool isPaused)
    {
        // All time related code is stopped if true
        if (isPaused) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }

        //player.GetComponent<PlayerMovement>().enabled = !isPaused;
        player.GetComponent<Animator>().enabled = !isPaused;

        // toggle player scripts
        var playerScripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in playerScripts)
        {
            // dialogues still need work during pauses
            if((script.GetType().Name == "DialogueManager") || (script.GetType().Name == "DialogueManagerV2"))
            {
                continue;
            }
            script.enabled = !isPaused;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // toggle if enemies are "alive" or not
        foreach (GameObject enemy in enemies)
        {
            // skip if enemy was already dead
            if (enemy.GetComponent<EnemyHealth>() != null && enemy.GetComponent<EnemyHealth>().IsDead())
            {
                continue;
            }

            // animate enemy
            if (enemy.GetComponent<Animator>() != null)
            {
                enemy.GetComponent<Animator>().enabled = !isPaused;
            }

            var enemyScripts = enemy.GetComponents<MonoBehaviour>();
            // toggle enemy's scripts
            foreach (var script in enemyScripts)
            {
                script.enabled = !isPaused;
            }
        }
    }

    public void updateCheckpoint(Vector3 newPos)
    {
        checkpoint = newPos;
    }

    public bool isPaused()
    {
        return gamePaused;
    }
}