using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleEnemySpawner : MonoBehaviour
{

    public GameObject bicycleGameObject;
    public float spawnDelay = 5f;
    private float nextSpawnTime = 0f;
    public float spawnLowerBound;
    public float spawnUpperBound;
    public bool toLeft = true;
    public float speed;

    // var for chat bubbles of enemies
    /*
    private bool spawnBubble = true;
    private float bubbleFrequency = 2.5f;
    private float bubbleDuration = 2.5f;
    private string[] barks;
    public GameObject gameManager;
    public TextAsset barksFile;
    */

    // Start is called before the first frame update
    void Start()
    {
        /*
        // read barks of bicycle enemies
        barks = barksFile.ToString().Split('\n');
        // start bubble timer
        StartCoroutine(AnotherBubble());
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBicycle();
            // Calculate the next spawn time based on the spawnDelay
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    void SpawnBicycle()
    {
        float spawnYPosition = Random.Range(spawnLowerBound, spawnUpperBound);

        Vector3 spawnPosition;

        if (toLeft)
        {
            // Define the spawn position to the right of the screen
            spawnPosition = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0, 0)).x, spawnYPosition, 0);
        }
        else
        {
            // Define the spawn position to the left of the screen
            spawnPosition = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(-0.2f, 0, 0)).x, spawnYPosition, 0);
        }

        // Instantiate the object
        GameObject bicycleEnemy = Instantiate(bicycleGameObject, spawnPosition, Quaternion.identity);
        bicycleEnemy.GetComponent<BicycleEnemy>().toLeft = toLeft;
        bicycleEnemy.GetComponent<BicycleEnemy>().speed = speed;
        Destroy(bicycleEnemy, 5);

        // occasionally create chat bubbles
        /*
        if (spawnBubble)
        {
            Debug.Log(Time.time);
            Debug.Log(spawnBubble);
            spawnBubble = false;
            int randBark = Random.Range(0, barks.Length);
            gameManager.GetComponent<ChatManager>().CreateBubble(bicycleEnemy, barks[randBark], bubbleDuration);
        }
        */
    }

    /*
    // timer to check if we need another chat bubble
    IEnumerator AnotherBubble()
    {
        while (true)
        {
            spawnBubble = true;
            yield return new WaitForSeconds(bubbleFrequency);
        }
    }
    */

}
