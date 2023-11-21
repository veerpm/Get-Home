using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikersBarks : MonoBehaviour
{
    public float waitTime;
    public float bubbleDuration;

    public GameObject gameManager;
    public TextAsset barksFile;

    private string[] barks;

    // Start is called before the first frame update
    void Start()
    {
        // read barks of bicycle enemies
        barks = barksFile.ToString().Split('\n');
        // start bubble timer
        StartCoroutine(AnotherBubble());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AnotherBubble()
    {
        while (true)
        {
            // get enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log(enemies);

            // spawn 1 chat bubble among all enemies present in the game
            if(enemies != null && enemies.Length != 0)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                int randBark = Random.Range(0, barks.Length);

                // ignore enemy spawner & restart loop
                if(enemy.name == "BicycleEnemySpawner(Clone)")
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }

                // create bubble
                gameManager.GetComponent<ChatManager>().CreateBubble(enemy, barks[randBark], bubbleDuration);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
