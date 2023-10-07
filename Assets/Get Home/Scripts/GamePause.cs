using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public GameObject player;
    public GameObject pauseScreen;
    public GameObject deathScreen;

    public Vector3 checkpoint = new Vector3(0, 0, 0);

    private bool gamePaused = false;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // activate pause screen
        if (Input.GetKeyDown(KeyCode.P) && !dead)
        {
            gamePaused = !gamePaused;
            pauseScreen.SetActive(gamePaused);
            pause(gamePaused);
        }

        // if player died, wait for input to restart
        if (dead)
        {
            if (Input.anyKeyDown)
            {
                // resets player position & health
                player.transform.position = checkpoint;
                player.GetComponent<PlayerHealth>().setFullHealth();
                
                // resets enemies position & health
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();
                    healthScript.Alive();
                    healthScript.resetPosition();
                }

                // alive
                setDeath(false);
            }
        }

        // check if player is dead
        if (player.GetComponent<PlayerHealth>().currentHealth <= 0 && !dead)
        {
            setDeath(true);
        }

    }

    void setDeath(bool isDead)
    {
        // show player screen & update script
        dead = isDead;
        deathScreen.SetActive(dead);
        pause(isDead);
    }

    void pause(bool isPaused)
    {
        player.GetComponent<PlayerMovement>().enabled = !isPaused;
        player.GetComponent<Animator>().enabled = !isPaused;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // toggle if enemies are "alive" or not
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Animator>().enabled = !isPaused;
            var scripts = enemy.GetComponents<MonoBehaviour>();
            // toggle enemy's scripts
            foreach (var script in scripts)
            {
                script.enabled = !isPaused;
            }
        }
    }

    public void updateCheckpoint(Vector3 newPos)
    {
        checkpoint = newPos;
    }
}