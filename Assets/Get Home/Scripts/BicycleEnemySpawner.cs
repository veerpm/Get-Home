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

    //SFX
    public AudioClip bellRight;
    public AudioClip bellLeft;
    public AudioSource bikeSounds;

    // Start is called before the first frame update
    void Start()
    {
        // modify volume to user's
        if (PlayerPrefs.HasKey("Volume"))
        {
            transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume");
        }

        //SFX
        if (toLeft)
        {
            bikeSounds.PlayOneShot(bellRight);
        }
        else
        {
            bikeSounds.PlayOneShot(bellLeft);
        }

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
            //SFX
        }
        else
        {
            // Define the spawn position to the left of the screen
            spawnPosition = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(-0.2f, 0, 0)).x, spawnYPosition, 0);
            //SFX
        }

        // Instantiate the object
        GameObject bicycleEnemy = Instantiate(bicycleGameObject, spawnPosition, Quaternion.identity);
        bicycleEnemy.GetComponent<BicycleEnemy>().toLeft = toLeft;
        bicycleEnemy.GetComponent<BicycleEnemy>().speed = speed;
        Destroy(bicycleEnemy, 5);
    }

}
