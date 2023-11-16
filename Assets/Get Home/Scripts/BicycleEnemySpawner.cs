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

    // Start is called before the first frame update
    void Start()
    {
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

        // Define the spawn position to the right of the screen
        Vector3 spawnPosition = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0, 0)).x, spawnYPosition, 0);

        // Instantiate the object
        GameObject bicycleEnemy = Instantiate(bicycleGameObject, spawnPosition, Quaternion.identity);
        Destroy(bicycleEnemy, 5);
    }

}
