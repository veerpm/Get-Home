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
    private float sensibility = 0.25f;
    public GameObject bicycleEnemySpawner;
    private GameObject bicycleEnemy;
    public GameObject enemyStop1;
    public GameObject enemyStop2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStop1.GetComponent<LockFrame>().locked && !activated1)
        {
            SpawnBicycle(3, true, enemyStop1.GetComponent<LockFrame>().unlockTime, 5);
            activated1 = true;
        }
        if (activated1 && bicycleEnemy == null && !enemyStop2.GetComponent<LockFrame>().EnemiesDefeated() && !activated3)
        {
            SpawnBicycle(4, false, 1000,7);
            activated3 = true;
        }
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
            SpawnBicycle(4, true, enemyStop2.GetComponent<LockFrame>().unlockTime, 5);
            activated2 = true;
        }
    }

    void SpawnBicycle(int num, bool toLeft, float time, float speed)
    {
        for (int i = num; i > 0; i--)
        {
            bicycleEnemy = Instantiate(bicycleEnemySpawner);
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnDelay = Random.Range(1.5f, 3.5f);
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnLowerBound = -3.47f;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().spawnUpperBound = 2.55f;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().toLeft = toLeft;
            bicycleEnemy.GetComponent<BicycleEnemySpawner>().speed = speed;
            Destroy(bicycleEnemy, time);
        }
    }
}
