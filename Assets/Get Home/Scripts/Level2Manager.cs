using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueManager;

public class Level2Manager : MonoBehaviour
{
    private GameObject player;
    public bool activated1 = false;
    public bool activated2 = false;
    public bool activated3 = false;
    public bool activated5 = false;
    private float sensibility = 0.25f;
    public GameObject bicycleEnemySpawner;
    private GameObject bicycleEnemy;
    public GameObject enemyStop1;
    public GameObject enemyStop2;
    public GameObject enemyStop4;
    public GameObject enemyStop5;

    // variables for the checkmark location's enemies
    public bool activated4 = false;
    public GameObject enemyStop3;
    public GameObject bicycleGameObject;
    // var for (new) timer of bicycleSpawner
    public GameObject gameManager;
    private Coroutine checkmarkCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // bike enemies at cross mark
        if (enemyStop1.GetComponent<LockFrame>().locked && !activated1)
        {
            SpawnBicycle(3, Random.Range(1.5f, 3.5f), -3.47f, 2.55f, true, enemyStop1.GetComponent<LockFrame>().unlockTime, 5);
            activated1 = true;
        }

        // bike enemies for chase
        if (activated1 && bicycleEnemy == null && !enemyStop2.GetComponent<LockFrame>().EnemiesDefeated() && !activated3)
        {
            activated3 = true;
            StartCoroutine(Delay());
        }

        // bike enemies after chase
        if (enemyStop2.GetComponent<LockFrame>().locked && !activated2 && enemyStop1.GetComponent<LockFrame>().EnemiesDefeated())
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<BicycleEnemySpawner>() != null)
                {
                    Destroy(enemy);
                }
            }
            SpawnBicycle(4, Random.Range(1.5f, 3.5f), -3.47f, 2.55f, true, enemyStop2.GetComponent<LockFrame>().unlockTime, 5);
            activated2 = true;
        }

        // bike enemies at check mark
        if (!activated4 && enemyStop3.GetComponent<LockFrame>().locked)
        {
            // spawn enemies in a row
            checkmarkCoroutine = StartCoroutine(CheckmarkObstacle());
            activated4 = true;
        }

        // bike enemies right at end with other enemies
        if (!activated5 && enemyStop4.GetComponent<LockFrame>().EnemiesDefeated())
        {
            SpawnBicycle(1, Random.Range(1.5f, 3.5f), -3.47f, 2.55f, true, 0, 5);
            SpawnBicycle(1, Random.Range(1.5f, 3.5f), -3.47f, 2.55f, false, 0, 5);
            activated5 = true;
        }
        // no more spawning bikes after final enemies defeated
        if (enemyStop5.GetComponent<LockFrame>().EnemiesDefeated())
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<BicycleEnemySpawner>() != null)
                {
                    Destroy(enemy);
                }
            }
        }
    }

    void SpawnBicycle(int num, float spawnDelay, float spawnLowerBound, float spawnUpperBound, bool toLeft, float time, float speed)
    {
        for (int i = num; i > 0; i--)
        {
            bicycleEnemy = Instantiate(bicycleEnemySpawner);
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnDelay = spawnDelay;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnLowerBound = spawnLowerBound;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnUpperBound = spawnUpperBound;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().toLeft = toLeft;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().speed = speed;
            if (time > 0)
            {
                Destroy(bicycleEnemy, time);
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        SpawnBicycle(4, Random.Range(1.5f, 3.5f), -3.47f, 2.55f, false, 0, 8);
    }

    IEnumerator CheckmarkObstacle()
    {
        // spawn 3 waves of bicycles, 3 secs between each
        for(int i = 1; i<6; i+=2)
        {
            SpawnBicyclesRow(i);
            yield return new WaitForSeconds(3f);
        }
    }

    void SpawnBicyclesRow(int skippedSpot)
    {
        // spawn row of bicyclers
        float YIncrement = (3.47f + 2.55f) / 6;
        for (int i = 0; i < 6; i++)
        {
            // bicycles everywhere except at 1 y-location
            if(i != skippedSpot)
            {
                // spawn 2 bicycles beside each other at each row
                for(int j = 0; j<2; j++)
                {
                    float spawnYPosition = -3.47f + i * YIncrement;
                    float spawnXPosition = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0, 0)).x + j * 3f;

                    // Instantiate
                    Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, 0);
                    GameObject bicycleEnemy = Instantiate(bicycleGameObject, spawnPosition, Quaternion.identity);
                    bicycleEnemy.GetComponent<BicycleEnemy>().toLeft = true;
                    bicycleEnemy.GetComponent<BicycleEnemy>().speed = 5f;
                    Destroy(bicycleEnemy, 5);
                }
            }
        }
    }

    public void StopCheckCoroutine()
    {
        StopCoroutine(checkmarkCoroutine);
    }
}
