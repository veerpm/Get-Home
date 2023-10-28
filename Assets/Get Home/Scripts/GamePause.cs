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
            if (Input.GetKeyDown("space"))
            {
                // alive
                bringBackAlive();
            }
        }
    }

    public void bringBackAlive()
    {

        // resets player position & health
        player.transform.position = checkpoint;
        player.GetComponent<PlayerHealth>().setFullHealth();

        // resets enemies position & health
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.name != "BicycleEnemy")
            {
                EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();

                if (healthScript != null) // checks if this enemy has healthscript
                {
                    healthScript.Alive();
                    healthScript.resetPosition();
                }

            }

        }
        // restart sound
        music.Play();

        // remove death scree & unpause
        dead = false;
        deathScreen.SetActive(false);
        pause(false);
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
        mainCamera.GetComponent<CameraMovement>().setFreeze(false);
        player.GetComponent<Boundaries>().unFreeze();
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
            if(script.GetType().Name == "DialogueManager")
            {
                continue;
            }
            script.enabled = !isPaused;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // toggle if enemies are "alive" or not
        foreach (GameObject enemy in enemies)
        {
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

    IEnumerator LoadScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}